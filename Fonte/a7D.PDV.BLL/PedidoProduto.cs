using System;
using System.Collections.Generic;
using System.Linq;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using System.Text;

namespace a7D.PDV.BLL
{
    public static class PedidoProduto
    {
        public static PedidoProdutoInformation Carregar(Int32 idPedidoProduto)
        {
            PedidoProdutoInformation obj = new PedidoProdutoInformation { IDPedidoProduto = idPedidoProduto };
            obj = (PedidoProdutoInformation)CRUD.Carregar(obj);
            obj.Produto = Produto.Carregar(obj.Produto.IDProduto.Value);

            List<PedidoProdutoInformation> listaGeral = PedidoProdutoDAL.ListarPorPedido(obj.Pedido.IDPedido.Value);
            obj.ListaModificacao = PreencherModificacoesItem(obj, listaGeral);

            if (obj?.IDPedidoProduto > 0)
                obj.Status = StatusModel.Inalterado;

            return obj;
        }

        public static void Salvar(PedidoProdutoInformation obj)
        {
            if (!obj.RetornarAoEstoque.HasValue)
                obj.RetornarAoEstoque = false;

            if (obj.IDPedidoProduto == null)
            {
                if (obj.DtInclusao == null)
                    obj.DtInclusao = DateTime.Now;

                PedidoProduto.Adicionar(obj);
            }
            else
            {
                obj.DtAlteracao = DateTime.Now;
                PedidoProduto.Alterar(obj);
            }
        }
        public static void Adicionar(PedidoProdutoInformation obj)
        {
            CRUD.Adicionar(obj);
        }
        public static void Alterar(PedidoProdutoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idPedidoProduto)
        {
            PedidoProdutoInformation obj = new PedidoProdutoInformation { IDPedidoProduto = idPedidoProduto };
            CRUD.Excluir(obj);
        }

        public static List<PedidoProdutoInformation> ListarPorPedido(Int32 idPedido)
        {
            List<PedidoProdutoInformation> listaGeral = PedidoProdutoDAL.ListarPorPedido(idPedido);
            List<PedidoProdutoInformation> lista = listaGeral.Where(l => l.PedidoProdutoPai == null).ToList();

            foreach (PedidoProdutoInformation item in lista)
                item.ListaModificacao = PreencherModificacoesItem(item, listaGeral);

            lista.ForEach(l => l.Status = StatusModel.Inalterado);

            return lista;
        }

        private static List<PedidoProdutoInformation> PreencherModificacoesItem(PedidoProdutoInformation pedidoProduto, List<PedidoProdutoInformation> listaGeral)
        {
            var lista = new List<PedidoProdutoInformation>();
            foreach (var item in listaGeral)
            {
                if (item.PedidoProdutoPai?.IDPedidoProduto == pedidoProduto.IDPedidoProduto)
                {
                    // Adiciona o item filho que será o pai do proximo nivel
                    item.PedidoProdutoPai = pedidoProduto;
                    item.Status = StatusModel.Inalterado;
                    lista.Add(item);
                    // Lineariza paineis de modificações hierarquicos
                    lista.AddRange(PreencherModificacoesItem(item, listaGeral));
                }
            }
            return lista;
        }

        public static List<PedidoProdutoInformation> FiltrarSemCancelados(List<PedidoProdutoInformation> lista)
        {
            return lista.Where(p => p.Cancelado != true).ToList();
        }

        public static List<PedidoProdutoInformation> FiltrarApenasCancelados(List<PedidoProdutoInformation> lista)
        {
            return lista.Where(p => p.Cancelado == true).ToList();
        }

