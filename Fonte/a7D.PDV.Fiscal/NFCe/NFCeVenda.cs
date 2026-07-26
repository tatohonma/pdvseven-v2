using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using a7D.PDV.BLL;
using a7D.PDV.Model;
using DFe.Classes.Flags;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs.InformacoesCbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.InformacoesIbsCbs.InformacoesIbs;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Compartilhado.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Total.IbsCbs;
using NFe.Classes.Informacoes.Total.IbsCbs.Cbs;
using NFe.Classes.Informacoes.Total.IbsCbs.Ibs;
using NFe.Classes.Informacoes.Transporte;
using NFe.Utils;
using Shared.NFe.Classes.Informacoes.InfRespTec;
using Estado = DFe.Classes.Entidades.Estado;

namespace a7D.PDV.Fiscal.NFCe
{
    public class NFCeVenda
    {
        // http://www.nfe.fazenda.gov.br/portal/exibirArquivo.aspx?conteudo=URCYvjVMIzI=
        public static NFCe CarregarCFe(PedidoInformation pedido, int idNF, bool cpfNaNota, string contabilidadeCpfCnpj)
        {
            var versao = ConfiguracaoServico.Instancia.VersaoLayout;

            var nfe = new NFe.Classes.NFe
            {
                infNFe = new infNFe
                {
                    versao = versao.VersaoServicoParaString(),
                    ide = NFeFacade.GetIdentificacao(idNF),
                    emit = NFeFacade.GetEmitente(),
                    transp = new transp { modFrete = ModalidadeFrete.mfSemFrete } //NFCe: Não pode ter frete
                }
            };

            if (cpfNaNota && !string.IsNullOrEmpty(pedido.DocumentoCliente))
            {
                nfe.infNFe.dest = new dest(versao);

                // Nome do destinatário
                if (pedido.Cliente != null && !string.IsNullOrEmpty(pedido.Cliente.NomeCompleto))
                {
                    nfe.infNFe.dest.xNome = pedido.Cliente.NomeCompleto.Trim();

                    if (nfe.infNFe.dest.xNome.Length < 2)
                        nfe.infNFe.dest.xNome = nfe.infNFe.dest.xNome.PadRight(2, '.');
                }
                else
                {
                    nfe.infNFe.dest.xNome = "CONSUMIDOR FINAL";
                    // nfe.infNFe.dest.enderDest = NFeFacade.GetEnderecoDestinatario();
                }

                // NFC-e: sempre não contribuinte
                nfe.infNFe.dest.indIEDest = indIEDest.NaoContribuinte;

                // Documento (CPF/CNPJ)
                if (cpfNaNota && !string.IsNullOrEmpty(pedido.DocumentoCliente))
                    switch (pedido.DocumentoCliente.Length)
                    {
                        case 11:
                            nfe.infNFe.dest.CPF = pedido.DocumentoCliente;
                            break;

                        case 14:
                            nfe.infNFe.dest.CNPJ = pedido.DocumentoCliente;
                            break;

                        default:
                            // Documento inválido → trata como consumidor padrão
                            nfe.infNFe.dest.CPF = "00000000000";
                            break;
                    }
                else
                    // Sem documento → padrão seguro para NFC-e
                    nfe.infNFe.dest.CPF = "00000000000";
            }

            var listaProduto = new List<PedidoProdutoInformation>();
            ProdutoInformation produto;
            var numeroItem = 0;

            // if (!ConfiguracoesSistema.Valores.ServicoComoItem)
            //     throw new ExceptionPDV(CodigoErro.E515);

            foreach (var item in pedido.ListaProduto)
                if (item.Cancelado != true && Produto.ServicoComoProduto(item.Produto))
                {
                    if (item.ValorUnitario > 0) listaProduto.Add(item);

                    if (item.ListaModificacao != null)
                        foreach (var modificacao in item.ListaModificacao)
                            if (modificacao.Cancelado != true && modificacao.ValorUnitario > 0)
                                listaProduto.Add(modificacao);
                }

            var listaProdutoAgrupado =
                from l in listaProduto
                group l by new { l.Produto.IDProduto, ValorUnitario = l.ValorUnitario ?? 0m }
                into g
                select new
                {
                    g.Key.IDProduto,
                    g.Key.ValorUnitario,
                    Quantidade = g.Sum(x => x.Quantidade ?? 0m),
                    ValorTotal = Math.Round(g.Sum(x => x.ValorTotal), 2, MidpointRounding.AwayFromZero)
                };

            var totalAgrupado = listaProdutoAgrupado.Sum(p => p.ValorTotal);
            var totalSemAgrupar = listaProduto.Sum(p => p.ValorTotal);

            if (totalAgrupado != totalSemAgrupar)
            {
                // Não pode agrupar para não dar divergencia de valor
                // listaProdutoAgrupado =
                //     from l in listaProduto
                //     select new
                //     {
                //         l.Produto.IDProduto,
                //         ValorUnitario = l.ValorUnitario ?? 0,
                //         l.ValorTotal,
                //         Quantidade = l.Quantidade ?? 0
                //     };
            }

            nfe.infNFe.det = new List<det>();

            var totalProdutos = listaProdutoAgrupado.Sum(p => p.ValorTotal);

            var servicoComoItem = ConfiguracoesSistema.Valores.ServicoComoItem;

            var taxaServicoTotal = servicoComoItem ? 0m : pedido.ValorServico ?? 0m;
            var acrescimoPorItem = taxaServicoTotal / totalProdutos;

            var descontoPorItem = (pedido.ValorDesconto ?? 0) / totalProdutos;

            var fretePorItem = (pedido.ValorEntrega ?? 0) / totalProdutos;

            for (var i = 0; i < listaProdutoAgrupado.Count(); i++)
            {
                numeroItem++;
                produto = Produto.Carregar(listaProdutoAgrupado.ElementAt(i).IDProduto.Value);

                if (produto.ClassificacaoFiscal == null)
                    throw new Exception("Categoria de Imposto não relacionada ao produto \"" + produto.Nome + "\"");

                if (produto.Unidade == null)
                    throw new Exception("Unidade comercial não relacionada ao produto \"" + produto.Nome + "\"");

                produto.ClassificacaoFiscal =
                    ClassificacaoFiscal.Carregar(produto.ClassificacaoFiscal.IDClassificacaoFiscal.Value);
                produto.Unidade = Unidade.Carregar(produto.Unidade.IDUnidade.Value);

                //produto.ProdutoImposto = ProdutoImposto.Carregar(produto.ProdutoImposto.IDProdutoImposto.Value);
                var pedidoproduto = listaProdutoAgrupado.ElementAt(i);

                // pedidoproduto.ValorTotal += pedido.ValorEntrega; 

                var descontoDiluido = descontoPorItem > 0 ? descontoPorItem * pedidoproduto.ValorTotal : (decimal?)null;
                var freteDiluido = fretePorItem > 0 ? fretePorItem * pedidoproduto.ValorTotal : (decimal?)null;


                var acrescimoDiluido = acrescimoPorItem != 0
                    ? Math.Round(acrescimoPorItem * pedidoproduto.ValorTotal, 2, MidpointRounding.AwayFromZero)
                    : (decimal?)null;

                var valorUnitarioFiscal = pedidoproduto.Quantidade > 0
                    ? Math.Round(pedidoproduto.ValorTotal / pedidoproduto.Quantidade, 10, MidpointRounding.AwayFromZero)
                    : pedidoproduto.ValorUnitario;

                nfe.infNFe.det.Add(new det());
                nfe.infNFe.det[i].nItem = numeroItem;
                nfe.infNFe.det[i].prod = new prod
                {
                    indTot = IndicadorTotal.ValorDoItemCompoeTotalNF,
                    cProd = pedidoproduto.IDProduto.ToString(),
                    xProd = RemoveInvalidCharacters(produto.Nome),

                    NCM = string.IsNullOrEmpty(produto.ClassificacaoFiscal.NCM)
                        ? string.Empty
                        : produto.ClassificacaoFiscal.NCM.Replace(".", string.Empty),
                    CEST = string.IsNullOrEmpty(produto.ClassificacaoFiscal.CEST)
                        ? null
                        : produto.ClassificacaoFiscal.CEST.Replace(".", string.Empty),
                    cEAN = string.IsNullOrEmpty(produto.cEAN) ? "SEM GTIN" : produto.cEAN,
                    cEANTrib = string.IsNullOrEmpty(produto.cEAN) ? "SEM GTIN" : produto.cEAN,
                    cBenef = string.IsNullOrEmpty(produto.ClassificacaoFiscal.CBENEF)
                        ? null
                        : produto.ClassificacaoFiscal.CBENEF.Replace(".", string.Empty),

                    uCom = produto.Unidade.Simbolo,
                    uTrib = produto.Unidade.Simbolo,

                    qCom = pedidoproduto.Quantidade,
                    qTrib = pedidoproduto.Quantidade,

                    vUnCom = valorUnitarioFiscal,
                    vUnTrib = valorUnitarioFiscal,

                    vProd = Math.Round(pedidoproduto.ValorTotal, 2, MidpointRounding.AwayFromZero),
                    vDesc = descontoDiluido,
                    vFrete = freteDiluido,
                    vOutro = acrescimoDiluido
                };

                if (ConfiguracaoServico.Instancia.cUF == Estado.PE)
                    nfe.infNFe.infRespTec = new infRespTec
                    {
                        CNPJ = "51603516000161",
                        xContato = "PDVSeven",
                        email = "suporte@pdvseven.com.br",
                        fone = "1142100122"
                    };


                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.CFOP))
                    throw new Exception("Campo CFOP do produto " + produto.Nome + " não pode estar vazio.");

