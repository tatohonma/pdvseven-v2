using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace a7D.PDV.Fiscal.Services
{
    public static class SATService
    {
        public static SATStatusLocal TesteConexaoSat()
        {
            try
            {
                SATStatusLocal resposta;
                var url = new Uri(FiscalServices.urlWSfiscal);
                var httpClient = new HttpClient();
                var result = httpClient.GetAsync(new Uri(url, "/api/Sat")).Result;
                var text = result.Content.ReadAsStringAsync().Result;
                if (text.StartsWith("{"))
                {
                    resposta = JsonConvert.DeserializeObject<SATStatusLocal>(text);
                    var retornoSat = resposta.mensagem ?? resposta.ExceptionMessage ?? resposta.Message;

                    if (resposta.Message != null || retornoSat?.ToLower().Contains("erro") == true)
                    {
                        resposta.mensagem = retornoSat.Replace("erro|", "");
                        resposta.EmOperacao = false;
                    }
                    else
                        resposta.EmOperacao = true;
                }
                else
                {
                    resposta = new SATStatusLocal()
                    {
                        mensagem = "Erro ao ler serviço em: " + url.AbsolutePath,
                        EmOperacao = false
                    };
                }

                return resposta;
            }
            catch (Exception ex)
            {
                return new SATStatusLocal() { EmOperacao = false, mensagem = ex.Message };
            }
        }

        private static readonly CultureInfo culture = CultureInfo.GetCultureInfo("pt-BR");

        //private static string Split(string texto, int colunas)
        //{
        //    var linhas = Enumerable.Range(0, texto.Length / colunas)
        //        .Select(i => texto.Substring(i * colunas, colunas));
        //    return String.Join(Environment.NewLine, linhas);
        //}

        private static string Center(string texto, int colunas)
        {
            int tamanho = (colunas - texto.Length) / 2;
            return tamanho > 0 ? (new string(' ', tamanho) + texto) : texto;
        }

        private static string DadoValor(string dado, string valor, int colunas)
        {
            if (dado.Length + valor.Length >= colunas)
            {
                // Imprime em até 2 linhas
                string linha1, linha2;
                if (dado.Length > colunas)
                {
                    linha1 = dado.Substring(0, colunas);
                    linha2 = dado.Substring(colunas);
                    if (linha2.Length > colunas)
                        linha2 = linha2.Substring(0, colunas);

                    return linha1 + Environment.NewLine + DadoValor(linha2, valor, colunas);
                }
                else
                {
                    if (valor.Length > colunas)
                        valor = valor.Substring(0, colunas);

                    linha1 = dado;
                    linha2 = new string(' ', colunas - valor.Length) + valor;
                    return linha1 + Environment.NewLine + linha2;
                }
            }
            else
                return dado + new string(' ', colunas - dado.Length - valor.Length) + valor;
        }

        public static string CupomVendaTexto(int idPedido, int colunas)
        {
            var pedido = Pedido.CarregarCompleto(idPedido);
            if (pedido?.RetornoSAT_venda?.IDRetornoSAT == null)
                return null;

            var retornoSAT = RetornoSAT.Carregar(pedido.RetornoSAT_venda.IDRetornoSAT.Value);
            if (retornoSAT == null)
                return null;

            string info = pedido.TipoPedido.Nome;
            if (pedido.TipoPedido.TipoPedido == ETipoPedido.Mesa)
            {
                var mesa = Mesa.CarregarPorGUID(pedido.GUIDIdentificacao);
                if (mesa != null)
                    info += ": " + mesa.Numero;
            }
            else if (pedido.TipoPedido.TipoPedido == ETipoPedido.Comanda)
            {
                var comanda = Comanda.CarregarPorGUID(pedido.GUIDIdentificacao);
                if (comanda != null)
                    info += ": " + comanda.Numero;
            }

            return CupomVendaTexto(retornoSAT.arquivoCFeSAT, pedido.IDPedido.Value, info, colunas);
        }

        public static string CupomVendaTexto(string arquivoCFeSAT, int pedido, string identificacao, int colunas)
        {
            //https://portal.fazenda.sp.gov.br/servicos/sat/Downloads/Manual_Orientacao_SAT_v_MO_2_4_05.pdf
            var texto = new StringBuilder();
            var xmlTexto = Encoding.UTF8.GetString(Convert.FromBase64String(arquivoCFeSAT));
            var xmlDoc = XDocument.Parse(xmlTexto, LoadOptions.None);

            #region Informações unicas do XML

            string chaveConsulta = xmlDoc.Root.Descendants("infCFe").
                    Attributes("Id").
                    Select(p => p.Value).
                    FirstOrDefault();

            string nomeFantasia = xmlDoc.Root.Descendants("emit").
                    Descendants("xFant").
                    Select(p => p.Value).
                    FirstOrDefault();

            string razaoSocial = xmlDoc.Root.Descendants("emit").
                    Descendants("xNome").
                    Select(p => p.Value).
                    FirstOrDefault();

            string rua = xmlDoc.Root.Descendants("emit").
                    Descendants("enderEmit").
                    Descendants("xLgr").
                    Select(p => p.Value).
                    FirstOrDefault();

            string numero = xmlDoc.Root.Descendants("emit").
                    Descendants("enderEmit").
                    Descendants("nro").
                    Select(p => p.Value).
                    FirstOrDefault();

            string bairro = xmlDoc.Root.Descendants("emit").
                    Descendants("enderEmit").
                    Descendants("xBairro").
                    Select(p => p.Value).
                    FirstOrDefault();

            string municipio = xmlDoc.Root.Descendants("emit").
                    Descendants("enderEmit").
                    Descendants("xMun").
                    Select(p => p.Value).
                    FirstOrDefault();

            string cep = xmlDoc.Root.Descendants("emit").
                    Descendants("enderEmit").
                    Descendants("CEP").
                    Select(p => p.Value).
                    FirstOrDefault();

            string cnpj = xmlDoc.Root.Descendants("emit").
                    Descendants("CNPJ").
                    Select(p => p.Value).
                    LastOrDefault();

            string ie = xmlDoc.Root.Descendants("emit").
                    Descendants("IE").
                    Select(p => p.Value).
                    FirstOrDefault();

            string im = xmlDoc.Root.Descendants("emit").
                    Descendants("IM").
                    Select(p => p.Value).
                    FirstOrDefault();

            string numeroSAT = xmlDoc.Root.Descendants("ide").
                    Descendants("nserieSAT").
                    Select(p => p.Value).
                    FirstOrDefault();

            string dataEmissao = xmlDoc.Root.Descendants("ide").
                    Descendants("dEmi").
                    Select(p => p.Value).
                    FirstOrDefault();

            string horaEmissao = xmlDoc.Root.Descendants("ide").
                    Descendants("hEmi").
                    Select(p => p.Value).
                    FirstOrDefault();

            string numeroDocumento = xmlDoc.Root.Descendants("nCFe").
                    Select(p => p.Value).
                    FirstOrDefault();

            string CPFCNPJCliente = xmlDoc.Root.Descendants("dest")
                    .Descendants("CPF")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(CPFCNPJCliente))
                CPFCNPJCliente = xmlDoc.Root.Descendants("dest")
                    .Descendants("CNPJ")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            string valorTotal = xmlDoc.Root.Descendants("total")
                    .Descendants("vProd")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            string valorDesconto = xmlDoc.Root.Descendants("total")
                    .Descendants("vDescSubtot")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            string valorAcrescimo = xmlDoc.Root.Descendants("total")
                    .Descendants("vAcresSubtot")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            string valorTotalAPagar = xmlDoc.Root.Descendants("total")
                    .Descendants("vCFe")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            string valorTroco = xmlDoc.Root.Descendants("vTroco")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            string assinaturaQRCode = xmlDoc.Root.Descendants("ide")
                    .Descendants("assinaturaQRCODE")
                    .Select(p => p.Value)
                    .FirstOrDefault();

            #endregion

            #region Tratamento de dados

            if (string.IsNullOrWhiteSpace(valorAcrescimo))
                valorAcrescimo = "0.00";

            if (string.IsNullOrWhiteSpace(valorDesconto))
                valorDesconto = "0.00";

            var vvDesconto = Convert.ToDecimal(valorDesconto.Replace(".", ","));
            var vvAcrescimo = Convert.ToDecimal(valorAcrescimo.Replace(".", ","));
            var vvTotal = Convert.ToDecimal(valorTotal.Replace(".", ","));
            var vvTotalAPagar = Convert.ToDecimal(valorTotalAPagar.Replace(".", ","));
            var vvTroco = Convert.ToDecimal(valorTroco.Replace(".", ","));

            string chaveConsultaParaCodigoBarras = chaveConsulta.Replace("CFe", "");

            var dtEmissao = DateTime.ParseExact($"{dataEmissao}{horaEmissao}", "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);

            string dadosQRCODE = string.Format("{0}|{1}{2}|{3}|{4}|{5}",
                chaveConsulta.StartsWith("CFe", true, CultureInfo.InvariantCulture) ? chaveConsulta.Remove(0, 3) : chaveConsulta,
                dataEmissao, horaEmissao, valorTotalAPagar, CPFCNPJCliente, assinaturaQRCode);

            #endregion

            // Header!

            if (!string.IsNullOrEmpty(nomeFantasia))
                texto.AppendLine(Center($"{nomeFantasia}", colunas));
            texto.AppendLine(Center($"{razaoSocial}", colunas));
            texto.AppendLine();

            texto.AppendLine(Center($"{rua}, {numero}", colunas));
            texto.AppendLine(Center($"{bairro}, {municipio}", colunas));
            texto.AppendLine(Center($"CEP: {cep}", colunas));

            texto.AppendLine();
            texto.AppendLine(Center($"CNPJ {cnpj} IE {ie}", colunas));
            if (!string.IsNullOrEmpty(im))
                texto.AppendLine(Center($"IM {im}", colunas));

            texto.AppendLine(new string('-', colunas));
            texto.AppendLine(Center($"Extrato No. {numeroDocumento}", colunas));
            texto.AppendLine(Center("CUPOM FISCAL ELETRÔNICO - SAT", colunas));
            texto.AppendLine(new string('-', colunas));
            texto.AppendLine(DadoValor("Pedido: " + pedido, identificacao, colunas));
            texto.AppendLine(new string('-', colunas));
            texto.AppendLine($"CPF/CNPJ Consumidor: {CPFCNPJCliente}");
            //if (!string.IsNullOrEmpty(cliente))
            //    texto.AppendLine($"{cliente}");

            texto.AppendLine(new string('-', colunas));
            if (colunas > 51)
                texto.AppendLine(DadoValor("#DESC | QTD | UN | VL UN R$ | (VL TR R$)*", "VL ITEM R$", colunas));
            else
                texto.AppendLine(DadoValor("#DESC|QTD|UN|VL UN R$|(VL TR R$)*", "VL ITEM R$", colunas));

            texto.AppendLine(new string('-', colunas));

            #region Itens

            var listaElementos = xmlDoc.Root.Descendants("det").ToList();

            var listaProduto = new List<string>();
            //var listaNova = new Dictionary<string, decimal>();
            decimal impostosTotais = 0m;

            foreach (XElement elemento in listaElementos)
            {
                var nItem = elemento.Attributes("nItem").Select(p => p.Value).FirstOrDefault();
                var produtos = elemento.Descendants("prod").ToList();
                var imposto = elemento.Descendants("imposto").FirstOrDefault();

                foreach (XElement elementoProduto in produtos)
                {
                    var vProd = elementoProduto.Descendants("vProd").Select(p => p.Value).FirstOrDefault();
                    var xProd = elementoProduto.Descendants("xProd").Select(p => p.Value).FirstOrDefault();
                    var cProd = elementoProduto.Descendants("cProd").Select(p => p.Value).FirstOrDefault();
                    var uCom = elementoProduto.Descendants("uCom").Select(p => p.Value).FirstOrDefault();
                    var qCom = elementoProduto.Descendants("qCom").Select(p => p.Value).FirstOrDefault();
                    var vUnCom = elementoProduto.Descendants("vUnCom").Select(p => p.Value).FirstOrDefault();
                    var vRatDesc = elementoProduto.Descendants("vRatDesc").Select(p => p.Value).FirstOrDefault();
                    var vRatAcr = elementoProduto.Descendants("vRatAcr").Select(p => p.Value).FirstOrDefault();
                    var vItem = elementoProduto.Descendants("vItem").Select(p => p.Value).FirstOrDefault();
                    var vItem12741 = imposto.Descendants("vItem12741").Select(p => p.Value).FirstOrDefault();

                    var dadosItem = Convert.ToDecimal(qCom.Replace(".", ",")).ToString("N2", culture) + uCom + "x";
                    dadosItem += Convert.ToDecimal(vUnCom.Replace(".", ",")).ToString("N2", culture);

                    var vUcomDec = Convert.ToDecimal(vUnCom.Replace(".", ","));
                    var qComDec = Convert.ToDecimal(qCom.Replace(".", ","));

                    var impostosLinha = 0m;
                    if (string.IsNullOrWhiteSpace(vItem12741) == false && decimal.TryParse(vItem12741.Replace(".", ","), out decimal dvItem12741))
                    {
                        impostosLinha = dvItem12741;
                        impostosTotais += impostosLinha;
                    }

                    var cItem = $"{nItem} {xProd} {qComDec.ToString("#0.#")} {uCom} {vUcomDec.ToString("N2", culture)} ({impostosLinha.ToString("N2")})";
                    var cValor = Convert.ToDecimal(vProd.Replace(".", ",")).ToString("N2", culture);
                    texto.AppendLine(DadoValor(cItem, cValor, colunas));
                }
            }

            #endregion

            #region Totais

            texto.AppendLine();
            texto.AppendLine(DadoValor("Subtotal", vvTotal.ToString("N2", culture), colunas));

            if (vvDesconto > 0m)
                texto.AppendLine(DadoValor("Desconto R$", vvDesconto.ToString("N2", culture), colunas));

            if (vvAcrescimo > 0m)
                texto.AppendLine(DadoValor("Acréscimos R$", vvAcrescimo.ToString("N2", culture), colunas));

            texto.AppendLine(DadoValor("Total", vvTotalAPagar.ToString("N2", culture), colunas));

            texto.AppendLine();
            foreach (var pagamento in xmlDoc.Root.Descendants("MP"))
            {
                var codigo = pagamento.Descendants("cMP").Select(p => p.Value).FirstOrDefault();
                var valor = pagamento.Descendants("vMP").Select(p => p.Value).FirstOrDefault();
                var vvValor = Convert.ToDecimal(valor.Replace(".", ","));
                //var meio = MeioPagamentoSAT.CarregarPorCodigo(codigo)?.Descricao ?? codigo;
                var meio = Enum.Parse(typeof(ECodigosPagamentoSAT), codigo).ToString();
                texto.AppendLine(DadoValor(meio, vvValor.ToString("N2", culture), colunas));
            }

            texto.AppendLine(DadoValor("Troco R$", vvTroco.ToString("N2", culture), colunas));

            #endregion

            #region Final

            texto.AppendLine(new string('-', colunas));
            texto.AppendLine(DadoValor("Tributos Totais (Lei Fed 12.741/12)", impostosTotais.ToString("N2", culture), colunas));
            texto.AppendLine("* Valor aproximado dos tributos do item");
            texto.AppendLine();

            texto.AppendLine(Center($"SAT No. {numeroSAT}", colunas));
            texto.AppendLine(new string('-', colunas));
            texto.AppendLine(Center(dtEmissao.ToString("dd/MM/yyyy - HH:mm:sss"), colunas));
            texto.AppendLine();

            var linhaBarras = "";
            for (int i = 0; i < chaveConsultaParaCodigoBarras.Length; i += 4)
            {
                linhaBarras += chaveConsultaParaCodigoBarras.Substring(i, 4);
                if (i == 16)
                {
                    texto.AppendLine(Center(linhaBarras, colunas));
                    linhaBarras = "";
                }
                else
                    linhaBarras += " ";
            }
            texto.AppendLine(Center(linhaBarras, colunas));


            int n = chaveConsultaParaCodigoBarras.Length / 2;
            var barras1 = chaveConsultaParaCodigoBarras.Substring(0, n);
            var barras2 = chaveConsultaParaCodigoBarras.Substring(n);
            texto.AppendLine("@ITF14:" + barras1);
            texto.AppendLine("@ITF14:" + barras2);
            texto.AppendLine("@QRCODE:" + dadosQRCODE);

            #endregion

            texto.AppendLine();
            texto.AppendLine(Center("*************** PDVSeven ***************", colunas));
            texto.AppendLine(Center("*  SISTEMAS PARA RESTAURANTES E BARES  *", colunas));
            texto.AppendLine(Center("www.pdvseven.com.br", colunas));

            return texto.ToString();
        }
    }
}