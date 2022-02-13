using a7D.PDV.Fiscal.Comunicacao.SAT;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace a7D.PDV.Fiscal.SAT._007.Cancelamento
{
    [XmlRoot(ElementName = "CFeCanc")]
    public class CFeCanc : ICFeCanc
    {
        public _infCFe infCFe;

        public static string GerarXMLCancelamento(CFeCanc _cfeCanc)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            XmlSerializer cs = new XmlSerializer(typeof(CFeCanc));

            cs.Serialize(writer, _cfeCanc, names);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd().RemoveAcentos();
        }

    }

    public class _infCFe
    {
        [XmlAttribute]
        public string chCanc;

        public _ide ide;
        public _emit emit;
        public _dest dest;
        public _total total;
    }

    public class _ide
    {
        public string CNPJ;
        public string signAC;
        public string numeroCaixa;
    }

    public class _emit
    {

    }

    public class _dest
    {
        
    }

    public class _total { }
}