        public static string TransferirProduto(Int32 idPedidoProduto, decimal quantidade, PedidoInformation pedidoDestino)
        {
            var pedidoProduto = Carregar(idPedidoProduto);

            var log = new StringBuilder();
            log.AppendLine($"Transferindo IDPedido: {pedidoProduto.Pedido.IDPedido} IDPedidoProduto: {idPedidoProduto} QTD: {quantidade} para {pedidoDestino.TipoPedido.TipoPedido.ToString()} IDPedido: {pedidoDestino.IDPedido}");
            log.AppendLine();

            // Cria novo item principal
            var pedidoProdutoDestino = new PedidoProdutoInformation
            {
                Pedido = pedidoDestino,
                Produto = pedidoProduto.Produto,
                CodigoAliquota = pedidoProduto.CodigoAliquota,
                ValorUnitario = pedidoProduto.ValorUnitario,
                DtInclusao = pedidoProduto.DtInclusao,
                Quantidade = quantidade,
                Notas = pedidoProduto.Notas,
                IDTipoEntrada = pedidoProduto.IDTipoEntrada,
                Viagem = pedidoProduto.Viagem,
                Cancelado = pedidoProduto.Cancelado,
                PDV = pedidoProduto.PDV,
                Usuario = pedidoProduto.Usuario,
                ValorDesconto = pedidoProduto.ValorDesconto,
                UsuarioDesconto = pedidoProduto.UsuarioDesconto,
                TipoDesconto = pedidoProduto.TipoDesconto
            };

            PedidoProduto.Salvar(pedidoProdutoDestino);
            log.AppendLine($"PAI IDPedidoProduto: {pedidoProduto.IDPedidoProduto} => {pedidoProdutoDestino.IDPedidoProduto} - {pedidoProdutoDestino.Produto.Nome}");

            // É rendundante, mas necessário para a auto referencia a seguir que será usada
            pedidoProdutoDestino.IDPedidoProduto_Original = pedidoProdutoDestino.IDPedidoProduto;

            //// Define o novo Pai de todos
            //var paiRaiz = pedidoProduto.ListaModificacao.Where(p => p.PedidoProdutoPai != null && p.PedidoProdutoPai.IDPedidoProduto.Value == pedidoProduto.IDPedidoProduto.Value);
            //foreach (var pai in paiRaiz)
            //{
            //    log.AppendLine($"Modificações Pai: {pai.IDPedidoProduto} => {pedidoProdutoDestino.IDPedidoProduto}");
            //    pai.PedidoProdutoPai = pedidoProdutoDestino;
            //}

            // Cria as modificações para cada pai (prevendo que a lista está em ordem de IDs)
            foreach (var modificacao in pedidoProduto.ListaModificacao)
            {
                var pedidoModificacaoDestino = new PedidoProdutoInformation
                {
                    Pedido = pedidoDestino,
                    Produto = modificacao.Produto,
                    CodigoAliquota = modificacao.CodigoAliquota,
                    ValorUnitario = modificacao.ValorUnitario,
                    Quantidade = modificacao.Quantidade,
                    Cancelado = modificacao.Cancelado,
                    PDV = modificacao.PDV,
                    Usuario = modificacao.Usuario
                };

                // Referencia do novo pai já criado: Acha o pai da modificação atual
                var pai = pedidoProduto.ListaModificacao.FirstOrDefault(p => p.IDPedidoProduto.Value == modificacao.PedidoProdutoPai.IDPedidoProduto.Value) ?? pedidoProdutoDestino;
                var novoPai = new PedidoProdutoInformation()
                {
                    IDPedidoProduto = pai.IDPedidoProduto_Original,
                    Produto = pai.Produto,
                    Pedido = pedidoDestino,
                };

                pedidoModificacaoDestino.PedidoProdutoPai = novoPai;

                PedidoProduto.Salvar(pedidoModificacaoDestino);
                log.AppendLine($"MODIFICAÇÃO IDPedidoProduto: {modificacao.IDPedidoProduto} => {pedidoModificacaoDestino.IDPedidoProduto} PAI: {novoPai.IDPedidoProduto} - {pedidoModificacaoDestino.Produto.Nome}");

                modificacao.IDPedidoProduto_Original = pedidoModificacaoDestino.IDPedidoProduto; // Atualiza na lista 
            }

            log.AppendLine();

            // Remove origem
            if (pedidoProduto.Quantidade <= quantidade)
            {
                var lista = pedidoProduto.ListaModificacao.OrderByDescending(p => p.IDPedidoProduto);
                foreach (var modificacao in lista)
                {
                    log.AppendLine($"EXCLUINDO MODIFICACAO IDPedidoProduto: {modificacao.IDPedidoProduto} - {modificacao.Produto.Nome}");
                    Excluir(modificacao.IDPedidoProduto.Value);
                }

                log.AppendLine($"EXCLUINDO PAI IDPedidoProduto: {pedidoProduto.IDPedidoProduto} - {pedidoProduto.Produto.Nome}");
                Excluir(idPedidoProduto);
            }
            else
            {
                log.AppendLine("REDUZINDO QUANTIDADE: " + quantidade);
                pedidoProduto.Quantidade -= quantidade;
                Salvar(pedidoProduto);
            }

            return log.ToString();

        }