                nfe.infNFe.det[i].prod.CFOP = int.Parse(produto.ClassificacaoFiscal.TipoTributacao.CFOP);

                if (string.IsNullOrEmpty(produto.Unidade.Simbolo))
                    throw new Exception("Campo uCom do produto " + produto.Nome + " não pode estar vazio.");

                nfe.infNFe.det[i].imposto = ImpostoProduto(produto.ClassificacaoFiscal.TipoTributacao,
                    nfe.infNFe.det[i].prod.vProd);
            }

            if (taxaServicoTotal != 0m && !servicoComoItem)
            {
                var somaVOutro = nfe.infNFe.det.Sum(d => d.prod.vOutro ?? 0m);
                var diff = Math.Round(taxaServicoTotal - somaVOutro, 2, MidpointRounding.AwayFromZero);

                if (diff != 0m)
                {
                    var last = nfe.infNFe.det.Last();
                    last.prod.vOutro = Math.Round((last.prod.vOutro ?? 0m) + diff, 2, MidpointRounding.AwayFromZero);
                }
            }

            nfe.infNFe.total = GetTotal(versao, nfe.infNFe.det);

            nfe.infNFe.pag = new List<pag> { new pag { detPag = new List<detPag>() } };

            var listapgto = PedidoPagamento.ListaSAT(pedido);

