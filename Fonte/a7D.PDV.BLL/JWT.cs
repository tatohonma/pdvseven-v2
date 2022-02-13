using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    internal static class JWT
    {
        public static readonly string CAIXA = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6ImNhaXhhIiwiaXNzIjoic2VsZiIsIm5iZiI6MTQ2MzQzMTc2N30.JR5Xe7HkHbh_H6k4eJUtMHt6lWqPUKuHNHCajV_PhZY";
        public static readonly string BACKOFFICE = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6ImJhY2tvZmZpY2UiLCJpc3MiOiJzZWxmIiwibmJmIjoxNDYzNDMyMTUzfQ.HUKVbtdcNKzcZXjHZB8a8OjMxQUtGnANxa069kgn3Ds";
        public static readonly string SERVER = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6InNlcnZlciIsImlzcyI6InNlbGYiLCJuYmYiOjE0NjM0MzIxODJ9.8Uw2JjVgPRxGOuuy9HPDwnubnuKNytuWvO_-ToQIPpw";

        internal static string Obter(TipoApp tipoApp)
        {
            switch (tipoApp)
            {
                case TipoApp.CAIXA:
                    return JWT.CAIXA;
                case TipoApp.BACKOFFICE:
                    return JWT.BACKOFFICE;
                case TipoApp.SERVER:
                    return JWT.SERVER;
                default:
                    return JWT.SERVER;
            }
        }
    }
}
