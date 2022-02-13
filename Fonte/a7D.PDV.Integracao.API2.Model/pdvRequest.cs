namespace a7D.PDV.Integracao.API2.Model
{
    public class pdvRequest
    {
        public int tipoPDV;
        public string hardware;
        public string versaoAPP;

        public pdvRequest()
        {
        }

        public pdvRequest(int tipo, string chaveHardware, string versao)
        {
            tipoPDV = tipo;
            hardware = chaveHardware;
            versaoAPP = versao;
        }
    }
}