            AdicionarAutXmlContabilidade(nfe, contabilidadeCpfCnpj);


            foreach (var item in listapgto)
            {
                // new pag {tPag = FormaPagamento.fpDinheiro, vPag = valorPagto},

                var det = new detPag
                {
                    tPag = (FormaPagamento)int.Parse(item.Codigo),
                    vPag = item.Valor
                };


                if (det.tPag == FormaPagamento.fpOutro) det.xPag = "Pagamento com o PIX";


                if (det.tPag == FormaPagamento.fpCartaoCredito || det.tPag == FormaPagamento.fpCartaoDebito)
                    det.card = new card
                    {
                        tpIntegra = TipoIntegracaoPagamento.TipNaoIntegrado,
                        tBand = BandeiraCartao.bcMasterCard
                    };

                // nfe.infNFe.pag[0].detPag.Add(new detPag()
                // {
                //     tPag = (FormaPagamento)int.Parse(item.Codigo),
                //     vPag = item.Valor,
                // });

                nfe.infNFe.pag[0].detPag.Add(det);
            }

            // decimal totalNF = Math.Round(nfe.infNFe.total.ICMSTot.vNF, 2, MidpointRounding.AwayFromZero);
            //
            // var dets = nfe.infNFe.pag[0].detPag;

            // decimal totalPagamentos = Math.Round(dets.Sum(p => p.vPag), 2, MidpointRounding.AwayFromZero);
            // decimal totalDinheiro   = Math.Round(
            //     dets.Where(p => p.tPag == FormaPagamento.fpDinheiro).Sum(p => p.vPag),
            //     2, MidpointRounding.AwayFromZero
            // );

