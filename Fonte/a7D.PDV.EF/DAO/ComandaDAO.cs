using a7D.PDV.EF.Models;
using System;

namespace a7D.PDV.EF.DAO
{
    public enum ComandaNumeroTipo
    {
        Numero,
        TAGHEX,
        TAGDEC
    }
    public static class ComandaDAO
    {

        public static tbComanda Carregar(string numero, ComandaNumeroTipo tipo)
        {
            if (tipo == ComandaNumeroTipo.Numero && int.TryParse(numero, out int n))
                return Repositorio.Carregar<tbComanda>(c => c.Numero == n);
            else if (tipo == ComandaNumeroTipo.TAGDEC && long.TryParse(numero, out long l))
                return Repositorio.Carregar<tbComanda>(c => c.Codigo == l);
            else if (tipo == ComandaNumeroTipo.TAGHEX)
            {
                try
                {
                    long h = Convert.ToInt64(numero, 16);
                    return Repositorio.Carregar<tbComanda>(c => c.Codigo == h);
                }
                catch (Exception)
                {
                }
            }
            // Não existe ou erros de conversão (numero ou tag invalida)
            return null;
        }
    }
}
