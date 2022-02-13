using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{



    public class PedidoProdutoEqualityComparer : IEqualityComparer<PedidoProdutoInformation>
    {
        private static PedidoProdutoEqualityComparer _instance;
        public static PedidoProdutoEqualityComparer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PedidoProdutoEqualityComparer();
                return _instance;
            }
        }

        public bool Equals(PedidoProdutoInformation x, PedidoProdutoInformation y)
        {
            var equal = true;

            equal = equal && x.Produto.IDProduto.Value == y.Produto.IDProduto.Value;
            equal = equal && x.Quantidade == y.Quantidade;
            equal = equal && x.Cancelado == y.Cancelado;
            equal = equal && ListsAreEqual(x.ListaModificacao, y.ListaModificacao);
            return equal;
        }

        public int GetHashCode(PedidoProdutoInformation obj)
        {
            return obj.GetHashCode();
        }

        private static bool ListsAreEqual(List<PedidoProdutoInformation> x, List<PedidoProdutoInformation> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null && y?.Count == 0)
                return true;

            if (x?.Count == 0 && y == null)
                return true;

            if (x == null && y != null)
                return false;

            if (x != null && y == null)
                return false;

            if (x.Count != y.Count)
                return false;

            return x.All(m => y.Contains(m, Instance)) && y.All(n => x.Contains(n, Instance));
        }
    }
}