            var vNF = Math.Round(nfe.infNFe.total.ICMSTot.vNF, 2, MidpointRounding.AwayFromZero);
            var somaPagamentos =
                Math.Round(nfe.infNFe.pag[0].detPag.Sum(p => p.vPag), 2, MidpointRounding.AwayFromZero);
            var diferenca = Math.Round(somaPagamentos - vNF, 2, MidpointRounding.AwayFromZero);

            if (diferenca != 0)
            {
                var ultimo = nfe.infNFe.pag.Last().detPag.Last();
                ultimo.vPag = Math.Round(ultimo.vPag - diferenca, 2, MidpointRounding.AwayFromZero);

                nfe.infNFe.pag[0].vTroco = null;
            }
            else
            {
                nfe.infNFe.pag[0].vTroco = null;
            }


            return new NFCe { nfe = nfe };
        }

        static void AdicionarAutXmlContabilidade(NFe.Classes.NFe nfe, string contabilidadeCpfCnpj)
        {
            var documentoContabilidade = SomenteNumeros(contabilidadeCpfCnpj);

            // Se não informou CPF/CNPJ da contabilidade, não adiciona autXML
            if (string.IsNullOrEmpty(documentoContabilidade))
                return;

            if (documentoContabilidade.Length != 11 && documentoContabilidade.Length != 14)
                throw new InvalidOperationException(
                    "O CPF/CNPJ da contabilidade deve conter 11 dígitos para CPF ou 14 dígitos para CNPJ."
                );

            if (nfe.infNFe.autXML == null)
                nfe.infNFe.autXML = new List<autXML>();

            if (documentoContabilidade.Length == 14)
                nfe.infNFe.autXML.Add(new autXML
                {
                    CNPJ = documentoContabilidade
                });
            else
                nfe.infNFe.autXML.Add(new autXML
                {
                    CPF = documentoContabilidade
                });
        }

        static string SomenteNumeros(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return new string(valor.Where(char.IsDigit).ToArray());
        }


        static string RemoveInvalidCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "Produto sem nome";

            var value = input
                .Replace("\u00A0", " ")
                .Replace("\u200B", "")
                .Replace("\uFEFF", "")
                .Replace("\t", " ")
                .Replace("\n", " ")
                .Replace("\r", " ");

            value = Regex.Replace(value, @"\s+", " ");

            value = Regex.Replace(value, @"[^0-9A-Za-zÀ-ÿ\s\.,;:/()%+]", "");

            value = value.Trim();

            return value;
        }

        static imposto ImpostoProduto(TipoTributacaoInformation tributacao, decimal vProd)
        {
            var imposto = new imposto();


            if (!string.IsNullOrEmpty(tributacao.ICMS00_Orig)
                && !string.IsNullOrEmpty(tributacao.ICMS00_CST)
                && !string.IsNullOrEmpty(tributacao.ICMS00_pICMS))
            {
                if (imposto.ICMS == null) imposto.ICMS = new ICMS();

                var vBC = vProd;
                var pICMS = tributacao.ICMS00_pICMS.ToAliquota(); // CORRIGIDO
                var vICMS = Math.Round(vBC * pICMS / 100m, 2, MidpointRounding.AwayFromZero);

                imposto.ICMS.TipoICMS = new ICMS00
                {
                    orig = tributacao.ICMS00_Orig.ToEnum<OrigemMercadoria>(),
                    CST = tributacao.ICMS00_CST.ToZeusEnum<Csticms>(),
                    vBC = vBC,
                    pICMS = pICMS,
                    vICMS = vICMS
                };
            }

            if (!string.IsNullOrEmpty(tributacao.ICMS40_Orig)
                && !string.IsNullOrEmpty(tributacao.ICMS40_CST))
            {
                if (imposto.ICMS == null) imposto.ICMS = new ICMS();

                imposto.ICMS.TipoICMS = new ICMS40
                {
                    orig = tributacao.ICMS40_Orig.ToEnum<OrigemMercadoria>(),
                    CST = tributacao.ICMS40_CST.ToZeusEnum<Csticms>()
                };
            }

            if (!string.IsNullOrEmpty(tributacao.ICMS60_Orig) && !string.IsNullOrEmpty(tributacao.ICMS60_CST))
            {
                if (imposto.ICMS == null) imposto.ICMS = new ICMS();

                imposto.ICMS.TipoICMS = new ICMS60
                {
                    orig = tributacao.ICMS60_Orig.ToEnum<OrigemMercadoria>(),
                    CST = tributacao.ICMS60_CST.ToZeusEnum<Csticms>()
                };
            }


            if (!string.IsNullOrEmpty(tributacao.ICMSSN102_Orig) && !string.IsNullOrEmpty(tributacao.ICMSSN102_CSOSN))
            {
                if (imposto.ICMS == null) imposto.ICMS = new ICMS();

                var csosn = tributacao.ICMSSN102_CSOSN.ToEnum<Csosnicms>();

                if (csosn == Csosnicms.Csosn500)
                    imposto.ICMS.TipoICMS = new ICMSSN500
                    {
                        CSOSN = csosn,
                        orig = tributacao.ICMSSN102_Orig.ToEnum<OrigemMercadoria>()
                    };
                else if (csosn == Csosnicms.Csosn900)
                    imposto.ICMS.TipoICMS = new ICMSSN900
                    {
                        CSOSN = csosn,
                        orig = tributacao.ICMSSN102_Orig.ToEnum<OrigemMercadoria>()
                    };
                else
                    imposto.ICMS.TipoICMS = new ICMSSN102
                    {
                        CSOSN = csosn,
                        orig = tributacao.ICMSSN102_Orig.ToEnum<OrigemMercadoria>()
                    };
            }

            if (!string.IsNullOrEmpty(tributacao.ICMSSN900_Orig) && !string.IsNullOrEmpty(tributacao.ICMSSN900_CSOSN) &&
                !string.IsNullOrEmpty(tributacao.ICMSSN900_pICMS))
            {
                if (imposto.ICMS == null) imposto.ICMS = new ICMS();

                var vBC = vProd;
                var pICMS = tributacao.ICMSSN900_pICMS.ToAliquota();
                var vICMS = Math.Round(vBC * pICMS / 100m, 2, MidpointRounding.AwayFromZero);

                imposto.ICMS.TipoICMS = new ICMSSN900
                {
                    orig = tributacao.ICMSSN900_Orig.ToEnum<OrigemMercadoria>(),
                    CSOSN = tributacao.ICMSSN900_CSOSN.ToEnum<Csosnicms>(),
                    vBC = vBC,
                    pICMS = pICMS,
                    vICMS = vICMS
                };
            }

            if (!string.IsNullOrEmpty(tributacao.PISAliq_CST) && !string.IsNullOrEmpty(tributacao.PISAliq_pPIS))
            {
                if (imposto.PIS == null) imposto.PIS = new PIS();

                var pPIS = tributacao.PISAliq_pPIS.ToAliquota();
                var vBC = vProd;
                var vPIS = Math.Round(vBC * pPIS / 100m, 2, MidpointRounding.AwayFromZero);

                imposto.PIS.TipoPIS = new PISAliq
                {
                    CST = tributacao.PISAliq_CST.ToEnum<CSTPIS>(),
                    vBC = vBC,
                    pPIS = pPIS,
                    vPIS = vPIS
                };
            }

            if (!string.IsNullOrEmpty(tributacao.PISQtde_CST) && !string.IsNullOrEmpty(tributacao.PISQtde_vAliqProd))
            {
                if (imposto.PIS == null) imposto.PIS = new PIS();

                imposto.PIS.TipoPIS = new PISQtde
                {
                    CST = tributacao.PISQtde_CST.ToEnum<CSTPIS>(),
                    vAliqProd = tributacao.PISQtde_vAliqProd.ToDecimal()
                };
            }

            if (!string.IsNullOrEmpty(tributacao.PISNT_CST))
            {
                if (imposto.PIS == null) imposto.PIS = new PIS();

                imposto.PIS.TipoPIS = new PISNT
                {
                    CST = tributacao.PISNT_CST.ToEnum<CSTPIS>()
                };
            }

            if (!string.IsNullOrEmpty(tributacao.PISOutr_CST) && !string.IsNullOrEmpty(tributacao.PISOutr_pPIS) &&
                !string.IsNullOrEmpty(tributacao.PISOutr_vAliqProd))
            {
                if (imposto.PIS == null) imposto.PIS = new PIS();

                imposto.PIS.TipoPIS = new PISOutr
                {
                    CST = tributacao.PISOutr_CST.ToEnum<CSTPIS>(),
                    pPIS = tributacao.PISOutr_pPIS.ToAliquota(),
                    vAliqProd = tributacao.PISOutr_vAliqProd.ToDecimal(),
                    vBC = vProd
                };
            }

            if (!string.IsNullOrEmpty(tributacao.COFINSAliq_CST) &&
                !string.IsNullOrEmpty(tributacao.COFINSAliq_pCOFINS))
            {
                if (imposto.COFINS == null) imposto.COFINS = new COFINS();

                var pCOFINS = tributacao.COFINSAliq_pCOFINS.ToAliquota();
                var vBC = vProd;
                var vCOFINS = Math.Round(vBC * pCOFINS / 100m, 2, MidpointRounding.AwayFromZero);

                imposto.COFINS.TipoCOFINS = new COFINSAliq
                {
                    CST = tributacao.COFINSAliq_CST.ToEnum<CSTCOFINS>(),
                    vBC = vBC,
                    pCOFINS = pCOFINS,
                    vCOFINS = vCOFINS
                };
            }

            if (!string.IsNullOrEmpty(tributacao.COFINSQtde_CST)
                && !string.IsNullOrEmpty(tributacao.COFINSQtde_vAliqProd))
            {
                if (imposto.COFINS == null) imposto.COFINS = new COFINS();

                imposto.COFINS.TipoCOFINS = new COFINSQtde
                {
                    CST = tributacao.COFINSQtde_CST.ToEnum<CSTCOFINS>(),
                    vAliqProd = tributacao.COFINSQtde_vAliqProd.ToDecimal()
                };
            }

            if (!string.IsNullOrEmpty(tributacao.COFINSOutr_CST) &&
                !string.IsNullOrEmpty(tributacao.COFINSOutr_pCOFINS) &&
                !string.IsNullOrEmpty(tributacao.COFINSOutr_vAliqProd))
            {
                if (imposto.COFINS == null) imposto.COFINS = new COFINS();

                imposto.COFINS.TipoCOFINS = new COFINSOutr
                {
                    CST = tributacao.COFINSOutr_CST.ToEnum<CSTCOFINS>(),
                    pCOFINS = tributacao.COFINSOutr_pCOFINS.ToAliquota(),
                    vAliqProd = tributacao.COFINSOutr_vAliqProd.ToDecimal(),
                    vBC = vProd
                };
            }

            if (!string.IsNullOrEmpty(tributacao.COFINSNT_CST))
            {
                if (imposto.COFINS == null) imposto.COFINS = new COFINS();

                imposto.COFINS.TipoCOFINS = new COFINSNT
                {
                    CST = tributacao.COFINSNT_CST.ToEnum<CSTCOFINS>()
                };
            }

            if (NFeFacade.Config.NFCe_CRT != 1 && !string.IsNullOrEmpty(tributacao.IBSCBS_CST) && !string.IsNullOrEmpty(tributacao.IBSCBS_cClassTrib))
            {
                var vBC = !string.IsNullOrEmpty(tributacao.IBSCBS_vBC)
                    ? tributacao.IBSCBS_vBC.ToDecimal()
                    : vProd;

                var pIBSUF = !string.IsNullOrEmpty(tributacao.IBSCBS_pAliqIBSUF)
                    ? tributacao.IBSCBS_pAliqIBSUF.ToAliquota()
                    : 0m;

                var pIBSMun = !string.IsNullOrEmpty(tributacao.IBSCBS_pAliqIBSMun)
                    ? tributacao.IBSCBS_pAliqIBSMun.ToAliquota()
                    : 0m;

                var pCBS = !string.IsNullOrEmpty(tributacao.IBSCBS_pCBS)
                    ? tributacao.IBSCBS_pCBS.ToAliquota()
                    : 0m;

                var vIBSUF = Math.Round(vBC * pIBSUF / 100m, 2, MidpointRounding.AwayFromZero);
                var vIBSMun = Math.Round(vBC * pIBSMun / 100m, 2, MidpointRounding.AwayFromZero);
                var vIBS = Math.Round(vIBSUF + vIBSMun, 2, MidpointRounding.AwayFromZero);
                var vCBS = Math.Round(vBC * pCBS / 100m, 2, MidpointRounding.AwayFromZero);

                imposto.IBSCBS = new IBSCBS
                {
                    CST = tributacao.IBSCBS_CST.ToZeusEnum<CST>(),
                    cClassTrib = tributacao.IBSCBS_cClassTrib,
                    gIBSCBS = new gIBSCBS
                    {
                        vBC = vBC,
                        gIBSUF = new gIBSUF
                        {
                            pIBSUF = pIBSUF,
                            vIBSUF = vIBSUF
                        },
                        gIBSMun = new gIBSMun
                        {
                            pIBSMun = pIBSMun,
                            vIBSMun = vIBSMun
                        },
                        vIBS = vIBS,
                        gCBS = new gCBS
                        {
                            pCBS = pCBS,
                            vCBS = vCBS
                        }
                    }
                };
            }

            return imposto;
        }


        static total GetTotal(VersaoServico versao, List<det> produtos)
        {
            var icmsTot = new ICMSTot
            {
                vProd = produtos.Sum(p => p.prod.vProd),
                vDesc = produtos.Sum(p => p.prod.vDesc ?? 0),
                vFrete = produtos.Sum(p => p.prod.vFrete ?? 0),
                vOutro = produtos.Sum(p => p.prod.vOutro ?? 0),
                vTotTrib = produtos.Sum(p => p.imposto.vTotTrib ?? 0),
                vICMSDeson = 0,
                vFCPUFDest = 0,
                vICMSUFDest = 0,
                vICMSUFRemet = 0,
                vFCP = 0,
                vFCPST = 0,
                vFCPSTRet = 0,
                vIPIDevol = 0
            };

            var totalVBCIBSCBS = 0m;
            var totalVIBSUF = 0m;
            var totalVIBSMun = 0m;
            var totalVIBS = 0m;
            var totalVCBS = 0m;

            foreach (var produto in produtos)
            {
                if (produto.imposto.IPI != null && produto.imposto.IPI.TipoIPI.GetType() == typeof(IPITrib))
                    icmsTot.vIPI = icmsTot.vIPI + ((IPITrib)produto.imposto.IPI.TipoIPI).vIPI ?? 0;

                if (produto.imposto.ICMS?.TipoICMS is ICMS00 icms00)
                {
                    icmsTot.vBC += icms00.vBC;
                    icmsTot.vICMS += icms00.vICMS;
                }
                else if (produto.imposto.ICMS?.TipoICMS is ICMS20 icms20)
                {
                    icmsTot.vBC += icms20.vBC;
                    icmsTot.vICMS += icms20.vICMS;
                }

                if (produto.imposto.PIS?.TipoPIS is PISAliq pisAliq)
                    icmsTot.vPIS += pisAliq.vPIS;
                else if (produto.imposto.PIS?.TipoPIS is PISOutr pisOutr)
                    icmsTot.vPIS += pisOutr.vPIS ?? 0m;

                if (produto.imposto.COFINS?.TipoCOFINS is COFINSAliq cofAliq)
                    icmsTot.vCOFINS += cofAliq.vCOFINS;
                else if (produto.imposto.COFINS?.TipoCOFINS is COFINSOutr cofOutr)
                    icmsTot.vCOFINS += cofOutr.vCOFINS ?? 0m;

                var gIBSCBS = produto.imposto?.IBSCBS?.gIBSCBS;

                if (gIBSCBS != null)
                {
                    totalVBCIBSCBS += gIBSCBS.vBC;
                    totalVIBSUF += gIBSCBS.gIBSUF?.vIBSUF ?? 0m;
                    totalVIBSMun += gIBSCBS.gIBSMun?.vIBSMun ?? 0m;
                    totalVIBS += gIBSCBS.vIBS;
                    totalVCBS += gIBSCBS.gCBS?.vCBS ?? 0m;
                }
            }

            icmsTot.vNF =
                icmsTot.vProd
                - icmsTot.vDesc
                - icmsTot.vICMSDeson.GetValueOrDefault()
                + icmsTot.vST
                + icmsTot.vFCPST.GetValueOrDefault()
                + icmsTot.vFrete
                + icmsTot.vSeg
                + icmsTot.vOutro
                + icmsTot.vII
                + icmsTot.vIPI
                + icmsTot.vIPIDevol.GetValueOrDefault();

            var t = new total { ICMSTot = icmsTot };

            var temIBSCBS = produtos.Any(p => p.imposto?.IBSCBS?.gIBSCBS != null);

            if (temIBSCBS)
            {
                totalVBCIBSCBS = Math.Round(totalVBCIBSCBS, 2, MidpointRounding.AwayFromZero);
                totalVIBSUF = Math.Round(totalVIBSUF, 2, MidpointRounding.AwayFromZero);
                totalVIBSMun = Math.Round(totalVIBSMun, 2, MidpointRounding.AwayFromZero);
                totalVIBS = Math.Round(totalVIBS, 2, MidpointRounding.AwayFromZero);
                totalVCBS = Math.Round(totalVCBS, 2, MidpointRounding.AwayFromZero);

                t.IBSCBSTot = new IBSCBSTot
                {
                    vBCIBSCBS = totalVBCIBSCBS,

                    gIBS = new gIBS
                    {
                        gIBSUF = new gIBSUFTotal
                        {
                            vDif = 0m,
                            vDevTrib = 0m,
                            vIBSUF = totalVIBSUF
                        },

                        gIBSMun = new gIBSMunTotal
                        {
                            vDif = 0m,
                            vDevTrib = 0m,
                            vIBSMun = totalVIBSMun
                        },

                        vIBS = totalVIBS,
                        vCredPres = 0m,
                        vCredPresCondSus = 0m
                    },

                    gCBS = new gCBSTotal()
                    {
                        vDif = 0m,
                        vDevTrib = 0m,
                        vCBS = totalVCBS,
                        vCredPres = 0m,
                        vCredPresCondSus = 0m
                    }
                };

                t.vNFTot = Math.Round(icmsTot.vNF + totalVIBS + totalVCBS, 2, MidpointRounding.AwayFromZero);
            }

            return t;
        }
    }
}
