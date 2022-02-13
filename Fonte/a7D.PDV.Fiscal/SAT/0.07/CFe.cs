using a7D.PDV.Fiscal.Comunicacao.SAT;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace a7D.PDV.Fiscal.SAT._007
{
    [XmlRoot(ElementName = "CFe")]
    public class CFe : ICFeVenda
    {
        public _infCFe infCFe;

        public string GerarXMLVenda()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            XmlSerializer cs = new XmlSerializer(typeof(CFe));

            cs.Serialize(writer, this, names);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd().RemoveAcentos();
        }

    }

    public class _infCFe
    {
        [XmlAttributeAttribute()]
        public string versaoDadosEnt;

        public _ide ide;
        public _emit emit;
        public _dest dest;
        public _entrega entrega;

        [XmlElement("det", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public _det[] det;

        public _total total;
        public _pgto pgto;
        public _infAdic infAdic;
    }

    public class _ide
    {
        public string CNPJ;
        public string signAC;
        public string numeroCaixa;
    }

    public class _emit
    {
        public string CNPJ;
        public string IE;
        public string IM;
        public string cRegTribISSQN;
        public string indRatISSQN;
    }

    public class _dest
    {
        public string CNPJ;
        public string CPF;
        public string xNome;
    }

    public class _entrega
    {
        public string xLgr;
        public string nro;
        public string xCpl;
        public string xBairro;
        public string xMun;
        public string UF;
    }

    public class _det
    {
        [XmlAttribute]
        public string nItem;

        public _prod prod;
        public _imposto imposto;
        public string infAdProd;
    }

    public class _prod
    {
        public string cProd;
        public string cEAN;
        public string xProd;
        public string NCM;
        public string CFOP;
        public string CEST;
        public string uCom;
        public string qCom;
        public string vUnCom;
        public string indRegra;
        public string vDesc;
        public string vOutro;

        public _obsFiscoDet obsFiscoDet;
    }

    public class _obsFiscoDet
    {
        [XmlAttribute]
        public string xCampoDet;

        public string xTextoDet;
    }

    public class _imposto
    {
        [XmlElement(Order = 0)]
        public string vItem12741 { get; set; }

        [XmlElement(Order = 1)]
        public _ICMS ICMS;

        [XmlElement(Order = 2)]
        public _PIS PIS;

        [XmlElement(Order = 3)]
        public _COFINS COFINS;

        [XmlElement(Order = 4)]
        public _ISSQN ISSQN;
    }

    public class _ICMS
    {
        public _ICMS00 ICMS00;
        public _ICMS40 ICMS40;
        public _ICMSSN102 ICMSSN102;
        public _ICMSSN900 ICMSSN900;
    }

    public class _ICMS00
    {
        public string Orig;
        public string CST;
        public string pICMS;
    }

    public class _ICMS40
    {
        public string Orig;
        public string CST;
    }

    public class _ICMSSN102
    {
        public string Orig;
        public string CSOSN;
    }

    public class _ICMSSN900
    {
        public string Orig;
        public string CSOSN;
        public string pICMS;
    }

    public class _PIS
    {
        public _PISAliq PISAliq;
        public _PISQtde PISQtde;
        public _PISNT PISNT;
        public _PISSN PISSN;
        public _PISOutr PISOutr;
        public _PISST PISST;
    }

    public class _PISAliq
    {
        public string CST;
        public string vBC;
        public string pPIS;
        public string vPIS;
    }

    public class _PISQtde
    {
        public string CST;
        public string qBCProd;
        public string vAliqProd;
    }

    public class _PISNT
    {
        public string CST;
    }

    public class _PISSN
    {
        public string CST;
    }

    public class _PISOutr
    {
        public string CST;
        public string vBC;
        public string pPIS;
        public string qBCProd;
        public string vAliqProd;
    }

    public class _PISST
    {
        public string vBC;
        public string pPIS;
        public string qBCProd;
        public string vAliqProd;
    }

    public class _COFINS
    {
        public _COFINSAliq COFINSAliq;
        public _COFINSQtde COFINSQtde;
        public _COFINSNT COFINSNT;
        public _COFINSSN COFINSSN;
        public _COFINSOutr COFINSOutr;
        public _COFINSST COFINSST;
    }

    public class _COFINSAliq
    {
        public string CST;
        public string vBC;
        public string pCOFINS;
    }

    public class _COFINSQtde
    {
        public string CST;
        public string qBCProd;
        public string vAliqProd;
    }

    public class _COFINSNT
    {
        public string CST;
    }

    public class _COFINSSN
    {
        public string CST;
    }

    public class _COFINSOutr
    {
        public string CST;
        public string vBC;
        public string pCOFINS;
        public string qBCProd;
        public string vAliqProd;
    }

    public class _COFINSST
    {
        public string vBC;
        public string pCOFINS;
        public string qBCProd;
        public string vAliqProd;
    }

    public class _ISSQN
    {
        public string vDeducISSQN;
        public string vAliq;
        public string cMunFG;
        public string cListServ;
        public string cServTribMun;
        public string cNatOp;
        public string indIncFisc;
    }

    public class _total
    {
        public _DescAcrEntr DescAcrEntr;
    }

    public class _DescAcrEntr
    {
        public string vDescSubtot;
        public string vAcresSubtot;
        public string vCFeLei12741;
    }

    public class _pgto
    {
        [XmlElement("MP", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public _MP[] MP;
    }

    public class _MP
    {
        public string cMP;
        public string vMP;
        public string cAdmC;
    }

    public class _infAdic
    {
        public string infCpl;
    }
}
