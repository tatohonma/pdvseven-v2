using a7D.PDV.EF.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace a7D.PDV.EF.DAO
{
    public static class CategoriaDAO
    {
        public static tbCategoriaProduto[] Listar(DateTime? dtAlteracao = null)
        {
            // TODO: Ativação de categoria ainda não foi implementado no backoffice
            Expression<Func<tbCategoriaProduto, bool>> whereCategoria;

            if (dtAlteracao != null)
            {
                var date = dtAlteracao.Value;
                whereCategoria = c => c.Excluido == false && c.DtUltimaAlteracao >= date;
            }
            else
            {
                whereCategoria = c => c.Excluido == false;
            }

            return Repositorio.Listar(whereCategoria).ToArray();
        }
    }
}
