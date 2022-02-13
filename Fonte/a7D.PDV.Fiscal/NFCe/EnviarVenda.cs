using a7D.PDV.BLL;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using NFe.Classes;
using NFe.Servicos.Retorno;
using NFe.Utils;
using System;
using System.IO;
using System.Text;

namespace a7D.PDV.Fiscal.NFCe
{
    public class EnviarVenda : IEnviarVenda
    {
        private const string fonte = "PDV-SAT|EnviarVenda";
        private PedidoInformation _pedido;
        private bool _cpfNaNota;
        private NFCe nfce;
        private ProcessamentoSATInformation _processamentoSat;
        private string retornoSAT;
        private string protCod;

        public EnviarVenda(PedidoInformation pedido, bool cpfNaNota, int idPdv, int idUsuario)
        {
            _pedido = pedido;
            _cpfNaNota = cpfNaNota;
            PrepararDados();
        }

        private void PrepararDados()
        {
            string xml = null;
            try
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
                        XMLEnvio = ""
                    };
                    ProcessamentoSAT.Salvar(_processamentoSat);
                }
                else if (_processamentoSat.IDStatusProcessamentoSAT == (int)EStatusProcessamentoSAT.SUCESSO)
                {
                    // TODO: Cancelar? ou reemitir? Validar se houve alterações
                    retornoSAT = null;
                    return;
                }
                else
                {
                    _processamentoSat.DataSolicitacao = DateTime.Now;
                    _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.NAO_INICIADO;
                }

                nfce = NFCeVenda.CarregarCFe(_pedido, _processamentoSat.IDProcessamentoSAT.Value, _cpfNaNota);

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
                    throw new ExceptionPDV(CodigoErro.E518, ex);
                }

                xml = nfce.GerarXMLVenda();

                try
                {
                    nfce.nfe = NFeFacade.Validar(nfce.nfe);
                }
                catch (Exception ex)
                {
                    throw new ExceptionPDV(CodigoErro.E519, ex);
                }

                xml = nfce.GerarXMLVenda();

                _processamentoSat.XMLEnvio = xml;
                _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.PROCESSANDO;
                ProcessamentoSAT.Salvar(_processamentoSat);

                if (!string.IsNullOrEmpty(NFeFacade.Config.NFCe_SalvarXML))
                {
                    File.WriteAllText(Path.Combine(NFeFacade.Config.NFCe_SalvarXML, "NFCe-" + nfce.nfe.infNFe.ide.nNF + ".xml"), xml);
                }

                string xMotivo;
                string mensagem;
                RetornoNFeAutorizacao retorno = null;
                try
                {
                    retorno = NFeFacade.Enviar(nfce.nfe);
                    mensagem = retorno.Retorno.xMotivo;
                    xMotivo = "online";
                }
                catch (Exception ex)
                {
                    throw new ExceptionPDV(CodigoErro.E517, ex.Message);
                    //xml = NFeFacade.EmitirOffline(nfce.nfe, ex.Message);
                    //xMotivo = "offline";
                    //mensagem = "Offline: " + ex.Message;
                }

                if (retorno != null)
                {
                    var proc = new nfeProc
                    {
                        NFe = nfce.nfe,
                        protNFe = retorno.Retorno.protNFe,
                        versao = nfce.nfe.infNFe.versao
                    };

                    protCod = proc.protNFe.infProt.nProt;
                    xMotivo = proc.protNFe.infProt.xMotivo;
                    if (protCod == null)
                    {
                        _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.ERRO;
                        ProcessamentoSAT.Salvar(_processamentoSat);
                        throw new ExceptionPDV(CodigoErro.E516, xMotivo);
                    }

                    xml = proc.ObterXmlString();
                }

                // numeroSessao|EEEEE|CCCC|mensagem|cod|mensagemSEFAZ|arquivoCFeBase64|timeS tamp|chaveConsulta|valorTotalCFe|CPFCNPJValue|assinaturaQRCODE
                var b64xml = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(xml));
                retornoSAT = $"0|06000|CCCC|{xMotivo}|{protCod}|{mensagem}|{b64xml}|{DateTime.Now.ToString("yyyyMMddHHmmss")}|{nfce.nfe.infNFe.Id.Substring(3)}|{nfce.nfe.infNFe.total.ICMSTot.vNF}|{_pedido.DocumentoCliente}|{nfce.nfe.infNFeSupl.qrCode}";

                _processamentoSat.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.SUCESSO;
                ProcessamentoSAT.Salvar(_processamentoSat);
            }
            catch (ExceptionPDV ex)
            {
                // https://jsonformatter.org/xml-formatter
                ex.Data.Add("xml", xml);
                throw ex;
            }
            catch (Exception ex)
            {
                ex.Data.Add("xml", xml);
                throw new ExceptionPDV(CodigoErro.E510, ex);
            }
        }

        public RetornoSATInformation Enviar()
        {
            ProcessamentoSAT.AlterarStatus(_processamentoSat.IDProcessamentoSAT.Value, EStatusProcessamentoSAT.PROCESSANDO);
            try
            {
                if (retornoSAT == null)
                {
                    return RetornoSAT.Carregar(_processamentoSat.IDRetornoSAT.Value);
                }
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
    }
}
