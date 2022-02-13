using a7D.PDV.BLL.Extension;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class OrdemProducaoServices
    {
        public static bool UsarOrdemProducao { get; set; }
        public static bool UsarAreasProducao { get; set; }
        public static string AreasProducaoPadrao { get; set; }
        public static string ImprimirViaExpedicao { get; set; } // NAO, PEDIDO, CONTA
        public static int IDAreaViaExpedicao { get; set; }

        private static void PreparaOrdemProducao(string pdv, string usuario, PedidoInformation pedido, IEnumerable<ImpressaoOrdemProducao> impressoes, int? IDAreaImpressao, bool pedidoAlterado = false, bool pedidoCancelado = false)
        {
            if (impressoes.Any(p => p.Viagem))
            {
                GerarOrdemProducaoFinal(pdv, usuario, pedido, impressoes.Where(p => !p.Viagem), IDAreaImpressao, pedidoAlterado, pedidoCancelado, false);
                GerarOrdemProducaoFinal(pdv, usuario, pedido, impressoes.Where(p => p.Viagem), IDAreaImpressao, pedidoAlterado, pedidoCancelado, true);
            }
            else
                GerarOrdemProducaoFinal(pdv, usuario, pedido, impressoes, IDAreaImpressao, pedidoAlterado, pedidoCancelado, false);
        }

        private static void GerarOrdemProducaoFinal(string pdv, string usuario, PedidoInformation pedido, IEnumerable<ImpressaoOrdemProducao> impressoes, int? IDAreaImpressao, bool pedidoAlterado, bool pedidoCancelado, bool viagem)
        {
            var impressoesAgrupadas = impressoes.GroupBy(i => i.IDAreaImpressao, (key, value) => new { IDAreaImpressao = key, Impressoes = value.ToList() });
            foreach (var agrupada in impressoesAgrupadas)
            {
                var conteudo = new StringBuilder();
                int MaxLine = Constantes.Colunas - 2;

                if (pedidoAlterado)
                    conteudo.Separador("PEDIDO ALTERADO", '*', MaxLine);

                else if (pedidoCancelado)
                    conteudo.Separador("PEDIDO CANCELADO", '*', MaxLine);

                else if (viagem)
                    conteudo
                        .Append("PRODUTOS PARA VIAGEM")
                        .AppendLine();
                else
                {
                    conteudo
                        .Append("ORDEM DE PRODUÇÃO")
                        .AppendLine();
                }

                conteudo.AppendLine();
                switch (pedido.TipoPedido.TipoPedido)
                {
                    case ETipoPedido.Mesa:

                        conteudo
                            .Append("MESA ")
                            .Append(Mesa.CarregarPorGUID(pedido.GUIDIdentificacao).Numero);

                        if (!string.IsNullOrEmpty(pedido.ReferenciaLocalizacao))
                        {
                            conteudo
                                .Append("<Ref: ")
                                .Append(pedido.ReferenciaLocalizacao)
                                .Append(">");
                        }
                        break;

                    case ETipoPedido.Comanda:

                        conteudo
                            .Append("COMANDA ")
                            .Append(Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero);

                        if (!String.IsNullOrEmpty(pedido.ReferenciaLocalizacao))
                        {
                            conteudo
                                .Append("<Ref: ")
                                .Append(pedido.ReferenciaLocalizacao)
                                .Append(">");
                        }

                        if (pedido.Cliente != null)
                        {
                            pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);
                            conteudo
                                .AppendLine()
                                .Append(pedido.Cliente.NomeCompleto);
                        }
                        break;

                    case ETipoPedido.Delivery:

                        // O numero do pedido sai no rodapé
                        string ifood = pedido.PedidoIFood;
                        if (!string.IsNullOrEmpty(ifood))
                            conteudo.AppendLine($"DELIVERY IFOOD {ifood}");
                        else
                            conteudo.AppendLine("DELIVERY");

                        if (!string.IsNullOrEmpty(pedido.ObservacaoCupom))
                            conteudo
                                .AppendLine(pedido.ObservacaoCupom)
                                .AppendLine();

                        if (pedido.Cliente?.IDCliente != null)
                        {
                            pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);
                            int tamanhoLinha = 33;

                            foreach (var linha in pedido.Cliente.NomeCompleto.SplitToLinesArray(tamanhoLinha))
                            {
                                if (!string.IsNullOrWhiteSpace(linha))
                                    conteudo.AppendLine(linha);
                            }

                            if (ConfiguracoesSistema.Valores.DadosClienteCompletoOrdem)
                            {
                                foreach (var linha in (pedido.Cliente.Endereco + ", " + pedido.Cliente.EnderecoNumero).SplitToLinesArray(tamanhoLinha))
                                {
                                    if (!string.IsNullOrWhiteSpace(linha))
                                        conteudo.AppendLine(linha);
                                }

                                if (!string.IsNullOrEmpty(pedido.Cliente.Complemento))
                                {
                                    foreach (var linha in pedido.Cliente.Complemento.SplitToLinesArray(tamanhoLinha))
                                    {
                                        if (!string.IsNullOrWhiteSpace(linha))
                                            conteudo.AppendLine(linha);
                                    }
                                }

                                foreach (var linha in (pedido.Cliente.Bairro + " - " + pedido.Cliente.Cidade).SplitToLinesArray(tamanhoLinha))
                                {
                                    if (!string.IsNullOrWhiteSpace(linha))
                                        conteudo.AppendLine(linha);
                                }

                                if (!string.IsNullOrEmpty(pedido.Cliente.Observacao))
                                {
                                    foreach (var linha in pedido.Cliente.Observacao.SplitToLinesArray(tamanhoLinha))
                                    {
                                        if (!string.IsNullOrWhiteSpace(linha))
                                            conteudo.AppendLine(linha);
                                    }
                                }
                            }

                            conteudo
                                .Append("Tel:")
                                .Append(pedido.Cliente.Telefone1Numero);
                        }
                        break;

                    case ETipoPedido.Balcao:

                        conteudo.Append("BALCÃO");
                        break;
                }

                conteudo
                    .AppendLine()
                    .AppendLine()
                    .Append('-', MaxLine)
                    .AppendLine();

                var larguraDefinida = MaxLine;

                for (int i = 0; i < agrupada.Impressoes.Count; i++)
                {
                    ImpressaoOrdemProducao impressao = agrupada.Impressoes[i];

                    //if (impressao.Viagem)
                    //    conteudo.Append("VIAGEM: ");

                    conteudo.Append(impressao.Quantidade.ToString("#.###"));
                    foreach (var t in impressao.Produto.Nome.SplitToLinesArray(larguraDefinida))
                    {
                        if (!string.IsNullOrWhiteSpace(t))
                        {
                            conteudo
                                .Append(' ')
                                .AppendLine(t);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(impressao.Notas))
                    {
                        var larguraAnotacoes = larguraDefinida - 2;
                        foreach (var t in impressao.Notas.SplitToLinesArray(larguraAnotacoes))
                        {
                            if (!string.IsNullOrWhiteSpace(t))
                            {
                                conteudo
                                    .Append(' ', 8)
                                    .AppendLine(t);
                            }
                        }
                    }

                    var larguraModificacoes = larguraDefinida - 3;
                    if (impressao.Modificacoes?.Count() > 0)
                    {
                        foreach (var modificacao in impressao.Modificacoes)
                        {
                            var nivelqtdmodificacao =
                                (modificacao.Quantidade != 1 ? (modificacao.Quantidade.ToString() + " ") : "") +
                                (modificacao.Nivel < 2 ? ">" : " ") +
                                modificacao.Produto.Nome;

                            foreach (var t in nivelqtdmodificacao.SplitToLinesArray(larguraModificacoes))
                            {
                                if (!string.IsNullOrWhiteSpace(t))
                                {
                                    conteudo
                                        .Append(' ', 2 + 2 * modificacao.Nivel)
                                        .AppendLine(t);
                                }
                            }
                            if (!string.IsNullOrEmpty(modificacao.Notas))
                            {
                                foreach (var t in modificacao.Notas.SplitToLinesArray(larguraModificacoes))
                                {
                                    if (!string.IsNullOrWhiteSpace(t))
                                    {
                                        conteudo
                                            .Append(' ', 2 + 2 * modificacao.Nivel + 4)
                                            .AppendLine(t);
                                    }
                                }
                            }
                        }
                    }
                    conteudo
                        .Append('-', MaxLine)
                        .AppendLine();
                }

                conteudo
                    .AppendLine()
                    .Append("PEDIDO ")
                    .Append(pedido.IDPedido.Value.ToString("#"))
                    .Append(" : ")
                    .Append(DateTime.Now.ToString("dd/MM HH:mm:ss"))
                    .AppendLine()
                    .Append(pdv)
                    .Append(" : ")
                    .Append(usuario)
                    .AppendLine()
                    .AppendLine();

                if (pedidoAlterado)
                    conteudo.Separador("PEDIDO ALTERADO", '*', MaxLine);

                if (pedidoCancelado)
                    conteudo.Separador("PEDIDO CANCELADO", '*', MaxLine);

                Salvar(IDAreaImpressao ?? agrupada.IDAreaImpressao, conteudo.ToString(), ETipoOrdemImpressao.Producao);
            }
        }

        public static void GerarOrdemProducao(Dictionary<int, List<PedidoProdutoInformation>> listaPedidoProduto)
        {
            foreach (var item in listaPedidoProduto)
            {
                var impressoes = new List<ImpressaoOrdemProducao>();
                var areaImpressao = AreaImpressao.Carregar(item.Key);

                foreach (var pp in item.Value)
                {
                    var impressao = new ImpressaoOrdemProducao(areaImpressao, pp);
                    var impressaoExistente = impressoes.FirstOrDefault(i => i == impressao);
                    if (impressaoExistente != null)
                        impressaoExistente.Quantidade += impressao.Quantidade;
                    else
                        impressoes.Add(impressao);
                }

                var pdv = PDV.Carregar(item.Value.First().PDV.IDPDV.Value).Nome;
                var usuario = Usuario.Carregar(item.Value.First().Usuario.IDUsuario.Value).Nome;
                var pedido = Pedido.Carregar(item.Value.First().Pedido.IDPedido.Value);
                PreparaOrdemProducao(pdv, usuario, pedido, impressoes, item.Key, false, false);
            }
        }

        public static void GerarOrdemProducaoEscolhida(List<PedidoProdutoInformation> produtos)
        {
            if (!UsarOrdemProducao || produtos.Count == 0)
                return;
            else if (UsarAreasProducao)
                GerarOrdemProducaoPorAreaSelecionada(produtos);
            else
                GerarOrdemProducao(produtos);
        }

        public static void GerarOrdemProducao(List<PedidoProdutoInformation> produtos, bool pedidoAlterado = false, bool pedidoCancelado = false)
        {
            if (produtos.Count == 0)
                return;

            var areasProducao = AreaImpressao.Listar();
            var listaProdutoImpressao = new Dictionary<int, List<PedidoProdutoInformation>>();
            var impressoes = new List<ImpressaoOrdemProducao>();
            foreach (var areaImpressao in areasProducao)
            {
                var listaProdutosArea = new List<PedidoProdutoInformation>();
                foreach (var item in produtos)
                {
                    if (MapAreaImpressaoProduto.ProdutoMapeado(areaImpressao.IDAreaImpressao.Value, item.Produto.IDProduto.Value))
                        listaProdutosArea.Add(item);
                }

                foreach (var l in listaProdutosArea)
                {
                    var impressao = new ImpressaoOrdemProducao(areaImpressao, l);
                    var impressaoExistente = impressoes.FirstOrDefault(i => i == impressao);
                    if (impressaoExistente != null)
                        impressaoExistente.Quantidade += impressao.Quantidade;
                    else
                        impressoes.Add(impressao);
                }

                if (listaProdutosArea.Count > 0)
                {
                    listaProdutoImpressao.Add(areaImpressao.IDAreaImpressao.Value, listaProdutosArea);
                }
            }

            var produto1 = produtos.First(); // (p => p.PedidoProdutoPai == null);
            var pdv = PDV.Carregar(produto1.PDV.IDPDV.Value).Nome;
            var usuario = Usuario.Carregar(produto1.Usuario.IDUsuario.Value).Nome;
            var pedido = Pedido.Carregar(produto1.Pedido.IDPedido.Value);
            PreparaOrdemProducao(pdv, usuario, pedido, impressoes, IDAreaImpressao: null, pedidoAlterado: pedidoAlterado, pedidoCancelado: pedidoCancelado);
        }

        public static bool SelecionarOrdemProducao(List<PedidoProdutoInformation> itens)
        {
            if (!UsarOrdemProducao || !UsarAreasProducao || itens.Count == 0)
                return true;

            itens.ForEach(i => i.idAreaProducao = 0);

            var areaspadrao = AreasProducaoPadrao
                .Split(',')
                .Where(a => Int32.TryParse(a.Trim(), out int aa))
                .Select(a => Int32.Parse(a.Trim()))
                .ToArray();

            int nAreaSelecionada = 0;
            var listaProdutoImpressao = new Dictionary<int, List<PedidoProdutoInformation>>();

            while (itens.Any(p => p.idAreaProducao == 0))
            {
                listaProdutoImpressao.Clear();

                string cProdutos = "";
                var opcoes = new List<tbAreaImpressao>();

                foreach (var item in itens)
                {
                    if (item.idAreaProducao != 0)
                        continue;

                    var areasProduto = MapAreaImpressaoProduto.AreasProduto(item.Produto.IDProduto.Value);
                    if (areasProduto.Length > 1)
                    {
                        foreach (var a in areasProduto)
                        {
                            if (areaspadrao.Contains(a.IDAreaImpressao))
                            {
                                item.idAreaProducao = areasProduto[0].IDAreaImpressao;
                                break;
                            }
                            else if (a.IDAreaImpressao == nAreaSelecionada)
                            {
                                item.idAreaProducao = nAreaSelecionada;
                                break;
                            }
                            var outro = itens.FirstOrDefault(o => o.IDPedidoProduto != o.IDPedidoProduto && o.Produto.IDProduto == item.Produto.IDProduto);
                            if (outro != null && outro.idAreaProducao != 0)
                            {
                                item.idAreaProducao = outro.idAreaProducao;
                                break;
                            }
                        }

                        if (item.idAreaProducao == 0)
                        {
                            if (!cProdutos.Contains(item.Produto.Nome + ","))
                                cProdutos += item.Produto.Nome + ", ";

                            foreach (var a in areasProduto)
                                if (!opcoes.Contains(a))
                                    opcoes.Add(a);
                        }
                    }
                    else if (areasProduto.Length == 1)
                    {
                        item.idAreaProducao = areasProduto[0].IDAreaImpressao;
                    }
                    else
                    {
                        item.idAreaProducao = -1; // Sem área de produção!!!
                    }
                }

                if (opcoes.Count > 0)
                {
                    // monta menu quando necessário
                    cProdutos = "Área de produção para: " + cProdutos.Substring(0, cProdutos.Length - 2);
                    nAreaSelecionada = SelectIDValor.Select(cProdutos, opcoes.Select(a => new SelectIDValor(a.IDAreaImpressao, a.Nome)).ToArray());
                    if (nAreaSelecionada == -1)
                        return false;
                }
            }
            return true;
        }

        private static void GerarOrdemProducaoPorAreaSelecionada(List<PedidoProdutoInformation> itens)
        {
            var areaItens = itens
                .Where(i => i.idAreaProducao > 0)
                .GroupBy(
                    a => a.idAreaProducao, // chave
                    (k, g) => new // metodo de agrupamento
                    {
                        id = k,
                        itens = g.ToList()
                    })
                .ToDictionary(ag => ag.id, ag => ag.itens);

            GerarOrdemProducao(areaItens);
        }

        private static void Salvar(int idArea, string conteudo, ETipoOrdemImpressao tipo)
        {
            EF.Repositorio.Inserir(new tbOrdemImpressao
            {
                DtOrdem = DateTime.Now,
                IDAreaImpressao = idArea,
                ConteudoImpressao = conteudo,
                TipoOrdemImpressao = tipo
            });
        }

        public static void GerarViaExpedicao(int idPedido, int idAreaProducao)
        {
            try
            {
                if (idAreaProducao < 1)
                    throw new ExceptionPDV(CodigoErro.E801);

                Salvar(idAreaProducao, "EXPEDICAO|" + idPedido, ETipoOrdemImpressao.Expedicao);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E802, ex, "ID " + idAreaProducao);
            }
        }

        public static void GerarConta(int idPedido, int idUsuario, int idPDV, int idAreaImpressao)
        {
            Salvar(idAreaImpressao, $"{idPedido}|{idUsuario}|{idPDV}", ETipoOrdemImpressao.Conta);
        }

        public static void GerarSAT(string arquivoCFeSAT, int idPedido, int idAreaImpressao)
        {
            Salvar(idAreaImpressao, $"{arquivoCFeSAT}|{idPedido}", ETipoOrdemImpressao.SAT);
        }
    }
}