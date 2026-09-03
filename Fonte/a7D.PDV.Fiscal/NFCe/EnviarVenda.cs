using a7D.PDV.BLL;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using NFe.Classes;
using NFe.Servicos.Retorno;
using NFe.Utils;
using System;
using System.IO;
using System.Text;
using a7D.PDV.BLL.Services;

namespace a7D.PDV.Fiscal.NFCe
{
    public class EnviarVenda : IEnviarVenda
    {
        private const string fonte = "PDV-SAT|EnviarVenda";
        private readonly PedidoInformation _pedido;
        private readonly bool _cpfNaNota;
        readonly string _contabilidadeCpfCnpj;

        private NFCe nfce;
        private ProcessamentoSATInformation _processamentoSat;
        private string retornoSAT;
        private string protCod;

        public EnviarVenda(PedidoInformation pedido, bool cpfNaNota, int idPdv, int idUsuario, string contabilidadeCpfCnpj)
        {
            _pedido = pedido;
            _cpfNaNota = cpfNaNota;
            _contabilidadeCpfCnpj = contabilidadeCpfCnpj;

            GarantirProcessamentoSat();
        }

        private void GarantirProcessamentoSat()
        {
            _processamentoSat = ProcessamentoSAT.Carregar(_pedido.GUIDMovimentacao, ETipoSolicitacaoSAT.ENVIAR_DADOS_VENDA);

            if (_processamentoSat == null)
            {
                _processamentoSat = new ProcessamentoSATInformation
                {
                    GUID = _pedido.GUIDMovimentacao,
                    DataSolicitacao = DateTime.Now,
                    IDTipoSolicitacaoSAT = (int)ETipoSolicitacaoSAT.ENVIAR_DADOS_VENDA,
                    IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.NAO_INICIADO,
                    XMLEnvio = "",
                    NumeroFiscalSequencial = null,
                    SerieFiscal = null
                };

                ProcessamentoSAT.Salvar(_processamentoSat);
                return;
            }

            if (_processamentoSat.IDStatusProcessamentoSAT == (int)EStatusProcessamentoSAT.SUCESSO)
                return;

            _processamentoSat.DataSolicitacao = DateTime.Now;
            _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.NAO_INICIADO;
            ProcessamentoSAT.Salvar(_processamentoSat);
        }

        private void PrepararDados()
        {
            string xml = null;

            try
            {
                GarantirProcessamentoSat();

                if (_processamentoSat.IDStatusProcessamentoSAT == (int)EStatusProcessamentoSAT.SUCESSO)
                {
                    retornoSAT = null;
                    return;
                }

                ReservarNumeracaoSeNecessario();

                nfce = NFCeVenda.CarregarCFe(_pedido, _processamentoSat.NumeroFiscalSequencial.Value, _cpfNaNota, _contabilidadeCpfCnpj);

                if (ConfiguracaoServico.Instancia.tpAmb == DFe.Classes.Flags.TipoAmbiente.Homologacao)
                {
                    nfce.nfe.infNFe.det[0].prod.xProd = "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
                    if (nfce.nfe.infNFe.dest != null)
                        nfce.nfe.infNFe.dest.xNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
                }

                xml = nfce.GerarXMLVenda();

                try
                {
                    nfce.nfe = NFeFacade.Assinar(nfce.nfe);
                }
                catch (Exception ex)
                {
                    LiberarNumeracaoPreEnvio();
                    throw new ExceptionPDV(CodigoErro.E518, ex);
                }

                xml = nfce.GerarXMLVenda();

                try
                {
                    nfce.nfe = NFeFacade.Validar(nfce.nfe);
                }
                catch (Exception ex)
                {
                    LiberarNumeracaoPreEnvio();
                    throw new ExceptionPDV(CodigoErro.E519, ex);
                }

                xml = nfce.GerarXMLVenda();

                _processamentoSat.XMLEnvio = xml;
                _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.PROCESSANDO;
                ProcessamentoSAT.Salvar(_processamentoSat);

                if (!string.IsNullOrEmpty(NFeFacade.Config.NFCe_SalvarXML))
                {
                    File.WriteAllText(
                        Path.Combine(NFeFacade.Config.NFCe_SalvarXML, "NFCe-" + nfce.nfe.infNFe.ide.nNF + ".xml"),
                        xml
                    );
                }

                string xMotivo;
                string mensagem;
                RetornoNFeAutorizacao retorno;

                try
                {
                    retorno = NFeFacade.Enviar(nfce.nfe);
                    mensagem = retorno.Retorno.xMotivo;
                    xMotivo = "online";
                }
                catch (Exception ex)
                {
                    // se for erro de rede/local, libera a numeração
                    if (EhErroLocalDeRede(ex))
                    {
                        LiberarNumeracaoPreEnvio();

                        // opcional: limpa também o XML pra não ficar "processando" com nNF definido
                        _processamentoSat.XMLEnvio = "";
                        _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.NAO_INICIADO;
                        ProcessamentoSAT.Salvar(_processamentoSat);
                    }
                    else
                    {
                        // se não dá pra afirmar que não chegou na SEFAZ, mantém número reservado
                        _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.ERRO;
                        ProcessamentoSAT.Salvar(_processamentoSat);
                    }

                    throw new ExceptionPDV(CodigoErro.E517, ex.Message);
                }
                var proc = new nfeProc
                {
                    NFe = nfce.nfe,
                    protNFe = retorno.Retorno.protNFe,
                    versao = nfce.nfe.infNFe.versao
                };

                if (proc.protNFe?.infProt == null)
                {
                    _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.ERRO;
                    ProcessamentoSAT.Salvar(_processamentoSat);
                    throw new ExceptionPDV(CodigoErro.E516, "A SEFAZ não retornou o protocolo da NFC-e.");
                }

                var infProt = proc.protNFe.infProt;
                protCod = infProt.nProt;
                xMotivo = infProt.xMotivo;

                // A presença de nProt não significa autorização. Algumas rejeições,
                // como duplicidade, também podem trazer esse valor no retorno.
                // Somente cStat 100 autoriza o uso da NFC-e.
                if (infProt.cStat != 100 || string.IsNullOrEmpty(protCod))
                {
                    _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.ERRO;
                    ProcessamentoSAT.Salvar(_processamentoSat);
                    throw new ExceptionPDV(CodigoErro.E516,
                        $"SEFAZ cStat {infProt.cStat}: {xMotivo}");
                }

                xml = proc.ObterXmlString();

                var b64xml = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(xml));
                retornoSAT =
                    $"0|06000|CCCC|{xMotivo}|{protCod}|{mensagem}|{b64xml}|{DateTime.Now:yyyyMMddHHmmss}|{nfce.nfe.infNFe.Id.Substring(3)}|{nfce.nfe.infNFe.total.ICMSTot.vNF}|{_pedido.DocumentoCliente}|{nfce.nfe.infNFeSupl.qrCode}";

                _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.SUCESSO;
                ProcessamentoSAT.Salvar(_processamentoSat);
            }
            catch (ExceptionPDV ex)
            {
                ex.Data.Add("xml", xml);
                throw;
            }
            catch (Exception ex)
            {
                ex.Data.Add("xml", xml);
                throw new ExceptionPDV(CodigoErro.E510, ex);
            }
        }

