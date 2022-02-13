using System;
using System.Collections.Generic;
using System.Linq;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao;

namespace a7D.PDV.BLL
{
    public static class Bandeira
    {
        public static List<tbBandeira> Listar()
        {
            return Repositorio.Listar<tbBandeira>();
        }

        public static tbBandeira Carregar(int id)
        {
            try
            {
                return Repositorio.Carregar<tbBandeira>(b => b.IDBandeira == id);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EBB0, ex);
            }
        }

        public static tbBandeira CarregarPorNome(string nome)
        {
            try
            {
                var nomeAux = "";
                if (string.IsNullOrEmpty(nome))
                    return null;

                nomeAux = nome.Replace(" ", "").ToUpper();
                if (nomeAux == "AMEX")
                    nomeAux = "AmericanExpress";
                else if (nomeAux == "MAESTROCP" || nomeAux == "MASTERCARDD")
                    nomeAux = "MastercardDébito";

                return Listar().FirstOrDefault(
                    c => c.Nome.Replace(" ", "").Equals(nomeAux, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EBB2, ex);
            }
        }
    }
}