using a7D.PDV.BLL;
using a7D.PDV.Fiscal.SAT;
using a7D.PDV.Model;
using System;

namespace a7D.PDV.Fiscal.Services
{
    public static class FiscalServices
    {
        internal static ConfiguracoesSAT ConfigSAT { get; private set; }

        internal static ConfiguracoesNFCe ConfigNFCe { get; private set; }

        internal static string urlWSfiscal { get; }

        private static EVersaoCFeSAT VersaoCfe { get; }

        private static EVersaoCFeSAT ObterVersao(string versao)
        {
            switch (versao)
            {
                case "0.07":
                    return EVersaoCFeSAT._007;
                case "0.08":
                    return EVersaoCFeSAT._008;
                default:
                    return EVersaoCFeSAT._000;
            }
        }

        static FiscalServices()
        {
            if (ConfiguracoesSistema.Valores.Fiscal == "SAT")
            {
                ConfigSAT = new ConfiguracoesSAT();
                VersaoCfe = ObterVersao(ConfigSAT.InfCFe_versaoDadosEnt);
                urlWSfiscal = ConfigSAT.InfCFe_urlServicoSAT;
            }
            else if (ConfiguracoesSistema.Valores.Fiscal == "NFCe")
            {
                ConfigNFCe = new ConfiguracoesNFCe();
                urlWSfiscal = ConfigNFCe.NFCe_urlServico;
            }

            if (string.IsNullOrEmpty(urlWSfiscal))
            {
                throw new ExceptionPDV(CodigoErro.E507);
            }
        }

        public static IFiscalApiClient SatApiClient()
        {
            return new FiscalApiClient(urlWSfiscal);
        }

        public static IEnviarVenda Venda(PedidoInformation pedido, bool cpfNaNota, int idPdv, int idUsuario)
        {
            if (ConfiguracoesSistema.Valores.Fiscal == "SAT")
            {
                switch (VersaoCfe)
                {
                    case EVersaoCFeSAT._007:
                        return new SAT._007.EnviarVenda(pedido, cpfNaNota, idPdv, idUsuario);
                    case EVersaoCFeSAT._008:
                        return new SAT._008.EnviarVenda(pedido, cpfNaNota, idPdv, idUsuario);
                    default:
                        throw new CFeException();
                }
            }
            else
            {
                return new NFCe.EnviarVenda(pedido, cpfNaNota, idPdv, idUsuario);
            }
        }

        public static IEnviarCancelamento Cancelamento(RetornoSATInformation retornoSat, int idPdv, int idUsuario)
        {
            if (ConfiguracoesSistema.Valores.Fiscal == "SAT")
            {
                switch (VersaoCfe)
                {
                    case EVersaoCFeSAT._007:
                        return new SAT._007.Cancelamento.EnviarCancelamento(retornoSat, idPdv, idUsuario);
                    case EVersaoCFeSAT._008:
                        return new SAT._008.Cancelamento.EnviarCancelamento(retornoSat, idPdv, idUsuario);
                    default:
                        throw new CFeException();
                }
            }
            else
            {
                return new NFCe.EnviarCancelamento(retornoSat, idPdv, idUsuario);
            }
        }
    }
}
