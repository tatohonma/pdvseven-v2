using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace a7D.PDV.BLL
{
    public static class EntradaSaida
    {

        #region CRUD
        public static List<EntradaSaidaInformation> Listar()
        {
            List<EntradaSaidaInformation> list = CRUD.Listar(new EntradaSaidaInformation()).Cast<EntradaSaidaInformation>().ToList();
            return list;
        }

        public static List<EntradaSaidaInformation> ListarValidas(DateTime? dataDe = null, DateTime? dataAte = null, bool exibirProdutosInativos = false)
        {
            return EntradaSaidaDAL.ListarValidas(dataDe, dataAte, exibirProdutosInativos);
        }

        public static EntradaSaidaInformation Carregar(int idEntradaSaida)
        {
            EntradaSaidaInformation obj = new EntradaSaidaInformation { IDEntradaSaida = idEntradaSaida };
            return (EntradaSaidaInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(EntradaSaidaInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(EntradaSaidaInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static bool Movimentar(MovimentacaoInformation movimentacao)
        {
            if (movimentacao == null)
                throw new ExceptionPDV(CodigoErro.E602);
            if (movimentacao.IDMovimentacao.HasValue == false)
                throw new ExceptionPDV(CodigoErro.E603);
            if (movimentacao.Processado == true)
                throw new ExceptionPDV(CodigoErro.A604);
            if (movimentacao.TipoMovimentacao == null || movimentacao.TipoMovimentacao.IDTipoMovimentacao.HasValue == false)
                throw new ExceptionPDV(CodigoErro.E605);

            if (movimentacao.MovimentacaoProdutos == null || movimentacao.MovimentacaoProdutos.Count < 1)
                return false;

            var movimentacoes = new List<MovimentacaoProduto>();

            foreach (var mp in movimentacao.MovimentacaoProdutos)
            {
                movimentacoes.Add(new MovimentacaoProduto
                {
                    Entrada = movimentacao.TipoMovimentacao.Tipo == "+",
                    Data = movimentacao.DataMovimentacao.Value,
                    Produto = mp.Produto,
                    Quantidade = mp.QuantidadeRelativa.Value,
                    Unidade = mp.Unidade
                });
            }

            Movimentar(movimentacao.GUID, movimentacoes);
            return true;
        }

        #endregion

        #region Relatorios

        public static DataTable EstoqueMovimentacoes(DateTime dataDe, DateTime? dataAte = null, bool exibirProdutosInativos = false)
        {
            try
            {
                var valorAnterior = new Dictionary<int, decimal>();
                if (dataAte.HasValue == false)
                    dataAte = DateTime.Now.AddDays(1).Date;
                else
                    dataAte = dataAte.Value.AddDays(1).Date;

                var dtResultado = new DataTable();

                using (var atual = EstoqueInventario(dataDe, exibirProdutosInativos: exibirProdutosInativos))
                {
                    var movimentacoes = ListarValidas(dataDe, dataAte, exibirProdutosInativos);

                    var datas = movimentacoes
                        .GroupBy(
                            m => new { Data = m.Data.Value.Date, Produto = m.Produto },
                            m => new { Data = m.Data.Value, Quantidade = m.Quantidade },
                            (key, value) => new { DataProduto = key, DataQuantidade = value }
                        )
                        .OrderBy(i => i.DataProduto.Data);

                    dtResultado.Columns.Add("IDProduto", typeof(int));
                    dtResultado.Columns.Add("Produto", typeof(string));
                    dtResultado.Columns.Add("Unidade", typeof(string));
                    dtResultado.Columns.Add("Quantidade Inicial", typeof(decimal));

                    //var listaDatas = datas.Select(d => d.DataProduto.Data).Distinct().OrderBy(d => d);
                    var setDatas = new HashSet<DateTime>();

                    for (var d = dataDe; d < dataAte; d = d.AddDays(1))
                        setDatas.Add(d);

                    //setDatas.UnionWith(Inventario.ListarProcessados().Where(i => i.Data.Value > dataDe && i.Data.Value <= dataAte.Value).Select(i => i.Data.Value.Date));

                    foreach (var item in setDatas.OrderBy(i => i))
                        dtResultado.Columns.Add(item.ToString("dd/MM/yyyy"), typeof(string));

                    dtResultado.Columns.Add("Saldo", typeof(string));
                    DataRow newRow = null;

                    foreach (DataRow row in atual.Rows)
                    {
                        newRow = dtResultado.NewRow();

                        var idProduto = row.Field<int>("IDProduto");
                        var idUnidade = row.Field<int>("IDUnidade");
                        newRow.SetField("IDProduto", idProduto);
                        newRow.SetField("Produto", row.Field<string>("Nome"));
                        newRow.SetField("Unidade", Unidade.CarregarPorProduto(idProduto).Nome);
                        var quantidade = ConversaoUnidade.Converter(row.Field<decimal>("Quantidade"), idUnidade, idProduto: idProduto, exata: true);
                        newRow.SetField("Quantidade Inicial", quantidade.ToString("+#,##0.00;-#,##0.00"));

                        dtResultado.Rows.Add(newRow);
                        valorAnterior.Add(idProduto, quantidade);
                    }

                    foreach (var data in setDatas.OrderBy(d => d))
                    {

                        HashSet<int> produtos = new HashSet<int>();
                        var inventario = InventarioProdutos.UltimoInventarioProcessado(data);

                        if (inventario?.Count == 0)
                            inventario = null;

                        if (inventario != null)
                            produtos.UnionWith(inventario.Select(i => i.Produto.IDProduto.Value));

                        produtos.UnionWith(datas.Where(d => d.DataProduto.Data == data).Select(d => d.DataProduto.Produto.IDProduto.Value));

                        foreach (var idProduto in produtos)
                        {
                            DataRow row = dtResultado.AsEnumerable().FirstOrDefault(dr => dr.Field<int>("IDProduto") == idProduto);
                            if (row == null)
                            {
                                row = dtResultado.NewRow();

                                row.SetField("IDProduto", idProduto);
                                row.SetField("Quantidade Inicial", 0m);
                                row.SetField("Produto", Produto.Carregar(idProduto).Nome);
                                row.SetField("Unidade", Unidade.CarregarPorProduto(idProduto).Nome);

                                dtResultado.Rows.Add(row);
                            }
                            decimal quantidade = 0m;
                            IEnumerable<InventarioProdutosInformation> linhasInventario = null;
                            if (inventario != null)
                                linhasInventario = inventario.Where(i => i.Produto.IDProduto == idProduto);

                            var res = datas.Where(d => d.DataProduto.Data == data && d.DataProduto.Produto.IDProduto == idProduto).FirstOrDefault();
                            var quantidadeMov = 0m;
                            if (res != null)
                            {
                                if (linhasInventario == null || linhasInventario.Count() == 0)
                                    quantidadeMov = res.DataQuantidade.Sum(a => a.Quantidade.HasValue ? a.Quantidade.Value : 0m);
                                else
                                    quantidadeMov = res.DataQuantidade.Where(r => r.Data > linhasInventario.First().Inventario.Data && r.Data < (r.Data.Date.AddDays(1)).Date).Sum(a => a.Quantidade.HasValue ? a.Quantidade.Value : 0m);

                            }
                            var quantidadeInv = 0m;
                            if (linhasInventario != null && linhasInventario.Count() > 0)
                            {
                                var valAnterior = 0m;
                                if (valorAnterior.ContainsKey(idProduto))
                                    valAnterior = valorAnterior[idProduto] * -1;

                                foreach (var linhaInventario in linhasInventario)
                                    quantidadeInv += ConversaoUnidade.Converter(linhaInventario.Quantidade.Value, linhaInventario.Unidade, linhaInventario.Produto.Unidade, true);

                                quantidadeInv += valAnterior;
                            }

                            quantidade = quantidadeInv + quantidadeMov;

                            if (valorAnterior.ContainsKey(idProduto))
                                valorAnterior[idProduto] = valorAnterior[idProduto] + quantidade;
                            else
                                valorAnterior.Add(idProduto, quantidade);

                            row[data.ToString("dd/MM/yyyy")] = quantidade.ToString("+#,##0.00;-#,##0.00");
                        }
                    }

                    foreach (DataRow dr in dtResultado.Rows)
                    {
                        var idProduto = dr.Field<int>("IDProduto");

                        var saldo = 0m;

                        if (valorAnterior.ContainsKey(idProduto))
                            saldo = valorAnterior[idProduto];

                        dr["Saldo"] = saldo.ToString("#,##0.00");

                        foreach (var data in setDatas.OrderBy(d => d))
                        {
                            if (dr[data.ToString("dd/MM/yyyy")] == DBNull.Value)
                                dr[data.ToString("dd/MM/yyyy")] = 0.00m;
                        }
                    }
                }

                return dtResultado;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E60A, ex);
            }
        }

        //public static decimal EstoqueAtual(ProdutoInformation produto)
        //{
        //    return EstoqueAtual(produto.IDProduto.Value);
        //}

        public static decimal EstoqueAtual(int idProduto)
        {
            using (var dt = EstoqueInventario(DateTime.Now, idProduto))
            {
                if (dt != null && dt.Rows.Count == 1)
                {
                    if (dt.Rows[0].Field<int?>("IDProduto") == idProduto)
                    {
                        var quantidade = dt.Rows[0].Field<decimal>("Quantidade");
                        return quantidade;
                    }
                }
            }
            return 0;
        }

        public static DataTable EstoqueInventario(DateTime dataEstoque, int? idProdutoFiltro = null, bool exibirProdutosInativos = false)
        {
            DataTable dtInventario = null;
            var dt = EntradaSaidaDAL.EstoqueAtual(dataEstoque, out dtInventario, idProdutoFiltro, exibirProdutosInativos);
            using (DataTable dtInventarioRef = dtInventario)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var idProduto = row.Field<int>("IDProduto");
                    var quantidade = row.Field<decimal>("Quantidade");
                    if (dtInventarioRef != null)
                    {
                        quantidade += dtInventarioRef.BuscarQuantidadeConvertida(idProduto);
                        row["Quantidade"] = quantidade;
                    }
                }

                if (dtInventarioRef != null)
                {
                    foreach (DataRow row in dtInventarioRef.Rows)
                    {
                        var idProduto = row.Field<int>("IDProduto");
                        var idUnidade = row.Field<int>("IDUnidade");
                        var nome = row.Field<string>("Nome");

                        if (Existe(dt, idProduto) == false)
                        {
                            var newRow = dt.NewRow();
                            var quantidade = row.Field<decimal>("Quantidade");
                            var unidade = Unidade.CarregarPorProduto(idProduto);

                            newRow.SetField("IDProduto", idProduto);
                            newRow.SetField("IDUnidade", idUnidade);
                            newRow.SetField("Nome", nome);
                            newRow.SetField("Unidade", unidade.Nome);
                            newRow.SetField("Simbolo", unidade.Simbolo);
                            newRow.SetField("Quantidade", dtInventarioRef.BuscarQuantidadeConvertida(idProduto));
                            dt.Rows.Add(newRow);
                        }
                    }
                }
            }
            return dt;
        }

        public static DataTable HistoricoPorDia(DateTime data, int idProduto)
        {
            try
            {
                var dtResultado = new DataTable();

                dtResultado.Columns.Add("Data", typeof(DateTime));
                dtResultado.Columns.Add("Tipo", typeof(string));
                dtResultado.Columns.Add("IDProduto", typeof(int));
                dtResultado.Columns.Add("Produto", typeof(string));
                dtResultado.Columns.Add("Detalhes", typeof(string));
                dtResultado.Columns.Add("IDUnidade", typeof(int));
                dtResultado.Columns.Add("Unidade", typeof(string));
                dtResultado.Columns.Add("Simbolo", typeof(string));
                dtResultado.Columns.Add("Quantidade", typeof(decimal));

                using (var ds = EntradaSaidaDAL.Historico(data.Date, idProduto))
                using (var dt = EstoqueInventario(data.Date.AddMilliseconds(-1), idProduto))
                {
                    DataTable dtMovimentacao = ds.Tables[0];
                    DataTable dtInventario = null;
                    if (ds.Tables.Count > 1)
                        dtInventario = ds.Tables[1];
                    var saldoInicial = 0m;

                    //var eiRow = dtResultado.NewRow();
                    //eiRow.SetField("Tipo", "Saldo Inicial");
                    //eiRow.SetField("Data", data.Date);
                    if (dt.Rows.Count > 0)
                        saldoInicial = dt.Rows[0].Field<decimal>("Quantidade");

                    //eiRow.SetField("Quantidade", saldoInicial);

                    //dtResultado.Rows.Add(eiRow);


                    if (dtInventario?.Rows.Count > 0)
                    {
                        var datasInventario = dtInventario.AsEnumerable().GroupBy(dr => dr.Field<DateTime>("Data"));
                        var dataFinal = data.Date.AddDays(-1);
                        var quantidade = saldoInicial;
                        foreach (var dataInventario in datasInventario)
                        {
                            foreach (DataRow row in dtMovimentacao.AsEnumerable().Where(dr => dr.Field<DateTime>("Data") <= dataInventario.Key))
                            {
                                var newmRow = dtResultado.NewRow();
                                newmRow.CopiarCampos<DateTime>(row, "Data");
                                newmRow.CopiarCampos<string>(row, "Produto", "Unidade", "Tipo", "Simbolo", "Detalhes");
                                newmRow.CopiarCampos<int>(row, "IDProduto", "IDUnidade");
                                newmRow.CopiarCampos<decimal>(row, "Quantidade");
                                quantidade += newmRow.Field<decimal>("Quantidade");

                                dtResultado.Rows.Add(newmRow);
                            }

                            var quantidadeInventario = 0m;
                            foreach (var dr in dataInventario)
                                quantidadeInventario += ConversaoUnidade.Converter(dr.Field<decimal>("Quantidade"), dr.Field<int>("IDUnidade"), idProduto: dr.Field<int>("IDProduto"), exata: true);

                            var newRow = dtResultado.NewRow();
                            var refRow = dataInventario.First();

                            newRow.SetField("Data", dataInventario.Key);
                            newRow.CopiarCampos<string>(refRow, "Tipo", "Produto", "Detalhes");
                            newRow.CopiarCampos<int>(refRow, "IDProduto");

                            var unidade = Unidade.CarregarPorProduto(refRow.Field<int>("IDProduto"));
                            newRow.SetField("IDUnidade", unidade.IDUnidade);
                            newRow.SetField("Simbolo", unidade.Simbolo);
                            newRow.SetField("Unidade", unidade.Nome);
                            quantidade = (quantidade * -1) + quantidadeInventario;
                            newRow.SetField("Quantidade", quantidade);

                            dtResultado.Rows.Add(newRow);

                            dataFinal = dataInventario.Key;
                        }

                        foreach (var dr in dtMovimentacao.AsEnumerable().Where(dr => dr.Field<DateTime>("Data") > dataFinal))
                        {
                            var newRow = dtResultado.NewRow();
                            newRow.CopiarCampos<DateTime>(dr, "Data");
                            newRow.CopiarCampos<string>(dr, "Tipo", "Produto", "Simbolo", "Unidade", "Detalhes");
                            newRow.CopiarCampos<int>(dr, "IDProduto", "IDUnidade");
                            newRow.CopiarCampos<decimal>(dr, "Quantidade");

                            dtResultado.Rows.Add(newRow);
                        }

                    }
                    else
                    {
                        foreach (var dr in dtMovimentacao.AsEnumerable())
                        {
                            var newRow = dtResultado.NewRow();

                            newRow.CopiarCampos<DateTime>(dr, "Data");
                            newRow.CopiarCampos<string>(dr, "Tipo", "Produto", "Simbolo", "Unidade");
                            newRow.CopiarCampos<int>(dr, "IDProduto", "IDUnidade");
                            newRow.CopiarCampos<decimal>(dr, "Quantidade");

                            dtResultado.Rows.Add(newRow);
                        }

                    }
                }
                return dtResultado;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E60A, ex);
            }
        }

        public static DataTable RelatorioInventario(int idInventario)
        {
            var inventario = Inventario.Carregar(idInventario);

            if (inventario == null || inventario.IDInventario.HasValue == false)
                throw new ExceptionPDV(CodigoErro.E606, idInventario.ToString());
            if (inventario.Excluido == true)
                throw new ExceptionPDV(CodigoErro.E607, idInventario.ToString());
            if (inventario.Processado == false)
                throw new ExceptionPDV(CodigoErro.E608, idInventario.ToString());

            var iAnterior = Inventario.ListarProcessados().Where(i => i.Data < inventario.Data).OrderByDescending(i => i.Data).FirstOrDefault();

            var inventarioAnterior = new List<InventarioProdutosInformation>();

            if (iAnterior != null)
                inventarioAnterior = iAnterior.InventarioProdutos;

            var dataInicio = iAnterior != null ? iAnterior.Data.Value : SqlDateTime.MinValue.Value;

            var dtResultado = EntradaSaidaDAL.RelatorioInventario(idInventario, dataInicio);

            using (var dtEstoqueInicial = EstoqueInventario(dataInicio))
            {
                for (int i = dtResultado.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = dtResultado.Rows[i];
                    var idProduto = row.Field<int>("IDProduto");
                    row.SetField("QuantidadeAnterior", dtEstoqueInicial.BuscarQuantidade(idProduto));
                    row.SetField("QuantidadeInventarioAtual", inventario.InventarioProdutos.BuscarQuantidadeConvertida(idProduto));

                    var soma = row.Field<decimal>("QuantidadeAnterior")
                        + row.Field<decimal>("QuantidadeMovimentacaoEntrada")
                        + row.Field<decimal>("QuantidadeVenda")
                        + row.Field<decimal>("QuantidadeMovimentacaoSaida")
                        + row.Field<decimal>("QuantidadeSaidaAvulsa")
                        + row.Field<decimal>("QuantidadeInventarioAtual");

                    if (soma == 0m)
                        row.Delete();
                }
            }
            return dtResultado;
        }

        public static IEnumerable<Estoque> EstoqueAtual()
        {
            using (var dt = EstoqueInventario(DateTime.Now))
            {
                foreach (DataRow row in dt.Rows)
                {
                    var idProduto = row.Field<int>("IDProduto");
                    var produto = Produto.Carregar(idProduto);
                    if (produto.Unidade != null && produto.Unidade.IDUnidade.HasValue)
                        produto.Unidade = Unidade.Carregar(produto.Unidade.IDUnidade.Value);

                    var quantidade = row.Field<decimal>("Quantidade");

                    yield return new Estoque
                    {
                        Produto = produto,
                        Unidade = Unidade.Carregar(row.Field<int>("IDUnidade")),
                        Quantidade = quantidade
                    };
                }
            }
        }

        public static DataTable RelatorioValorizacaoEstoque(DateTime dataEstoque, bool exibirProdutosInativos = false)
        {
            var dtResultado = new DataTable("RelatorioValorizacaoEstoque");

            dtResultado.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("IDProduto", typeof(int)),
                new DataColumn("Produto", typeof(string)),
                new DataColumn("IDUnidade", typeof(int)),
                new DataColumn("Unidade", typeof(string)),
                new DataColumn("Quantidade", typeof(string)),
                new DataColumn("ValorUnitario", typeof(string)),
                new DataColumn("ValorEstimado", typeof(string))
            });

            using (var dtAtual = EstoqueInventario(dataEstoque, exibirProdutosInativos: exibirProdutosInativos))
            {
                var precoProdutos = Produto.Listar(new ProdutoInformation() { Excluido = false })
                    .Where(p => dtAtual.AsEnumerable().Any(dr => dr.Field<int>("IDProduto") == p.IDProduto))
                    .Select(p => new { p.IDProduto, p.ValorUnitario });

                foreach (var dr in dtAtual.AsEnumerable())
                {
                    var idProduto = dr.Field<int>("IDProduto");
                    var quantidade = dr.Field<decimal>("Quantidade");


                    var newRow = dtResultado.NewRow();
                    newRow.CopiarCampos<string>(dr, "Unidade");
                    newRow.SetField("Produto", dr.Field<string>("Nome"));
                    newRow.CopiarCampos<int>(dr, "IDProduto", "IDUnidade");
                    newRow.SetField("Quantidade", quantidade.ToString("#,##0.00"));
                    var precoProduto = precoProdutos.FirstOrDefault(pp => pp.IDProduto == idProduto);
                    var valorUnitario = 0m;

                    if (precoProduto != null)
                        valorUnitario = precoProduto.ValorUnitario.Value;

                    newRow.SetField("ValorUnitario", valorUnitario.ToString("R$ #,##0.00"));
                    newRow.SetField("ValorEstimado", (quantidade * valorUnitario).ToString("R$ #,##0.00"));

                    dtResultado.Rows.Add(newRow);
                }

            }

            return dtResultado;
        }

        #endregion

        #region Movimentacao

        public static bool Movimentar(PedidoInformation pedido, bool entrada = false)
        {
            try
            {
                if (PDV.PossuiEstoque() == false)
                    return false;

                if (pedido == null || pedido.IDPedido.HasValue == false)
                    return false;

                if (pedido.ListaProduto == null || pedido.ListaProduto.Count == 0)
                    return false;

                if (string.IsNullOrWhiteSpace(pedido.GUIDMovimentacao))
                    throw new ExceptionPDV(CodigoErro.E601);

                return Movimentar(pedido.GUIDMovimentacao, pedido.ListaProduto, entrada, pedido.DtPedidoFechamento);
            }
            catch (Exception ex)
            {
                Logs.Erro(CodigoErro.E600, ex);
                return false;
            }
        }

        public static bool Movimentar(string GUIDIdentificacao, List<PedidoProdutoInformation> pedidoProdutos, bool entrada = false, DateTime? data = null)
        {
            for (int i = 0; i < pedidoProdutos.Count(); i++)
            {
                var idPedidoProduto = pedidoProdutos[i].IDPedidoProduto;
                if (idPedidoProduto.HasValue && idPedidoProduto > 0)
                    pedidoProdutos[i] = PedidoProduto.Carregar(pedidoProdutos[i].IDPedidoProduto.Value);
            }

            var lista = pedidoProdutos.Where(p => (p.Cancelado == false || (p.Cancelado == true && p.RetornarAoEstoque == false)) || (p.IDPedidoProduto.HasValue == false || p.IDPedidoProduto.Value <= 0)).ToList();
            var movimentacoes = new List<MovimentacaoProduto>();

            // TODO: Testar modificações multi niveis
            foreach (var item in lista)
            {
                movimentacoes.AddRange(GerarMovimentacoes(item.Produto, item.Quantidade.Value, entrada, data));
                if (item.ListaModificacao?.Count > 0)
                {
                    foreach (var modificacao in item.ListaModificacao.Where(m => m.Cancelado == false || (m.Cancelado == true && m.RetornarAoEstoque == false)))
                    {
                        modificacao.Produto = Produto.Carregar(modificacao.Produto.IDProduto.Value);
                        movimentacoes.AddRange(GerarMovimentacoes(modificacao.Produto, modificacao.Quantidade.Value, entrada, data));
                    }
                }
            }

            if (movimentacoes.Count > 0)
            {
                Movimentar(GUIDIdentificacao, movimentacoes);
                return true;
            }
            else
                return false;
        }

        #endregion

        #region Métodos Privados

        private static void Movimentar(string GUID_Origem, IEnumerable<MovimentacaoProduto> movimentacoes)
        {
            var sb = new StringBuilder();
            try
            {
                using (var scope = new TransactionScope())
                {
                    foreach (var m in movimentacoes)
                    {
                        sb.Append($"{m.Produto} => {m.Quantidade} {m.Unidade}");
                        decimal quantidade = ConversaoUnidade.Converter(m.Quantidade, m.Unidade, m.Produto.Unidade, true);
                        sb.Append(" =>" + quantidade);
                        var es = new EntradaSaidaInformation
                        {
                            Data = m.Data,
                            Entrada = m.Entrada,
                            GUID_Origem = GUID_Origem,
                            Quantidade = m.Entrada ? quantidade : quantidade * -1,
                            Produto = m.Produto,
                        };
                        Salvar(es);
                        sb.AppendLine(" OK " + es.IDEntradaSaida);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Movimentar Log", sb.ToString());
                throw new ExceptionPDV(CodigoErro.E60E, ex);
            }
        }

        private static List<MovimentacaoProduto> GerarMovimentacoes(ProdutoInformation produto, decimal quantidade, bool entrada = false, DateTime? data = null)
        {
            var sb = new StringBuilder("GerarMovimentacoes");
            sb.AppendLine();
            try
            {
                var result = new List<MovimentacaoProduto>();
                produto.ListaProdutoReceita = ProdutoReceita.ListarPorProduto(produto.IDProduto.Value);
                if (produto.ListaProdutoReceita == null || produto.ListaProdutoReceita.Count < 1)
                {
                    if (produto.ControlarEstoque == false)
                        return result;

                    result.Add(new MovimentacaoProduto
                    {
                        Produto = produto,
                        Data = data ?? DateTime.Now,
                        Entrada = entrada,
                        Quantidade = quantidade,
                        Unidade = produto.Unidade
                    });
                }
                else
                {
                    foreach (var item in produto.ListaProdutoReceita)
                    {
                        sb.Append($"{item.Produto} => {item.Quantidade} {item.Unidade}");
                        var novaQuantidade = (ConversaoUnidade.Converter(item.Quantidade.Value, item.Unidade, item.ProdutoIngrediente.Unidade, true) * quantidade);
                        sb.AppendLine(" => " + novaQuantidade);
                        result.AddRange(GerarMovimentacoes(item.ProdutoIngrediente, novaQuantidade, entrada));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ex.Data.Add("GerarMovimentacoes LOG", sb.ToString());
                throw new ExceptionPDV(CodigoErro.E60A, ex);
            }
        }

        #endregion

        #region Extension Methods

        private static decimal BuscarQuantidadeConvertida(this DataTable fonte, int idProduto)
        {
            try
            {
                var quantidadeFinal = 0m;
                foreach (DataRow row in fonte.Rows)
                {
                    if (row.Field<int>("IDProduto") == idProduto)
                    {
                        var idUnidade = row.Field<int>("IDUnidade");
                        var quantidade = row.Field<decimal>("Quantidade");
                        quantidade = ConversaoUnidade.Converter(quantidade, idUnidade, idProduto: idProduto, exata: true);
                        quantidadeFinal += quantidade;
                    }
                }

                return quantidadeFinal;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E60A, ex);
            }
        }

        private static decimal BuscarQuantidadeConvertida(this List<InventarioProdutosInformation> inventarioProdutos, int idProduto)
        {
            try
            {
                var quantidadeFinal = 0m;

                foreach (var ip in inventarioProdutos.Where(ip => ip.Produto.IDProduto == idProduto))
                {
                    quantidadeFinal += ConversaoUnidade.Converter(ip.Quantidade.Value, ip.Unidade, ip.Produto, true);
                }

                return quantidadeFinal;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E60A, ex);
            }
        }

        private static decimal BuscarQuantidade(this DataTable fonte, int idProduto)
        {
            var quantidadeFinal = 0m;
            foreach (DataRow row in fonte.Rows)
            {
                if (row.Field<int>("IDProduto") == idProduto)
                    quantidadeFinal += row.Field<decimal>("Quantidade");
            }

            return quantidadeFinal;
        }

        private static bool Existe(this DataTable fonte, int idProduto)
        {
            foreach (DataRow row in fonte.Rows)
            {
                if (row.Field<int>("IDProduto") == idProduto)
                    return true;
            }
            return false;
        }

        #endregion

        private class MovimentacaoProduto
        {
            internal ProdutoInformation Produto { get; set; }
            internal decimal Quantidade { get; set; }
            internal UnidadeInformation Unidade { get; set; }
            internal DateTime Data { get; set; }
            internal bool Entrada { get; set; }
        }

        public class Estoque
        {
            public ProdutoInformation Produto { get; set; }
            public UnidadeInformation Unidade { get; set; }
            public decimal Quantidade { get; set; }
        }
    }
}
