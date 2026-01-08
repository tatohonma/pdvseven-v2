using a7D.PDV.BLL;
using a7D.PDV.Model;
using NFe.Classes;
using NFe.Classes.Servicos.Download;
using System;
using System.Text;

namespace a7D.PDV.Fiscal.NFCe
{
    public class EnviarCancelamento : IEnviarCancelamento
    {
        private readonly int _idPdv;
        private readonly int _idUsuario;
        private readonly RetornoSATInformation _retornoSatVenda;

        private ProcessamentoSATInformation _processamento;

        public EnviarCancelamento(RetornoSATInformation retornoSat, int idPdv, int idUsuario)
        {
            _retornoSatVenda = retornoSat;
            _idPdv = idPdv;
            _idUsuario = idUsuario;
        }

        public RetornoSATInformation Enviar(out PedidoInformation pedido)
        {
            pedido = null;

            string guid = GerarGuidCancelamento(_retornoSatVenda.chaveConsulta);

            _processamento = ProcessamentoSAT.Carregar(guid, ETipoSolicitacaoSAT.CANCELAR_VENDA);
            if (_processamento == null)
            {
                _processamento = new ProcessamentoSATInformation
                {
                    GUID = guid,
                    DataSolicitacao = DateTime.Now,
                    IDTipoSolicitacaoSAT = (int)ETipoSolicitacaoSAT.CANCELAR_VENDA,
                    IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.NAO_INICIADO,
                    XMLEnvio = ""
                };

                ProcessamentoSAT.Salvar(_processamento);
            }
            else if (_processamento.IDStatusProcessamentoSAT == (int)EStatusProcessamentoSAT.SUCESSO
                     && _processamento.IDRetornoSAT.HasValue)
            {
                return RetornoSAT.Carregar(_processamento.IDRetornoSAT.Value);
            }
            else
            {
                _processamento.DataSolicitacao = DateTime.Now;
                _processamento.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.NAO_INICIADO;
                ProcessamentoSAT.Salvar(_processamento);
            }

            ProcessamentoSAT.AlterarStatus(_processamento.IDProcessamentoSAT.Value, EStatusProcessamentoSAT.PROCESSANDO);

            try
            {
                string arquivoVenda = Encoding.UTF8.GetString(Convert.FromBase64String(_retornoSatVenda.arquivoCFeSAT));
                nfeProc proc = new procNFe().nfeProc.CarregarDeXmlString(arquivoVenda);

                _processamento.XMLEnvio =
                    $"<cancelamentoEnv><chNFe>{_retornoSatVenda.chaveConsulta}</chNFe><nProt>{proc.protNFe.infProt.nProt}</nProt><motivo>Cancelamento por PDV: {_idPdv} USER: {_idUsuario}</motivo></cancelamentoEnv>";
                ProcessamentoSAT.Salvar(_processamento);

                var result = NFeFacade.Cancelar(proc.protNFe, $"Cancelamento por PDV: {_idPdv} USER: {_idUsuario}");
                if (result.Retorno?.retEvento == null || result.Retorno.retEvento.Count == 0)
                    throw new Exception("Sem retorno de cancelamento");

                var inf = result.Retorno.retEvento[0].infEvento;

                var b64xml = Convert.ToBase64String(Encoding.UTF8.GetBytes(result.RetornoCompletoStr));

                string retorno =
                    $"0|07000|CCCC|{inf.xMotivo}|{inf.nProt}|cancelamento|{b64xml}|{DateTime.Now:yyyyMMddHHmmss}|{_retornoSatVenda.chaveConsulta}|{_retornoSatVenda.valorTotalCFe}|{_retornoSatVenda.CPFCNPJValue}|{_retornoSatVenda.assinaturaQRCODE}";

                pedido = BLL.Pedido.CarregarPorIdRetornoSatVenda(_retornoSatVenda.IDRetornoSAT.Value);

                var retornoCancelamento = PDV.SAT.RetornoSatFactory.GerarRetornoCancelamento(retorno);

                RetornoSAT.Salvar(retornoCancelamento);

                _processamento.IDRetornoSAT = retornoCancelamento.IDRetornoSAT;
                _processamento.IDStatusProcessamentoSAT = (int)EStatusProcessamentoSAT.SUCESSO;
                ProcessamentoSAT.Salvar(_processamento);

                return retornoCancelamento;
            }
            catch (Exception ex)
            {
                ex.Data.Add("_processamento.XMLEnvio", _processamento?.XMLEnvio);
                ProcessamentoSAT.AlterarStatus(_processamento.IDProcessamentoSAT.Value, EStatusProcessamentoSAT.ERRO);
                throw new ExceptionPDV(CodigoErro.E512, ex);
            }
        }

        private static string GerarGuidCancelamento(string chaveConsulta)
        {
            string baseStr = $"CANC|{(chaveConsulta ?? "").Trim()}";

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(baseStr);
                var hash = md5.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
}
