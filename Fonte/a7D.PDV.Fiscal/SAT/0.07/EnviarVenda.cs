using a7D.PDV.BLL;
using a7D.PDV.Fiscal.SAT._007.Cancelamento;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using System;
using System.Runtime.ExceptionServices;
using System.Text;

namespace a7D.PDV.Fiscal.SAT._007
{
    public class EnviarVenda : EnviarSATBase, IEnviarVenda
    {
        private const string fonte = "PDV-SAT|EnviarVenda";

        private PedidoInformation _pedido;
        private string retornoSat;
        private CFe cfe;
        private ProcessamentoSATInformation _processamentoSat;
        private int _idPdv;
        private int _idUsuario;
        private bool _cpfNaNota;

        public EnviarVenda(PedidoInformation pedido, bool cpfNaNota, int idPdv, int idUsuario)
        {
            _pedido = pedido;
            _processamentoSat = ProcessamentoSAT.Carregar(_pedido.GUIDMovimentacao, ETipoSolicitacaoSAT.ENVIAR_DADOS_VENDA);
            _idPdv = idPdv;
            _idUsuario = idUsuario;
            _cpfNaNota = cpfNaNota;
        }

        [HandleProcessCorruptedStateExceptions]
        public RetornoSATInformation Enviar()
        {
            VerificarEnvioAnterior();
            PrepararDados();
            ProcessamentoSAT.AlterarStatus(_processamentoSat.IDProcessamentoSAT.Value, EStatusProcessamentoSAT.PROCESSANDO);
            try
            {
                using (var client = FiscalServices.SatApiClient().VendaClient(ObterCodigoDeAtivacao(), cfe, _processamentoSat.NumeroSessao.Value))
                {
                    retornoSat = client.Enviar();
                }

                ProcessamentoSAT.AlterarStatus(_processamentoSat.IDProcessamentoSAT.Value, EStatusProcessamentoSAT.PROCESSANDO);
                var result = RetornoSatFactory.GerarRetornoVenda(retornoSat, true);

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

        private void VerificarEnvioAnterior()
        {
            try
            {
                if (_processamentoSat != null && _processamentoSat.NumeroSessao.HasValue)
                {
                    string strRet = string.Empty;
                    using (var client = FiscalServices.SatApiClient().ConsultaClient(ObterCodigoDeAtivacao(), _processamentoSat.NumeroSessao.Value))
                    {
                        strRet = client.Enviar();
                    }

                    try
                    {
                        var retornoSat = RetornoSatFactory.GerarRetornoVenda(strRet, true);
                        new EnviarCancelamento(retornoSat, _idPdv, _idUsuario).Enviar(out PedidoInformation pedido);
                    }
                    catch
                    { }
                }
                else
                {
                    if (_processamentoSat == null)
                    {
                        _processamentoSat = new ProcessamentoSATInformation
                        {
                            GUID = _pedido.GUIDMovimentacao,
                            DataSolicitacao = DateTime.Now,
                            IDTipoSolicitacaoSAT = (int)ETipoSolicitacaoSAT.ENVIAR_DADOS_VENDA,
                            IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.NAO_INICIADO,
                            XMLEnvio = string.Empty
                        };
                        ProcessamentoSAT.Salvar(_processamentoSat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E511, ex);
            }
        }

        private void PrepararDados()
        {
            try
            {
                cfe = CFeVenda.CarregarCFe(_pedido, _cpfNaNota);

                Random rnd = new Random();
                _processamentoSat.NumeroSessao = rnd.Next(1, 999999);
                _processamentoSat.XMLEnvio = Encoding.GetEncoding("iso-8859-1").GetString(Encoding.UTF8.GetBytes(cfe.GerarXMLVenda()));

                ProcessamentoSAT.Salvar(_processamentoSat);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E510, ex);
            }
        }
    }
}
