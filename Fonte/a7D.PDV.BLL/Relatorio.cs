using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public static class Relatorio
    {

        public enum TipoTotalizador
        {
            s,
            c,
            m
        }

        private static readonly ISet<Type> _tiposDisponiveis = new HashSet<Type>
        {
            typeof(decimal),
            typeof(double),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
        };

        public static DataTable QuantidadePedidosPorDia()
            => RelatorioDAL.QuantidadePedidosPorDia();
        public static DataTable QuantidadePedidosPorSexo()
            => RelatorioDAL.QuantidadePedidosPorSexo();
        public static DataTable QuantidadePedidosPorTipoEntrada()
            => RelatorioDAL.QuantidadePedidosPorTipoEntrada();
        public static DataTable ValorPorTipoPagamento()
            => RelatorioDAL.ValorPorTipoPagamento();
        //private static DataTable CarregarRelatorio(String querySQL, Dictionary<Object, Object> parametros) 
        //    => RelatorioDAL.CarregarRelatorio(querySQL, parametros);
        public static DataTable ResumoFechamento(Int32 idFechamento)
            => RelatorioDAL.ResumoFechamento(idFechamento);
        public static DataTable CarregarRelatorio(int idRelatorio)
            => CarregarRelatorio(idRelatorio, null);

        public static DataTable Totalizar(this DataTable dt, string totalizador)
        {
            if (dt == null)
                return null;

            if (string.IsNullOrWhiteSpace(totalizador))
                return dt;

            var result = dt.Copy();
            try
            {
                if (!totalizador.Contains(';'))
                    return result;
                if (result.Rows.Count < 1)
                    return result;
                var linha = result.NewRow();

                var colunas = new Dictionary<int, TipoTotalizador>();

                var totalizadores = totalizador.Split(';');

                for (int i = 0; i < totalizadores.Length; i++)
                {
                    if (i < result.Columns.Count)
                    {
                        var strTot = totalizadores[i].ToLowerInvariant();
                        if (_tiposDisponiveis.Contains(result.Columns[i].DataType))
                        {
                            var tipo = default(TipoTotalizador);
                            if (Enum.TryParse(strTot, out tipo))
                            {
                                colunas.Add(i, tipo);
                            }
                        }
                    }
                }

                foreach (DataRow row in result.Rows)
                {
                    foreach (var coluna in colunas)
                    {
                        var valor = default(decimal);
                        if (linha[coluna.Key] == DBNull.Value)
                            valor = 0m;
                        else
                            valor = Convert.ToDecimal(linha[coluna.Key]);

                        switch (coluna.Value)
                        {
                            case TipoTotalizador.m:
                            case TipoTotalizador.s:
                                valor += Convert.ToDecimal(row[coluna.Key]);
                                break;
                            case TipoTotalizador.c:
                                valor++;
                                break;
                            default:
                                break;
                        }
                        linha[coluna.Key] = valor;
                    }
                }

                foreach (var coluna in colunas)
                {
                    if (coluna.Value == TipoTotalizador.m)
                    {
                        var valor = default(decimal);
                        if (linha[coluna.Key] == DBNull.Value)
                            valor = 0m;
                        else
                            valor = Convert.ToDecimal(linha[coluna.Key]);
                        valor = valor / result.Rows.Count;
                        linha[coluna.Key] = valor;
                    }
                }
                result.Rows.Add(linha);
            }
            catch (Exception ex)
            {
                BLL.Logs.ErroBox(CodigoErro.E010, ex);
                //Debug.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            return result;
        }

        public static DataTable CarregarRelatorio(Int32 idRelatorio, Dictionary<Object, Object> parametros)
        {
            RelatorioInformation relatorio = null;
            try
            {
                relatorio = Relatorio.Carregar(idRelatorio);
                return RelatorioDAL.CarregarRelatorio(relatorio.QuerySQL, parametros);
            }
            catch (SqlException ex)
            {
                int i = ex.Message.IndexOf(".");
                var exPDV = new ExceptionPDV(CodigoErro.E120, i > 0 ? new Exception(ex.Message.Substring(0, i), ex) : ex);
                exPDV.Data.Add("idRelatorio", idRelatorio);
                exPDV.Data.Add("relatorio.QuerySQL", relatorio?.QuerySQL);
                throw exPDV;
            }
            catch (Exception ex)
            {
                var exPDV = new ExceptionPDV(CodigoErro.E120, ex);
                exPDV.Data.Add("idRelatorio", idRelatorio);
                exPDV.Data.Add("relatorio.QuerySQL", relatorio?.QuerySQL);
                throw exPDV;
            }
        }

        public static DataTable CarregarRelatorioFechamento(Int32 idRelatorio, Int32 idFechamento)
        {
            Dictionary<Object, Object> parametros = new Dictionary<object, object>();

            parametros.Add("@idFechamento", idFechamento);

            return CarregarRelatorio(idRelatorio, parametros);
        }
        public static DataTable CarregarRelatorioPorData(Int32 idRelatorio, DateTime dtInicio, DateTime dtFim)
        {
            Dictionary<Object, Object> parametros = new Dictionary<object, object>();

            parametros.Add("@dtInicio", dtInicio);
            parametros.Add("@dtFim", dtFim);

            return CarregarRelatorio(idRelatorio, parametros);
        }

        public static List<RelatorioInformation> Listar()
        {
            RelatorioInformation objFiltro = new RelatorioInformation();

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<RelatorioInformation> lista = listaObj.ConvertAll(new Converter<Object, RelatorioInformation>(RelatorioInformation.ConverterObjeto));

            return lista;
        }
        public static RelatorioInformation Carregar(Int32 idRelatorio)
        {
            RelatorioInformation obj = new RelatorioInformation { IDRelatorio = idRelatorio };
            obj = (RelatorioInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void ExportarParaTxt(DataTable dataTable, String arquivo)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Join(";", dataTable.Columns.Cast<DataColumn>().Select(arg => arg.ColumnName)));
            foreach (DataRow dataRow in dataTable.Rows)
                stringBuilder.AppendLine(string.Join(";", dataRow.ItemArray.Select(arg => arg.ToString())));

            File.WriteAllText(arquivo, stringBuilder.ToString(), Encoding.GetEncoding("ISO-8859-1"));
        }

        #region Relatorios CAIXA
        public static List<String> ConferenciaConta(Int32 idPedido)
        {
            PedidoInformation pedido = Pedido.CarregarCompleto(idPedido);

            List<String> relatorio = new List<String>();
            String linha = "";

            relatorio.Add("Pedido: " + pedido.IDPedido.Value.ToString("00000") + "\n\n");

            linha = "ITEM".PadRight(18, ' ');
            linha += "QTD".PadLeft(9, ' ');
            linha += "VL UNIT".PadLeft(9, ' ');
            linha += "VL ITEM".PadLeft(9, ' ');

            relatorio.Add(linha + "\n");

            foreach (var item in pedido.ListaProduto.Where(l => l.Cancelado == false).ToList())
            {
                linha = item.Produto.Nome.PadRight(18, ' ');
                linha += item.Quantidade.Value.ToString("0.00").PadLeft(9, ' ');
                linha += "$" + item.ValorUnitarioString.PadLeft(9, ' ');
                linha += "$" + item.ValorTotalString.PadLeft(9, ' ');

                relatorio.Add(linha + "\n");
            }

            relatorio.Add("\n");
            relatorio.Add("Valor total: ".PadRight(35, ' ') + ("R$ " + pedido.ValorTotalProdutos.ToString("#,##0.00")).PadLeft(10, ' ') + "\n");
            //relatorio.Add("Taxa serviço: ".PadRight(35, ' ') + ("R$ " + pedido.ValorServico.Value.ToString("#,##0.00")).PadLeft(10, ' ') + "\n");
            //relatorio.Add("Total: ".PadRight(35, ' ') + ("R$ " + pedido.ValorProdutoTemp.ToString("#,##0.00")).PadLeft(10, ' ') + "\n");

            relatorio.Add("\n\n");
            relatorio.Add("CPF/CNPJ: ________________________________");
            relatorio.Add("\n\n");

            return relatorio;
        }
        public static List<String> CaixaResumido(DateTime dtMovimento)
        {
            List<String> relatorio = new List<String>();
            String linha;
            Decimal movimento;
            Decimal total1; Decimal total2;

            List<CaixaInformation> listaCaixa = Caixa.ListarCompleto().Where(l => l.DtAbertura.Value.Date == dtMovimento.Date).ToList();
            List<CaixaValorRegistroInformation> listaValorRegistro;
            List<CaixaAjusteInformation> listaAjuste;

            relatorio.Add("Movimento do dia " + dtMovimento.ToShortDateString() + "\n\n");

            foreach (var item in listaCaixa)
            {
                total1 = 0;
                total2 = 0;

                relatorio.Add("---------------------------------------------------------------------------------------\n\n");
                relatorio.Add("PDV: " + item.PDV.IDPDV.Value.ToString("00") + "\n");
                relatorio.Add("Caixa: " + item.Usuario.Nome + "\n");
                relatorio.Add("Abertura: " + item.DtAbertura.Value.ToString("HH:mm") + "\n");
                if (item.DtFechamento != null)
                    relatorio.Add("Fechamento: " + item.DtFechamento.Value.ToString("HH:mm") + "\n");

                relatorio.Add("\nMOVIMENTO\n\n");

                linha = "TipoPagamento".PadRight(18, ' ');
                linha += "Abert.".PadLeft(10, ' ');
                linha += "Mov.".PadLeft(10, ' ');
                linha += "Fech.".PadLeft(10, ' ');
                relatorio.Add(linha + "\n");

                listaValorRegistro = CaixaValorRegistro.ListarPorCaixa(item.IDCaixa.Value);
                foreach (var item2 in listaValorRegistro)
                {
                    movimento = Caixa.TotalMovimentacao(item.IDCaixa.Value, item2.TipoPagamento.IDTipoPagamento.Value);

                    linha = item2.TipoPagamento.Nome.PadRight(16, ' ');
                    linha += item2.ValorAbertura.Value.ToString("#,##0.00").PadLeft(10, ' ');
                    total1 += item2.ValorAbertura.Value + movimento;

                    linha += movimento.ToString("#,##0.00").PadLeft(10, ' ');
                    if (item2.ValorFechamento != null)
                    {
                        linha += item2.ValorFechamento.Value.ToString("#,##0.00").PadLeft(10, ' ');
                        total2 += item2.ValorFechamento.Value;

                        if (item2.ValorAbertura + movimento != item2.ValorFechamento)
                            linha += "\n ** DIFERENÇA: " + (item2.ValorFechamento.Value - (item2.ValorAbertura.Value + movimento)).ToString("R$ #,##0.00");
                    }

                    relatorio.Add(linha + "\n");
                }

                relatorio.Add("\n\n");
                relatorio.Add("Total sistema (Abertura+Movimento): R$ " + total1.ToString("#,##0.00") + "\n");
                relatorio.Add("Total fechamento: R$ " + total2.ToString("#,##0.00") + "\n");
                if (total1 != total2)
                    relatorio.Add(" ** DIFERENÇA: " + (total2 - total1).ToString("R$ #,##0.00"));

                listaAjuste = CaixaAjuste.ListarPorCaixa(item.IDCaixa.Value);
                if (listaAjuste.Count > 0)
                {
                    relatorio.Add("\nSANGRIA E SUPRIMENTO\n");

                    foreach (var item3 in listaAjuste)
                    {
                        linha = item3.CaixaTipoAjuste.Nome + ": " + item3.Valor.Value.ToString("#,##0.00") + " (" + item3.Descricao + ")";

                        relatorio.Add(linha + "\n");
                    }
                }

                relatorio.Add("\n\n");
            }

            return relatorio;
        }
        public static List<String> FechamentoCaixa(Int32 idCaixa, bool autoclose)
        {
            var relatorio = new List<String>();
            String linha;
            Decimal movimento;
            Decimal total1 = 0; Decimal total2 = 0;

            CaixaInformation caixa = Caixa.Carregar(idCaixa);
            caixa.PDV = PDV.Carregar(caixa.PDV.IDPDV.Value);

            if (autoclose)
                relatorio.Add("\nFechamento de Caixa Automático\n\n");
            else
                relatorio.Add("Fechamento de Caixa\n\n");

            relatorio.Add($"PDV: {caixa.PDV.IDPDV} - {caixa.PDV?.Nome}");
            relatorio.Add("Usuário caixa: " + caixa.Usuario.Nome);
            relatorio.Add("Abertura: " + caixa.DtAbertura.Value.ToString("dd/MM/yy HH:mm"));
            relatorio.Add("Fechamento: " + caixa.DtFechamento.Value.ToString("dd/MM/yy HH:mm"));

            relatorio.Add("\n");

            linha = " TipoPagamento".PadRight(15, ' ');
            linha += "Abert.".PadLeft(10, ' ');
            linha += "Mov.".PadLeft(10, ' ');
            linha += "Fech.".PadLeft(10, ' ');
            relatorio.Add(linha);

            var listaValorRegistro = CaixaValorRegistro.ListarPorCaixa(caixa.IDCaixa.Value);
            foreach (var item2 in listaValorRegistro)
            {
                movimento = Caixa.TotalMovimentacao(caixa.IDCaixa.Value, item2.TipoPagamento.IDTipoPagamento.Value);

                linha = item2.TipoPagamento.Nome.PadRight(15, ' ').Substring(0, 15);
                linha += item2.ValorAbertura.Value.ToString("#,##0.00").PadLeft(10, ' ');
                linha += movimento.ToString("#,##0.00").PadLeft(10, ' ');
                linha += item2.ValorFechamento.Value.ToString("#,##0.00").PadLeft(10, ' ');

                total1 += item2.ValorAbertura.Value + movimento;
                total2 += item2.ValorFechamento.Value;

                if (item2.ValorAbertura + movimento != item2.ValorFechamento)
                    linha += "\n ** DIFERENÇA: " + (item2.ValorFechamento.Value - (item2.ValorAbertura.Value + movimento)).ToString("R$ #,##0.00");

                relatorio.Add(linha);
            }

            relatorio.Add("");
            relatorio.Add("Total sistema (Abertura+Movimento): R$ " + total1.ToString("#,##0.00"));
            relatorio.Add("Total fechamento: R$ " + total2.ToString("#,##0.00"));
            if (total1 != total2)
                relatorio.Add(" ** DIFERENÇA: " + (total2 - total1).ToString("R$ #,##0.00"));

            var listaAjuste = CaixaAjuste.ListarPorCaixa(caixa.IDCaixa.Value);
            if (listaAjuste.Count > 0)
            {
                relatorio.Add("SANGRIA E SUPRIMENTO\n");

                foreach (var item3 in listaAjuste)
                {
                    linha = item3.CaixaTipoAjuste.Nome + ": " + item3.Valor.Value.ToString("#,##0.00") + " (" + item3.Descricao + ")";
                    relatorio.Add(linha);
                }
            }

            relatorio.Add("\n\n");

            relatorio.Add("Caixa: ___________________________\n\n");
            relatorio.Add("Gerente: ___________________________\n\n");

            return relatorio;
        }

        public static List<String> Fechamento(Int32 idFechamento, bool reimpressao)
        {
            //TODO: Listar itens Cancelados
            //TODO: Listar contas abertas
            //TODO: Listar mesas abertas

            List<String> relatorio = new List<String>();
            String linha;
            Decimal movimento;
            Decimal total1; Decimal total2;

            FechamentoInformation fechamento = BLL.Fechamento.CarregarCompleto(idFechamento);
            List<CaixaInformation> listaCaixa = Caixa.ListarCompleto().Where(l => l.Fechamento != null && l.Fechamento.IDFechamento == idFechamento).ToList();
            List<CaixaValorRegistroInformation> listaValorRegistro;
            List<CaixaAjusteInformation> listaAjuste;

            relatorio.Add("---------------------------------------------------------------------------------------");
            relatorio.Add("---------------------------------------------------------------------------------------");

            relatorio.Add("             FECHAMENTO DO DIA\n\n");
            relatorio.Add("Emitido em " + DateTime.Now.ToShortDateString() + " por " + fechamento.Usuario.Nome);

            #region Resumo por caixa
            if (RelatorioFechamento.ResumoCaixaAtivado)
            {
                foreach (var item in listaCaixa)
                {
                    listaValorRegistro = CaixaValorRegistro.ListarPorCaixa(item.IDCaixa.Value);
                    if (listaValorRegistro.Count == 0) // Para os fechamento automatico
                        continue;

                    total1 = 0;
                    total2 = 0;

                    relatorio.Add("---------------------------------------------------------------------------------------\n");
                    relatorio.Add("PDV: " + item.PDV.Nome);
                    relatorio.Add("Usuário: " + item.Usuario.Nome);
                    relatorio.Add("Abertura: " + item.DtAbertura.Value.ToString("dd/MM HH:mm"));
                    relatorio.Add("Fechamento: " + item.DtFechamento.Value.ToString("dd/MM HH:mm"));

                    relatorio.Add("");

                    linha = "TipoPagamento".PadRight(18, ' ');
                    linha += "Abert.".PadLeft(10, ' ');
                    linha += "Mov.".PadLeft(10, ' ');
                    linha += "Fech.".PadLeft(10, ' ');
                    relatorio.Add(linha);

                    foreach (var item2 in listaValorRegistro)
                    {
                        movimento = Caixa.TotalMovimentacao(item.IDCaixa.Value, item2.TipoPagamento.IDTipoPagamento.Value);

                        linha = item2.TipoPagamento.Nome.PadRight(16, ' ');
                        linha += item2.ValorAbertura.Value.ToString("#,##0.00").PadLeft(10, ' ');
                        total1 += item2.ValorAbertura.Value + movimento;

                        linha += movimento.ToString("#,##0.00").PadLeft(10, ' ');
                        if (item2.ValorFechamento != null)
                        {
                            linha += item2.ValorFechamento.Value.ToString("#,##0.00").PadLeft(10, ' ');
                            total2 += item2.ValorFechamento.Value;

                            if (item2.ValorAbertura + movimento != item2.ValorFechamento)
                                linha += "\n ** DIFERENÇA: " + (item2.ValorFechamento.Value - (item2.ValorAbertura.Value + movimento)).ToString("R$ #,##0.00");
                        }

                        relatorio.Add(linha);
                    }

                    relatorio.Add("");
                    relatorio.Add("Total (Abertura+Movimento): R$ " + total1.ToString("#,##0.00"));
                    relatorio.Add("Total informado: R$ " + total2.ToString("#,##0.00"));
                    if (total1 != total2)
                        relatorio.Add(" ** DIFERENÇA: " + (total2 - total1).ToString("R$ #,##0.00"));

                    relatorio.Add("");

                    listaAjuste = CaixaAjuste.ListarPorCaixa(item.IDCaixa.Value);
                    if (listaAjuste.Count > 0)
                    {
                        relatorio.Add("SANGRIA E SUPRIMENTO\n");

                        foreach (var item3 in listaAjuste)
                        {
                            linha = item3.CaixaTipoAjuste.Nome + ": " + item3.Valor.Value.ToString("#,##0.00") + " (" + item3.Descricao + ")";
                            relatorio.Add(linha);
                        }
                    }
                }
            }
            #endregion

            #region Tipo de pagamento

            if (RelatorioFechamento.ResumoTipoPagamentoAtivado)
            {
                relatorio.Add("---------------------------------------------------------------------------------------\n");

                relatorio.Add("RESUMO POR TIPO DE PAGAMENTO\n");

                relatorio.AddRange(Relatorio.PorTipoPagamento(idFechamento));
            }

            if (RelatorioFechamento.ResumoCreditoPagamentoAtivado)
            {
                relatorio.Add("---------------------------------------------------------------------------------------\n");

                relatorio.Add("RESUMO CONTA CLIENTE\n");

                relatorio.AddRange(Relatorio.TotalContaCliente(idFechamento));

            }

            #endregion

            relatorio.Add("");

            #region Produtos cancelados
            if (RelatorioFechamento.ProdutosCanceladosAtivado)
            {
                relatorio.Add("---------------------------------------------------------------------------------------\n");

                relatorio.Add("PRODUTOS CANCELADOS\n");
                relatorio.AddRange(Relatorio.ProdutosCancelados(idFechamento));
                relatorio.Add("");
            }

            if (RelatorioFechamento.ProdutosCanceladosAbertoAtivado && !reimpressao)
            {
                relatorio.Add("PRODUTOS CANCELADOS EM PEDIDOS ABERTOS\n");
                relatorio.AddRange(Relatorio.ProdutosCancelados(null));
            }
            #endregion
            #endregion

            #region Produtos vendidos
            if (RelatorioFechamento.ProdutosVendidosAtivado)
            {

                relatorio.Add("---------------------------------------------------------------------------------------\n");

                relatorio.Add("PRODUTOS VENDIDOS\n");
                relatorio.AddRange(BLL.Relatorio.ProdutosVendidos(idFechamento));
            }
            #endregion

            #region Produtos em aberto
            if (RelatorioFechamento.ProdutosAbertosAtivado && !reimpressao)
            {
                relatorio.Add("PRODUTOS EM PEDIDOS ABERTOS\n");
                relatorio.AddRange(BLL.Relatorio.ProdutosVendidos(null));
            }
            #endregion

            #region Pedidos com desconto resumo
            if (RelatorioFechamento.PedidosDescontoResumoAtivado)
            {
                relatorio.Add("---------------------------------------------------------------------------------------\n");
                relatorio.Add("TOTAL DE DESCONTOS");
                relatorio.AddRange(Relatorio.TotalDesconto(idFechamento));
            }
            #endregion

            #region Pedidos com desconto detalhe
            if (RelatorioFechamento.PedidosDescontoDetalheLigado)
            {
                relatorio.Add("---------------------------------------------------------------------------------------\n");
                relatorio.Add("TOTAL DE DESCONTOS (detalhe)");
                relatorio.AddRange(Relatorio.TotalDesconto(idFechamento, resumo: false));
            }
            #endregion

            relatorio.Add("---------------------------------------------------------------------------------------\n\n\n");

            relatorio.Add("_______________________________\n" + fechamento.Usuario.Nome + " (Gerente)\n\n");

            return relatorio;
        }

        public static List<string> PorTipoPagamento(int idFechamento)
        {
            var relatorio = new List<string>();
            var dt = RelatorioDAL.ValorPorTipoPagamento(idFechamento);

            foreach (DataRow tipoPagamento in dt.Rows)
            {
                var linha = tipoPagamento["Tipo pagamento"].ToString().PadRight(25, ' ').Substring(0, 25);
                linha += ("R$ " + (tipoPagamento["Valor total (R$)"] == DBNull.Value ? "0,00" : Convert.ToDecimal(tipoPagamento["Valor total (R$)"]).ToString("#,##0.00")));

                relatorio.Add(linha);
            }

            return relatorio;
        }

        public static List<string> TotalContaCliente(int idFechamento)
        {
            var relatorio = new List<string>();
            var dt = RelatorioDAL.ResumoCreditos(idFechamento);

            foreach (DataRow totais in dt.Rows)
            {
                var linha = totais[0].ToString().PadRight(25, ' ').Substring(0, 25);
                linha += "R$ " + ((decimal)totais[1]).ToString("#,##0.00");

                relatorio.Add(linha);
            }

            return relatorio;
        }

        public static List<String> ProdutosVendidos(Int32? idFechamento)
        {
            List<String> relatorio = new List<String>();
            String linha = "";

            DataTable dtProdutosVendidos = RelatorioDAL.ProdutosVendidos(idFechamento);

            foreach (DataRow produto in dtProdutosVendidos.Rows)
            {
                linha = produto["Produto"].ToString().PadRight(25, ' ').Substring(0, 25);
                linha += Convert.ToDecimal(produto["Quantidade"]).ToString("#,##0.##").PadRight(6, ' ').Substring(0, 6);
                linha += "R$ " + Convert.ToDecimal(produto["ValorTotal"]).ToString("#,##0.00");

                relatorio.Add(linha);
            }

            relatorio.Add("");

            return relatorio;
        }
        public static List<String> ProdutosCancelados(Int32? idFechamento)
        {
            List<String> relatorio = new List<String>();
            String linha = "";

            DataTable dtProdutosCancelados = RelatorioDAL.ProdutosCancelados(idFechamento);

            var cancelamentos = dtProdutosCancelados.AsEnumerable()
               .GroupBy(r1 => new { IDUsuario_Cancelamento = r1["IDUsuario"], UsuarioCancelamento = r1["UsuarioCancelamento"] })
               .Select(r2 => r2.OrderBy(r1 => r1["IDPedido"]));

            foreach (var pedidoProduto in cancelamentos)
            {
                relatorio.Add("Cancelado por " + pedidoProduto.ElementAt(0)["UsuarioCancelamento"] + "\n");

                var dtProdutosCancelados2 = dtProdutosCancelados.Select("IDUsuario=" + pedidoProduto.ElementAt(0)["IDUsuario"]);
                foreach (var produto in dtProdutosCancelados2)
                {
                    linha = Convert.ToDateTime(produto["DtCancelamento"]).ToString("dd/MM hh:mm") + ": ";
                    linha += produto["Produto"].ToString().PadRight(25, ' ').Substring(0, 25);
                    linha += ("R$ " + Convert.ToDecimal(produto["ValorTotal"]).ToString("#,##0.00"));

                    relatorio.Add(linha);
                }

                relatorio.Add("");
            }

            return relatorio;
        }

        public static List<string> TotalDesconto(int idFechamento, bool resumo = true)
        {
            var relatorio = new List<string>();
            var dt = RelatorioDAL.TotalDesconto(idFechamento);

            foreach (DataRow desconto in dt.Rows)
            {
                if (resumo && desconto["Motivo Desconto"].ToString() != "TOTAL")
                    continue;
                var linha = desconto["Motivo Desconto"].ToString().PadRight(25, ' ').Substring(0, 25);
                linha += "R$ " + Convert.ToDecimal(desconto["Valor total (R$)"]).ToString("#,##0.00");

                relatorio.Add(linha);
            }

            return relatorio;
        }
    }
}
