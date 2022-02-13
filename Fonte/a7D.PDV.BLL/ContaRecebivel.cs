using System;
using System.Collections.Generic;
using System.Linq;
using a7D.PDV.EF.Models;

namespace a7D.PDV.BLL
{
    public static class ContaRecebivel
    {
        public static List<tbContaRecebivel> Listar()
        {
            return EF.Repositorio.Listar<tbContaRecebivel>();
        }

        public static tbContaRecebivel Carregar(int id)
        {
            try
            {
                return EF.Repositorio.Carregar<tbContaRecebivel>(c => c.IDContaRecebivel == id);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EBB0, ex);
            }
        }

        public static tbContaRecebivel CarregarPorNome(string nome)
        {
            try
            {
                return Listar().FirstOrDefault(
                    c => c.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EBB2, ex);
            }
        }
    }
}