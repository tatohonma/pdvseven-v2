using a7D.PDV.Model;
using NFe.Classes;
using NFe.Classes.Servicos.Download;
using System;
using System.Text;

namespace a7D.PDV.Fiscal.NFCe
{
    public class EnviarCancelamento : IEnviarCancelamento
    {
        private int _idPdv;
        private int _idUsuario;
        private RetornoSATInformation _retornoSat;

        public EnviarCancelamento(RetornoSATInformation retornoSat, int idPdv, int idUsuario)
        {
            _retornoSat = retornoSat;
            _idPdv = idPdv;
            _idUsuario = idUsuario;
        }

        public RetornoSATInformation Enviar(out PedidoInformation pedido)
        {
            string arquivo = Encoding.UTF8.GetString(Convert.FromBase64String(_retornoSat.arquivoCFeSAT));
            nfeProc proc = new procNFe().nfeProc.CarregarDeXmlString(arquivo);
            var result = NFeFacade.Cancelar(proc.protNFe, $"Cancelamento por PDV: {_idPdv} USER: {_idUsuario}");
            if (result.Retorno.retEvento == null || result.Retorno.retEvento.Count == 0)
                throw new Exception("Sem retorno de cancelamento");

            var cancelamento = result.Retorno.retEvento[0].infEvento;
            var b64xml = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(result.RetornoCompletoStr));
            string retorno = $"0|07000|CCCC|{cancelamento.xMotivo}|{cancelamento.nProt}|cancelamento|{b64xml}|{DateTime.Now.ToString("yyyyMMddHHmmss")}|{_retornoSat.chaveConsulta}|{_retornoSat.valorTotalCFe}|{_retornoSat.CPFCNPJValue}|{_retornoSat.assinaturaQRCODE}";

            pedido = BLL.Pedido.CarregarPorIdRetornoSatVenda(_retornoSat.IDRetornoSAT.Value);
            var retornoCancelamento = PDV.SAT.RetornoSatFactory.GerarRetornoCancelamento(retorno);

            return retornoCancelamento;
        }
    }
}
