using NFe.Utils.NFe;

namespace a7D.PDV.Fiscal.NFCe
{
    public class NFCe : ICFeVenda
    {
        public NFe.Classes.NFe nfe;

        public string GerarXMLVenda()
        {
            return nfe.ObterXmlString();
        }
    }
}