        public static List<PedidoProdutoInformation> ListarPorPedidoAgrupado(Int32 idPedido)
        {
            var lista = PedidoProdutoDAL.ListarPorPedidoAgrupado(idPedido);

            lista.ForEach(l => l.Status = StatusModel.Inalterado);

            return lista;
        }

        public static List<PedidoProdutoInformation> ListarPorDataPedido(DateTime dt)
        {
            return PedidoProdutoDAL.ListarPorDataPedido(dt);
        }

        public static bool PedidoDuplicado(String guidControleDuplicidade)
        {
            return PedidoProdutoDAL.PedidoDuplicado(guidControleDuplicidade);
        }

        public static ModificacoesInvalidas ValidarModificacoes(List<PedidoProdutoInformation> ListaPedidoProduto)
        {
            var produtosInvalidos = new List<ModificacaoProduto>();

            foreach (var obj in ListaPedidoProduto)
            {
                obj.Produto = Produto.CarregarCompleto(obj.Produto.IDProduto.Value);
                if (obj.Produto.AssistenteModificacoes == true)
                    continue;

                var modificacoesSelecionadas = obj.ListaModificacao == null ? new List<int>() : obj.ListaModificacao.Select(m => m.Produto.IDProduto.Value).ToList();
                var modificacoes = obj.Produto.ListaPainelModificacao.GroupBy(ppm => ppm.PainelModificacao.IDPainelModificacao.Value);
                foreach (var painel in modificacoes)
                {
                    var painelModificacao = PainelModificacao.CarregarCompleto(painel.Key);
                    var num = modificacoesSelecionadas.Where(ms => painelModificacao.ListaProduto.Select(p => p.Produto.IDProduto.Value).Any(p => p == ms)).Count();

                    var respeitaMin = painelModificacao.Minimo == null || num >= painelModificacao.Minimo;
                    var respeitaMax = painelModificacao.Maximo == null || num <= painelModificacao.Maximo;

                    if (!respeitaMin || !respeitaMax)
                        produtosInvalidos.Add(new ModificacaoProduto { Min = painelModificacao.Minimo, Max = painelModificacao.Maximo, PainelModificacao = painelModificacao, Produto = obj.Produto, QtdSelecionada = num });
                }
            }

            return new ModificacoesInvalidas { Valido = produtosInvalidos.Count == 0, Produtos = produtosInvalidos };
        }

        public static string TextoModificacoesInvalidas(ModificacoesInvalidas modificacaoesInvalidas)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Verifique a quantidade de modificações selecionadas dos itens abaixo:").AppendLine();

            foreach (var m in modificacaoesInvalidas.Produtos)
            {
                sb.AppendLine(m.Produto.Nome);
                sb.Append(string.Format("\t{0}: selecionado {1} (", m.PainelModificacao.Nome, m.QtdSelecionada));

                if (m.Min.HasValue)
                    sb.Append("min ").Append(m.Min.Value.ToString()).Append(" / ");

                if (m.Max.HasValue)
                    sb.Append("max ").Append(m.Max.Value.ToString());

                sb.AppendLine(")").AppendLine();
            }

            return sb.ToString();

        }

        public class ModificacoesInvalidas
        {
            public bool Valido { get; set; }
            public List<ModificacaoProduto> Produtos { get; set; }
        }

        public class ModificacaoProduto
        {
            public int? Min { get; set; }
            public int? Max { get; set; }
            public int QtdSelecionada { get; set; }
            public ProdutoInformation Produto { get; set; }
            public PainelModificacaoInformation PainelModificacao { get; set; }
        }

        public static void CancelarPorPedido(Int32 idPedido, int idUsuario)
        {
            using (var pdv = new pdv7Context())
            {
                var produtospedido = pdv.tbPedidoProdutos.Where(p => p.IDPedido == idPedido);
                foreach (var pp in produtospedido)
                {
                    pp.Cancelado = true;
                    pp.IDUsuario_cancelamento = idUsuario;
                    pp.DtCancelamento = DateTime.Now;
                }
                pdv.SaveChanges();
            }
        }
    }
}
