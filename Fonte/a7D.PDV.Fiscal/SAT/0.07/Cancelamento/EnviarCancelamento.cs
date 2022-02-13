using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using System;

namespace a7D.PDV.Fiscal.SAT._007.Cancelamento
{
    public class EnviarCancelamento : EnviarSATBase, IEnviarCancelamento
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
            string retorno;
            CFeCanc cfeCanc = CFeCancelamento.CarregarCFeCanc(_retornoSat, out pedido);
            Random rnd = new Random();
            int numeroSessao = rnd.Next(1, 999999);

            //if (string.IsNullOrWhiteSpace(new ConfiguracoesSAT().InfCFe_urlServicoSAT))
            //    retorno = ComunicacaoSat.EnviarCancelamento(ObterCodigoDeAtivacao(), numeroSessao, _retornoSat.chaveConsulta, CFeCanc.GerarXMLCancelamento(cfeCanc));
            //else
            {
                using (var client = FiscalServices.SatApiClient().CancelamentoClient(ObterCodigoDeAtivacao(), numeroSessao, _retornoSat.chaveConsulta, CFeCanc.GerarXMLCancelamento(cfeCanc)))
                {
                    retorno = client.Enviar();
                }
            }

            var retornoCancelamento = RetornoSatFactory.GerarRetornoCancelamento(retorno);

            return retornoCancelamento;
        }

    }
}
