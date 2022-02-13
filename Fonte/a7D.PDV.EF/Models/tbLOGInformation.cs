using System;

namespace a7D.PDV.EF.Models
{
    public class tbLOGInformation
    {
        public int IDLOG { get; set; }

        public DateTime Data { get; set; }

        public string Aplicacao { get; set; }

        public string Versao { get; set; }

        public string Codigo { get; set; }

        public string Titulo { get; set; }

        public string Dados { get; set; }

        public int? IDUsuario{ get; set; }

        public int? IDPDV { get; set; }
    }
}