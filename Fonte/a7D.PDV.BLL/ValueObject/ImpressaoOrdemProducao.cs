using a7D.PDV.Model;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.BLL.ValueObject
{
    public class ImpressaoOrdemProducao
    {
        public int IDAreaImpressao { get; set; }
        public int IDProduto { get; set; }
        public ProdutoInformation Produto { get; set; }
        public List<ImpressaoOrdemProducao> Modificacoes { get; set; }
        public decimal Quantidade { get; set; }
        public string Notas { get; set; }

        public int Nivel;

        public bool Viagem;

        public ImpressaoOrdemProducao(AreaImpressaoInformation areaImpressao, PedidoProdutoInformation pedidoProduto, int nivelItem = 0)
        {
            IDAreaImpressao = areaImpressao?.IDAreaImpressao ?? 0;
            IDProduto = pedidoProduto.Produto.IDProduto.Value;
            Produto = pedidoProduto.Produto;
            Quantidade = pedidoProduto.Quantidade ?? 1;
            Notas = pedidoProduto.Notas;
            Nivel = nivelItem;
            Viagem = pedidoProduto.Viagem == true;

            if (pedidoProduto.ListaModificacao != null && pedidoProduto.ListaModificacao.Count() > 0 && Nivel == 0)
            {
                int idPedidoProdutoPai = pedidoProduto.ListaModificacao.Min(m => m.PedidoProdutoPai.IDPedidoProduto.Value);
                Modificacoes = new List<ImpressaoOrdemProducao>();
                FillModificacoesFilha(areaImpressao, idPedidoProdutoPai, pedidoProduto.ListaModificacao, Modificacoes, 1);
            }
        }

        private void FillModificacoesFilha(AreaImpressaoInformation areaImpressao, int pedidoPai, List<PedidoProdutoInformation> listaModificacao, List<ImpressaoOrdemProducao> modificacoes, int nivelAtual)
        {
            foreach (var mod in listaModificacao.Where(m => m.PedidoProdutoPai.IDPedidoProduto == pedidoPai))
            {
                mod.Produto = BLL.Produto.Carregar(mod.Produto.IDProduto.Value);
                // Modificação pai
                Modificacoes.Add(new ImpressaoOrdemProducao(areaImpressao, mod, nivelAtual));

                // Todas modificações filhas
                FillModificacoesFilha(areaImpressao, mod.IDPedidoProduto.Value, listaModificacao, Modificacoes, nivelAtual + 1);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as ImpressaoOrdemProducao;

            if ((object)other == null)
                return false;

            var modIguais = false;

            if (Modificacoes == null && other.Modificacoes == null)
                modIguais = true;
            else if ((Modificacoes == null && other.Modificacoes != null) || (Modificacoes != null && other.Modificacoes == null))
                modIguais = false;
            else
            {
                modIguais = Modificacoes.All(other.Modificacoes.Contains) && Modificacoes.Count == other.Modificacoes.Count;
            }

            return IDAreaImpressao == other.IDAreaImpressao
                & IDProduto == other.IDProduto
                & Notas == other.Notas
                & modIguais;
        }

        public static bool operator ==(ImpressaoOrdemProducao a, ImpressaoOrdemProducao b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if ((a is null) || (b is null))
                return false;

            if (a.IDAreaImpressao != b.IDAreaImpressao
             || a.IDProduto != b.IDProduto
             || a.Notas != b.Notas
             || a.Viagem != b.Viagem)
                return false;

            if (a.Modificacoes == null && b.Modificacoes == null)
                return  true;
            else if ((a.Modificacoes == null && b.Modificacoes != null) || (a.Modificacoes != null && b.Modificacoes == null))
                return false;
            else
                return a.Modificacoes.All(b.Modificacoes.Contains) && a.Modificacoes.Count == b.Modificacoes.Count;
        }

        public static bool operator !=(ImpressaoOrdemProducao a, ImpressaoOrdemProducao b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            unchecked
            {
                hash = (hash * 7) + IDAreaImpressao.GetHashCode();
                hash = (hash * 7) + IDProduto.GetHashCode();
                hash = (hash * 7) + Quantidade.GetHashCode();
                if (Modificacoes != null)
                {
                    foreach (var mod in Modificacoes)
                        hash = (hash * 7) + mod.GetHashCode();
                }
            }
            return hash;
        }

        public override string ToString()
        {
            return $"IDAreaImpressao: {IDAreaImpressao} IDProduto: {IDProduto}, Quantidade: {Quantidade.ToString("#,##0.00")} Mods: {Modificacoes?.Count() ?? 0} {(Viagem ? "(Viagem)" : "")}";
        }
    }
}
