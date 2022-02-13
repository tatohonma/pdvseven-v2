using System;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.EF.Models
{
    public class tbVersao
    {
        public int IDVersao { get; set; }

        public string Versao { get; set; }

        public DateTime Data { get; set; }

        public Version ToVersion() => new Version(Versao);

    }
}
