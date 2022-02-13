using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace a7D.PDV.Ativacao.WS.Model
{
    [Serializable]
    [XmlRoot(ElementName = "ListaPDV")]
    public class ListaPDVType
    {
        [XmlElement("PDV")]
        public PDVType[] ListaPDV { get; set; }

    }

    [XmlType("PDV")]
    public partial class PDVType
    {
        [XmlIgnore]
        private readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");

        public int IDPDV { get; set; }
        public string Version { get; set; }
        public int IDTipoPDV { get; set; }
        public string ChaveHardware { get; set; }
        public string Nome { get; set; }

        [XmlIgnore]
        public DateTime? UltimoAcessoDT { get; set; }
        [XmlIgnore]
        public DateTime? UltimaAlteracaoDT { get; set; }

        public string UltimoAcesso
        {
            get { return UltimoAcessoDT.HasValue ? UltimoAcessoDT.Value.ToString("yyyyMMddHHmmss") : string.Empty; }
            set
            {
                UltimoAcessoDT = string.IsNullOrWhiteSpace(value) == false ? DateTime.ParseExact(value, "yyyyMMddHHmmss", _cultureInfo) : null as DateTime?;
            }
        }

        public string UltimaAlteracao
        {
            get { return UltimaAlteracaoDT.HasValue ? UltimaAlteracaoDT.Value.ToString("yyyyMMddHHmmss") : string.Empty; }
            set
            {
                UltimaAlteracaoDT = string.IsNullOrWhiteSpace(value) == false ? DateTime.ParseExact(value, "yyyyMMddHHmmss", _cultureInfo) : null as DateTime?;
            }
        }
    }
}