        private void ReservarNumeracaoSeNecessario()
        {
            if (_processamentoSat.NumeroFiscalSequencial.HasValue)
                return;

            int numeroFiscal = NumeroFiscalService.ObterProximoNumero("NFCE");
            _processamentoSat.NumeroFiscalSequencial = numeroFiscal;

            _processamentoSat.SerieFiscal = int.TryParse(ConfiguracaoBD.BuscarConfiguracao("NFCe_Serie").Valor, out var serie) ? serie : (int?)null;

            ProcessamentoSAT.Salvar(_processamentoSat);
        }

        private void LiberarNumeracaoPreEnvio()
        {
            if (_processamentoSat?.IDProcessamentoSAT == null)
                return;

            _processamentoSat.NumeroFiscalSequencial = null;
            _processamentoSat.SerieFiscal = null;
            _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.ERRO;
            ProcessamentoSAT.Salvar(_processamentoSat);
        }

        public RetornoSATInformation Enviar()
        {
            PrepararDados();

            // ProcessamentoSAT.AlterarStatus(_processamentoSat.IDProcessamentoSAT.Value, EStatusProcessamentoSAT.PROCESSANDO);

            try
            {
                if (retornoSAT == null)
                    return RetornoSAT.Carregar(_processamentoSat.IDRetornoSAT.Value);

                var result = RetornoSatFactory.GerarRetornoVenda(retornoSAT, true);

                _processamentoSat.IDRetornoSAT = result.IDRetornoSAT;
                _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.SUCESSO;
                ProcessamentoSAT.Salvar(_processamentoSat);

                return result;
            }
            catch (Exception ex)
            {
                ex.Data.Add("_processamentoSat.XMLEnvio", _processamentoSat.XMLEnvio);
                ProcessamentoSAT.AlterarStatus(_processamentoSat.IDProcessamentoSAT.Value, EStatusProcessamentoSAT.ERRO);
                throw new ExceptionPDV(CodigoErro.E512, ex);
            }
        }
        
        static bool EhErroLocalDeRede(Exception ex)
        {
            if (ex is System.Net.WebException) return true;
            if (ex is System.Net.Http.HttpRequestException) return true;

            var inner = ex.InnerException;
            while (inner != null)
            {
                if (inner is System.Net.Sockets.SocketException) return true;
                if (inner is System.IO.IOException) return true;
                inner = inner.InnerException;
            }

            var msg = (ex.Message ?? "").ToLowerInvariant();
            return msg.Contains("name resolution") ||
                   msg.Contains("could not resolve") ||
                   msg.Contains("timed out") ||
                   msg.Contains("timeout") ||
                   msg.Contains("forcibly closed") ||
                   msg.Contains("conex") ||
                   msg.Contains("internet");
        }

    }
}
