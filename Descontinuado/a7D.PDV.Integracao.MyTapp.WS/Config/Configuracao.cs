using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.MyTapp.WS.Config
{
    public static class Configuracao
    {
        public static TipoIntegracao TipoIntegracao
        {
            get
            {
                var codigo = ConfigurationManager.AppSettings["Codigo"];
                switch (codigo)
                {
                    case "1":
                        return TipoIntegracao.INTEGRACAO1;
                    case "2":
                        return TipoIntegracao.INTEGRACAO2;
                    default:
                        return TipoIntegracao.CODIGO;
                }
            }
        }
    }
}