using a7D.PDV.Model.Dashboard;
using a7D.PDV.Model.Dashboard.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class Dashboard
    {
        private static readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");
        private static readonly string _formato = "MMyyyy";

        private static readonly List<string> _coresBorda = new List<string>
        {
            "rgba(255,99,132,1)",
            "rgba(54, 162, 235, 1)",
            "rgba(255, 206, 86, 1)",
            "rgba(75, 192, 192, 1)",
            "rgba(153, 102, 255, 1)",
            "rgba(255, 159, 64, 1)",
            //google colors
            "rgba(51, 102, 204, 1)",
            "rgba(220, 57, 18, 1)",
            "rgba(255, 153, 0, 1)",
            "rgba(16, 150, 24, 1)",
            "rgba(153, 0, 153, 1)",
            "rgba(59, 62, 172, 1)",
            "rgba(0, 153, 198, 1)",
            "rgba(221, 68, 119, 1)",
            "rgba(102, 170, 0, 1)",
            "rgba(184, 46, 46, 1)",
            "rgba(49, 99, 149, 1)",
            "rgba(153, 68, 153, 1)",
            "rgba(34, 170, 153, 1)",
            "rgba(170, 170, 17, 1)",
            "rgba(102, 51, 204, 1)",
            "rgba(255, 159, 64, 1)",
            "rgba(230, 115, 0, 1)",
            "rgba(139, 7, 7, 1)",
            "rgba(50, 146, 98, 1)",
            "rgba(85, 116, 166, 1)",
            "rgba(59, 62, 172, 1)",
        };

        private static readonly List<string> _coresFundo = new List<string>
        {
            "rgba(255, 99, 132, 0.2)",
            "rgba(54, 162, 235, 0.2)",
            "rgba(255, 206, 86, 0.2)",
            "rgba(75, 192, 192, 0.2)",
            "rgba(153, 102, 255, 0.2)",
            "rgba(255, 159, 64, 0.2)",
            //google colors
            "rgba(51, 102, 204, 0.2)",
            "rgba(220, 57, 18, 0.2)",
            "rgba(255, 153, 0, 0.2)",
            "rgba(16, 150, 24, 0.2)",
            "rgba(153, 0, 153, 0.2)",
            "rgba(59, 62, 172, 0.2)",
            "rgba(0, 153, 198, 0.2)",
            "rgba(221, 68, 119, 0.2)",
            "rgba(102, 170, 0, 0.2)",
            "rgba(184, 46, 46, 0.2)",
            "rgba(49, 99, 149, 0.2)",
            "rgba(153, 68, 153, 0.2)",
            "rgba(34, 170, 153, 0.2)",
            "rgba(170, 170, 17, 0.2)",
            "rgba(102, 51, 204, 0.2)",
            "rgba(255, 159, 64, 0.2)",
            "rgba(230, 115, 0, 0.2)",
            "rgba(139, 7, 7, 0.2)",
            "rgba(50, 146, 98, 0.2)",
            "rgba(85, 116, 166, 0.2)",
            "rgba(59, 62, 172, 0.2)",
        };

        private static Chart FaturamentoPorDia()
        {
            var chart = ChartPadrao(_Type.Line);
            chart.Options.Scales = new _Scales
            {
                YAxes = new List<_Axis>
                {
                    new _Axis
                    {
                        Ticks = new _Ticks
                        {
                            BeginAtZero = true,
                            Callback = new JRaw("function(value, index, values) { return numeral(value).format('$0,0.00'); }")
                        }
                    }
                },
                XAxes = new List<_Axis>
                {
                    new _Axis
                    {
                        GridLines = new _GridLines
                        {
                            Display = false
                        }
                    }
                }
            };

            chart = chart
                .ComLegenda()
                .ComToolTip();

            chart.Data.Labels = Enumerable.Range(1, 31).Select(r => r.ToString()).ToList();

            chart.Options.Legend.Position = _Position.Bottom;

            chart.Options.ToolTips.Mode = _InteractionMode.X;
            chart.Options.ToolTips.Position = "nearest";

            chart.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                beforeTitle = new JRaw("function (tooltipItems, data) { return 'Dia'; }"),
                Label = new JRaw("function(tooltipItem, data) { return numeral(tooltipItem.yLabel).format('$0,0.00'); }")
            };

            var datasets = new List<_DataSet>();
            datasets.Add(new _LineDataSet
            {
                Label = DateTime.Now.AddMonths(-1).ToString("MMMM", _cultureInfo),
                Data = new List<decimal>(),
                BackgroundColor = _coresFundo[0],
                BorderColor = _coresBorda[0],
                SpanGaps = true,
                PointRadius = 3,
                PointHitRadius = 4
            });

            datasets.Add(new _LineDataSet
            {
                Label = DateTime.Now.ToString("MMMM", _cultureInfo),
                Data = new List<decimal>(),
                BackgroundColor = _coresFundo[1],
                BorderColor = _coresBorda[1],
                SpanGaps = true,
                PointRadius = 3,
                PointHitRadius = 4
            });

            var faturamentos = DAL.DashboardDAL.FaturamentoPorDia2Meses().GroupBy(f => f.Data.ToString(_formato, _cultureInfo), (key, value) => new { MesAno = key, Faturamentos = value.OrderBy(f => f.Data).ToList() });
            var anoAtual = DateTime.Now.Year;

            foreach (var agrupamentoMes in faturamentos)
            {
                var dataMes = DateTime.ParseExact($"{agrupamentoMes.MesAno}01", $"{_formato}dd", _cultureInfo);
                var mes = dataMes.Month;
                while (dataMes.Month == mes && dataMes.Date <= DateTime.Now.Date)
                {
                    _DataSet dataset;
                    if (mes == DateTime.Now.Month)
                        dataset = datasets[1];
                    else
                        dataset = datasets[0];

                    var valor = agrupamentoMes.Faturamentos.FirstOrDefault(f => f.Data == dataMes);
                    var valorAnterior = 0m;

                    if (dataset.Data.Count > 0)
                        valorAnterior = dataset.Data[dataset.Data.Count - 1];

                    dataset.Data.Add(valorAnterior + (valor?.Valor ?? 0));

                    dataMes = dataMes.AddDays(1);
                }
            }

            chart.Data.Datasets = datasets;

            return chart;
        }

        private static Chart FaturamentoMensal()
        {
            var chart = ChartPadrao(_Type.Bar);

            chart = chart
                .ComToolTip()
                .ComLegenda();

            chart.Options.Scales = new _Scales
            {
                YAxes = new List<_Axis>
                {
                    new _Axis
                    {
                        Ticks = new _Ticks
                        {
                            BeginAtZero = true,
                            Callback = new JRaw("function(value, index, values) { return numeral(value).format('$0,0.00'); }")
                        }
                    }
                },
                XAxes = new List<_Axis>
                {
                    new _Axis
                    {
                        GridLines = new _GridLines
                        {
                            Display = false
                        }
                    }
                }
            };

            chart.Options.Legend.Position = _Position.Bottom;

            chart.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                Label = new JRaw("function(tooltipItem, data) { return numeral(tooltipItem.yLabel).format('$0,0.00'); }")
            };

            var datasets = new List<_DataSet>();
            DateTime ano = DateTime.Now.AddYears(-1);
            datasets.Add(new _DataSet
            {
                Label = ano.ToString("yyyy"),
                Data = new List<decimal>(),
                BackgroundColor = _coresFundo[0],
                BorderColor = _coresBorda[0],
                HoverBackgroundColor = _coresBorda[0],
                BorderWidth = 1
            });
            datasets.Add(new _DataSet
            {
                Label = DateTime.Now.ToString("yyyy"),
                Data = new List<decimal>(),
                BackgroundColor = _coresFundo[1],
                BorderColor = _coresBorda[1],
                HoverBackgroundColor = _coresBorda[1],
                BorderWidth = 1
            });

            IEnumerable<int> meses = Enumerable.Range(1, 12);
            chart.Data.Labels = meses.Select(m => new DateTime(1, m, 1).ToString("MMMM", _cultureInfo)).ToList();

            var faturamentosMensais = DAL.DashboardDAL.FaturamentoMensalUltimos2Anos();

            while (ano.Year <= DateTime.Now.Year)
            {
                var dataset = default(_DataSet);
                if (ano.Year == DateTime.Now.Year)
                    dataset = datasets[1];
                else
                    dataset = datasets[0];
                foreach (var m in meses)
                {
                    var dados = faturamentosMensais.FirstOrDefault(fm => fm.Ano == ano.Year && fm.Mes == m);
                    dataset.Data.Add(dados?.Total ?? 0);
                }
                ano = ano.AddYears(1);
            }

            chart.Data.Datasets = datasets;

            return chart;
        }

        private static Chart MotivosCancelamento(DateTime date, DateTime? dateFim)
        {
            var motivosCancelamento = DAL.DashboardDAL.MotivosCancelamento(date, dateFim);
            var chart = ChartPadrao(_Type.Pie);

            chart = chart
                .ComToolTip()
                .ComLegenda();

            chart.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                Label = new JRaw("function(tooltipItem, data) { return data.labels[tooltipItem.index] + ': ' + numeral(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]).format('$0,0.00'); }")
            };

            chart.Options.Legend.Position = _Position.Bottom;
            chart.Options.ToolTips.Position = "nearest";

            chart.Data.Labels = new List<string>();

            var dataset = new _DataSet
            {
                Data = new List<decimal>(),
                BackgroundColor = _coresBorda,
                HoverBackgroundColor = _coresBorda
            };

            foreach (var motivoCancelamento in motivosCancelamento.OrderBy(m => m.Motivo))
            {
                chart.Data.Labels.Add(motivoCancelamento.Motivo);
                dataset.Data.Add(motivoCancelamento.Valor);
            }

            chart.Data.Datasets.Add(dataset);

            return chart;
        }

        public static ResultadoDashboard ObterDashboard(DateTime date, DateTime? dateFim = null, bool somenteDetalhes = false)
        {
            var result = new ResultadoDashboard();
            if (!somenteDetalhes)
            {
                result.FaturamentoMensal = FaturamentoMensal();
                result.FaturamentoPorDia = FaturamentoPorDia();
            };

            ObterFaturamentoTipoESemana(ref result, date, dateFim);
            result.MotivosCancelamento = MotivosCancelamento(date, dateFim);
            result.FaturamentoPorCategoria = FaturamentoPorCategoria(date, dateFim);
            result.FaturamentoPorTipoDePagamento = FaturamentoPorTipoDePagamento(date, dateFim);
            result.VolumePorGarcom = VolumePorGarcom(date, dateFim);

            return result;
        }

        private static Chart VolumePorGarcom(DateTime date, DateTime? dateFim)
        {
            var faturamentoGarcom = DAL.DashboardDAL.ObterFaturamentoPorGarcom(date, dateFim);
            var chart =
                ChartPadrao(_Type.Pie)
                .ComToolTip()
                .ComLegenda();

            chart.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                Label = new JRaw("function(tooltipItem, data) { return data.labels[tooltipItem.index] + ': ' + numeral(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]).format('$0,0.00'); }")
            };

            chart.Options.Legend.Position = _Position.Bottom;
            chart.Options.ToolTips.Position = "nearest";

            chart.Data.Labels = new List<string>();

            var dataset = new _DataSet
            {
                Data = new List<decimal>(),
                BackgroundColor = _coresBorda,
                HoverBackgroundColor = _coresBorda
            };

            foreach (var g in faturamentoGarcom)
            {
                chart.Data.Labels.Add(g.Nome);
                dataset.Data.Add(faturamentoGarcom.Where(fg => fg.Nome == g.Nome).Sum(fg => fg.Valor));
            }

            chart.Data.Datasets.Add(dataset);

            return chart;
        }

        private static Chart FaturamentoPorTipoDePagamento(DateTime date, DateTime? dateFim)
        {
            var faturamentoTipoPagamento = DAL.DashboardDAL.ObterFaturamentoTipoPagamento(date, dateFim);
            var chart =
                ChartPadrao(_Type.Pie)
                .ComToolTip()
                .ComLegenda();

            chart.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                Label = new JRaw("function(tooltipItem, data) { return data.labels[tooltipItem.index] + ': ' + numeral(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]).format('$0,0.00'); }")
            };

            chart.Options.Legend.Position = _Position.Bottom;
            chart.Options.ToolTips.Position = "nearest";

            chart.Data.Labels = new List<string>();

            var dataset = new _DataSet
            {
                Data = new List<decimal>(),
                BackgroundColor = _coresBorda,
                HoverBackgroundColor = _coresBorda
            };

            foreach (var ft in faturamentoTipoPagamento)
            {
                chart.Data.Labels.Add(ft.Tipo);
                dataset.Data.Add(faturamentoTipoPagamento.Where(f => f.Tipo == ft.Tipo).Sum(f => f.Valor));
            }

            chart.Data.Datasets.Add(dataset);

            return chart;
        }

        private static Chart FaturamentoPorCategoria(DateTime date, DateTime? dateFim)
        {
            var faturamentoCategoria = DAL.DashboardDAL.ObterFaturamentoPorCategoria(date, dateFim);
            var chart =
                ChartPadrao(_Type.Pie)
                .ComToolTip()
                .ComLegenda();

            chart.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                Label = new JRaw("function(tooltipItem, data) { return data.labels[tooltipItem.index] + ': ' + numeral(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]).format('$0,0.00'); }")
            };

            chart.Options.Legend.Position = _Position.Bottom;
            chart.Options.ToolTips.Position = "nearest";

            chart.Data.Labels = new List<string>();

            var dataset = new _DataSet
            {
                Data = new List<decimal>(),
                BackgroundColor = _coresBorda,
                HoverBackgroundColor = _coresBorda
            };

            var topCategorias = new List<string>();
            var i = 0;
            var contagem = faturamentoCategoria.Count();
            for (; i < 5 && i < contagem; i++)
            {
                var fc = faturamentoCategoria.ElementAt(i);
                chart.Data.Labels.Add(fc._Categoria);
                topCategorias.Add(fc._Categoria);
                dataset.Data.Add(faturamentoCategoria.Where(f => f._Categoria == fc._Categoria).Sum(f => f.Valor));
            }

            if (i <= contagem)
            {
                chart.Data.Labels.Add("Outras");
                dataset.Data.Add(faturamentoCategoria.Where(f => !topCategorias.Contains(f._Categoria)).Sum(f => f.Valor));
            }

            chart.Data.Datasets.Add(dataset);

            return chart;
        }

        private static void ObterFaturamentoTipoESemana(ref ResultadoDashboard result, DateTime date, DateTime? dateFim)
        {
            var faturamentoDetalhado = DAL.DashboardDAL.FaturamentoDetalhado(date, dateFim);

            #region Faturamento por tipo de pedido
            var faturamentoPorTipoDePedido = ChartPadrao(_Type.Pie);

            faturamentoPorTipoDePedido = faturamentoPorTipoDePedido
                .ComToolTip()
                .ComLegenda();

            faturamentoPorTipoDePedido.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                Label = new JRaw("function(tooltipItem, data) { return data.labels[tooltipItem.index] + ': ' + numeral(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]).format('$0,0.00'); }")
            };

            faturamentoPorTipoDePedido.Options.Legend.Position = _Position.Bottom;
            faturamentoPorTipoDePedido.Options.ToolTips.Position = "nearest";

            faturamentoPorTipoDePedido.Data.Labels = new List<string>();

            var tiposPedido = TipoPedido.Listar();
            var dataset = new _DataSet
            {
                Data = new List<decimal>(),
                BackgroundColor = _coresBorda,
                HoverBackgroundColor = _coresBorda
            };

            foreach (var tipoPedido in tiposPedido.OrderBy(tp => tp.Nome))
            {
                faturamentoPorTipoDePedido.Data.Labels.Add(tipoPedido.Nome);
                dataset.Data.Add(faturamentoDetalhado.Where(fd => fd.Tipo == tipoPedido.Nome).Sum(fd => fd.Valor));
            }

            faturamentoPorTipoDePedido.Data.Datasets.Add(dataset);

            result.FaturamentoPorTipoPedido = faturamentoPorTipoDePedido;
            #endregion

            #region Faturamento por dia da semana

            var faturamentoPorDiaDaSemana = ChartPadrao(_Type.Pie);

            faturamentoPorDiaDaSemana = faturamentoPorDiaDaSemana
                .ComToolTip()
                .ComLegenda();

            faturamentoPorDiaDaSemana.Options.ToolTips.Callbacks = new _ToolTipCallbacks
            {
                Label = new JRaw("function(tooltipItem, data) { return data.labels[tooltipItem.index] + ': ' +  numeral(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]).format('$0,0.00'); }")
            };

            faturamentoPorDiaDaSemana.Options.Legend.Position = _Position.Bottom;
            faturamentoPorDiaDaSemana.Options.ToolTips.Position = "nearest";

            faturamentoPorDiaDaSemana.Data.Labels = new List<string>();
            var datasetSemana = new _DataSet
            {
                Data = new List<decimal>(),
                BackgroundColor = _coresBorda,
                HoverBackgroundColor = _coresBorda
            };

            var data = new DateTime(1, 1, 1);

            while (data.Day <= 7)
            {
                faturamentoPorDiaDaSemana.Data.Labels.Add(data.ToString("dddd", _cultureInfo));
                datasetSemana.Data.Add(faturamentoDetalhado.Where(fd => fd.Data.DayOfWeek == data.DayOfWeek).Sum(fd => fd.Valor));
                data = data.AddDays(1);
            }

            faturamentoPorDiaDaSemana.Data.Datasets.Add(datasetSemana);

            result.FaturamentoPorDiaDaSemana = faturamentoPorDiaDaSemana;
            #endregion
        }

        public static Chart ChartPadrao(_Type tipo)
        {
            return new Chart
            {
                Data = new _Data
                {
                    Datasets = new List<_DataSet>()
                },
                Type = tipo,
                Options = new _Options
                {
                    Responsive = true,
                    MaintainAspectRatio = true
                }
            };
        }

        public static Chart ComLegenda(this Chart chart)
        {
            chart.Options.Legend = new _Legend();
            return chart;
        }

        public static Chart ComToolTip(this Chart chart)
        {
            chart.Options.ToolTips = new _ToolTips();
            chart.Options.Hover = new _Hover();
            return chart;
        }

        public static Chart ComTitulo(this Chart chart, string titulo)
        {
            chart.Options.Title = new _Title
            {
                Display = true,
                Text = titulo
            };
            return chart;
        }

        public static void AplicarCoresPadrao(this Chart chart)
        {
            if (chart?.Data?.Datasets?.Count > 0)
            {
                foreach (var dataset in chart.Data.Datasets)
                {
                    dataset.BackgroundColor = new List<string>
                    {
                        "rgba(255, 99, 132, 0.2)",
                        "rgba(54, 162, 235, 0.2)",
                        "rgba(255, 206, 86, 0.2)",
                        "rgba(75, 192, 192, 0.2)",
                        "rgba(153, 102, 255, 0.2)",
                        "rgba(255, 159, 64, 0.2)"
                    };
                    dataset.BorderColor = new List<string>
                    {
                        "rgba(255,99,132,1)",
                        "rgba(54, 162, 235, 1)",
                        "rgba(255, 206, 86, 1)",
                        "rgba(75, 192, 192, 1)",
                        "rgba(153, 102, 255, 1)",
                        "rgba(255, 159, 64, 1)"
                    };
                }
            }
        }

        public static ProdutosVendidos ObterRankingProdutos(string strTipo, DateTime date, DateTime? dateFim)
        {
            var tipo = (DAL.DashboardDAL.TipoRanking)Enum.Parse(typeof(DAL.DashboardDAL.TipoRanking), strTipo);

            var top = DAL.DashboardDAL.ObterRankingProdutos(tipo, DAL.DashboardDAL.DirecaoRanking.Mais, date, dateFim);
            var bot = DAL.DashboardDAL.ObterRankingProdutos(tipo, DAL.DashboardDAL.DirecaoRanking.Menos, date, dateFim);
            var valorProdutosVendidos = DAL.DashboardDAL.ValorProdutosVendidos();

            var percentual = new Func<decimal, decimal, decimal>((valorTotal, valor) =>
            {
                if (valorTotal == 0)
                    return 0m;
                return valor / valorTotal;
            });

            var qtdProdutos = DAL.DashboardDAL.QuantidadeProdutosVendidos();
            var ranking = 1;

            var result = new ProdutosVendidos
            {
                MaisVendidos = top.Select(t => new Model.Dashboard.DTO.Produto { _Produto = t.Produto, Qtd = t.Quantidade, Valor = t.Valor, Ranking = ranking++, Percentual = percentual(valorProdutosVendidos, t.Valor) }).ToList(),
                MenosVendidos = bot.Select(t => new Model.Dashboard.DTO.Produto { _Produto = t.Produto, Qtd = t.Quantidade, Valor = t.Valor, Ranking = qtdProdutos--, Percentual = percentual(valorProdutosVendidos, t.Valor) }).ToList(),
            };
            return result;
        }

        public static MelhoresClientes ObterMelhoresClientes(DateTime date, DateTime? dateFim)
        {
            var melhoresClientes = DAL.DashboardDAL.MelhoresClientes(date, dateFim);

            var ranking = 1;
            return new MelhoresClientes
            {
                TotalDeVendas = melhoresClientes.Select(mc => new Model.Dashboard.DTO.Cliente { Nome = mc.Nome, QtdVisitas = mc.QtdVisitas, TicketMedio = mc.TicketMedio, TotalVendas = mc.TotalVendas, UltimaVisita = mc.UltimaVisita, Ranking = ranking++ }).ToList()
            };
        }

        public static Model.Dashboard.DTO.Fechamento ObterFechamento(int ultimoFechamento)
        {
            var result = new Model.Dashboard.DTO.Fechamento();
            result.SemFechamento = true;

            if (ultimoFechamento <= 0)
                ultimoFechamento = Fechamento.UltimoFechamento();

            if (ultimoFechamento > 0)
            {
                result.SemFechamento = false;
                result.Id = ultimoFechamento;

                var fechamentos = Fechamento.Listar().Where(f => f.IDFechamento != ultimoFechamento && f.DtFechamento != null).OrderByDescending(f => f.DtFechamento);

                result.Fechamentos = fechamentos.Select(f => new FechamentoFiltro { Id = f.IDFechamento.Value, Nome = $"Fechamento {f.DtFechamento.Value.ToString("dd/MM/yyyy - dddd", _cultureInfo)}" }).ToList();

                var fechamentoTipo = DAL.DashboardDAL.ObterFechamentoTipo(ultimoFechamento);
                var fechamentoCaixa = DAL.DashboardDAL.ObterFechamentoCaixa(ultimoFechamento);
                var fechamentoGarcom = DAL.DashboardDAL.ObterFechamentoGarcom(ultimoFechamento);

                result.Abertura = fechamentoCaixa.Min(fc => fc.DataAbertura);
                result._Fechamento = fechamentoCaixa.Max(fc => fc.DataFechamento);

                result.Descontos = fechamentoTipo.Sum(ft => ft.Desconto);
                result.FaturamentoTotal = fechamentoTipo.Sum(ft => ft.Valor);

                result.Caixa = fechamentoCaixa.GroupBy(fc => new { Usuario = fc.Usuario, pdv = fc.PDV }, (key, value) => new Model.Dashboard.DTO.Caixa { Nome = key.pdv, Valor = value.Sum(fc => fc.ValorRecebido), Diferenca = value.Sum(fc => fc.Diferenca), Usuario = key.Usuario }).ToList();
                result.TipoPagamento = fechamentoCaixa.GroupBy(fc => fc.TipoPagamento, (key, value) => new Model.Dashboard.DTO.TipoPagamento { Tipo = key, Valor = value.Sum(fc => fc.ValorRecebido), Percentual = result.FaturamentoTotal == 0 ? 0 : value.Sum(fc => fc.ValorRecebido) / result.FaturamentoTotal }).Where(tp => tp.Valor != 0).ToList();
                result.TipoPedido = fechamentoTipo.GroupBy(ft => ft.Tipo, (key, value) => new Model.Dashboard.DTO.TipoPedido { Tipo = key, Valor = value.Sum(ft => ft.Valor), Quantidade = value.Count() }).ToList();
                result.TotalPedidos = fechamentoTipo.Count();
                var totalGarcom = fechamentoGarcom.Sum(fg => fg.Valor);

                result.Garcom = fechamentoGarcom.OrderByDescending(fg => fg.Valor).Select(fg => new Garcom { Nome = fg.Nome, Valor = fg.Valor, Percentual = totalGarcom == 0 ? 0 : fg.Valor / totalGarcom }).ToList();
            }

            return result;
        }

        public static Resumo ObterResumo(DateTime data, DateTime? dataFim)
        {
            var ticketPedido = DAL.DashboardDAL.ObterDadosResumo(data, dataFim, DAL.DashboardDAL.TipoResumo.Pedido);
            var ticketCliente = DAL.DashboardDAL.ObterDadosResumo(data, dataFim, DAL.DashboardDAL.TipoResumo.Cliente);

            var result = new Resumo
            {
                TotalVendas = ticketPedido.Sum(tp => tp.Valor),
                Cliente = new TicketMedio(),
                Pedido = new TicketMedio()
            };

            result.Pedido.ObterTicketMedio(ticketPedido);
            result.Cliente.ObterTicketMedio(ticketCliente);

            return result;
        }

        private static void ObterTicketMedio(this TicketMedio ticketMedio, IEnumerable<DAL.DashboardDAL.Resumo> ticketsMedio)
        {
            ticketMedio.Quantidade = ticketsMedio.Sum(tm => tm.Quantidade);
            ticketMedio.Total = ticketMedio.Quantidade == 0 ? 0 : ticketsMedio.Sum(tm => tm.Valor) / ticketMedio.Quantidade;

            var obterTicket = new Func<IEnumerable<DAL.DashboardDAL.Resumo>, int, Ticket>((tickets, tipo) =>
           {
               var slice = tickets.Where(r => r.IDTipoPedido == tipo);
               return new Ticket
               {
                   Quantidade = slice.Sum(r => r.Quantidade),
                   Valor = slice.Sum(r => r.Valor)
               };
           });

            ticketMedio.Mesa = obterTicket(ticketsMedio, 10);
            ticketMedio.Comanda = obterTicket(ticketsMedio, 20);
            ticketMedio.Delivery = obterTicket(ticketsMedio, 30);
            ticketMedio.Balcao = obterTicket(ticketsMedio, 40);
        }

        public partial struct ResultadoDashboard
        {
            public Chart FaturamentoPorDia { get; set; }
            public Chart FaturamentoMensal { get; set; }

            public Chart FaturamentoPorCategoria { get; set; }
            public Chart FaturamentoPorTipoDePagamento { get; set; }
            public Chart FaturamentoPorTipoPedido { get; set; }

            public Chart VolumePorGarcom { get; set; }
            public Chart FaturamentoPorDiaDaSemana { get; set; }
            public Chart MotivosCancelamento { get; set; }
        }
    }
}
