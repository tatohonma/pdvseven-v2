using a7D.PDV.BLL;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using System;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.Fiscal.SAT._008
{
    public class CFeVenda
    {
        public static CFe CarregarCFe(PedidoInformation pedido, bool cpfNaNota)
        {
            var configuracoesSat = new ConfiguracoesSAT();
            var enviar12741 = configuracoesSat.InfCFe_12741;
            CFe cfe = new CFe();
            cfe.infCFe = new _infCFe();
            cfe.infCFe.versaoDadosEnt = configuracoesSat.InfCFe_versaoDadosEnt;

            #region ide

            cfe.infCFe.ide = new _ide();
            cfe.infCFe.ide.CNPJ = configuracoesSat.InfCFe_ide_CNPJ;
            cfe.infCFe.ide.signAC = configuracoesSat.InfCFe_ide_signAC;
            cfe.infCFe.ide.numeroCaixa = pedido.Caixa.PDV.IDPDV.Value.ToString().PadLeft(3, '0').Substring(0, 3);

            #endregion

            #region emit

            cfe.infCFe.emit = new _emit();
            cfe.infCFe.emit.CNPJ = configuracoesSat.InfCFe_emit_CNPJ;
            cfe.infCFe.emit.IE = configuracoesSat.InfCFe_emit_IE;
            cfe.infCFe.emit.IM = string.IsNullOrWhiteSpace(configuracoesSat.InfCFe_emit_IM) ? null : configuracoesSat.InfCFe_emit_IM;
            cfe.infCFe.emit.indRatISSQN = configuracoesSat.InfCFe_emit_indRatISSQN;

            #endregion

            #region dest

            cfe.infCFe.dest = new _dest();

            if (cpfNaNota)
            {
                if (!string.IsNullOrEmpty(pedido.DocumentoCliente))
                {
                    switch (pedido.DocumentoCliente.Length)
                    {
                        case 11:
                            cfe.infCFe.dest.CPF = pedido.DocumentoCliente;
                            break;

                        case 14:
                            cfe.infCFe.dest.CNPJ = pedido.DocumentoCliente;
                            break;
                    }
                }
            }

            if (pedido.Cliente != null)
            {
                cfe.infCFe.dest.xNome = pedido.Cliente.NomeCompleto;
                if (cfe.infCFe.dest.xNome.Length < 2)
                    cfe.infCFe.dest.xNome = cfe.infCFe.dest.xNome.PadRight(2, '.');
            }

            #endregion

            #region det

            List<PedidoProdutoInformation> listaProduto = new List<PedidoProdutoInformation>();
            ProdutoInformation produto;
            Int32 numeroItem = 0;

            //var servico = pedido.ListaProduto
            //                .Where(pp => pp.Cancelado == false)
            //                .Where(pp => pp.Produto.IDProduto == 4)
            //                .GroupBy(pp => new { IDProduto = pp.Produto.IDProduto, Produto = pp.Produto }, (key, values) => new { Produto = key.Produto, Valor = values.Sum(x => x.Quantidade * x.ValorUnitario) })
            //                .Sum(g => g.Valor);
            var valorServico = pedido.ValorServico.Value;
            if (ConfiguracoesSistema.Valores.ServicoComoItem)
                valorServico = 0;

            foreach (var item in pedido.ListaProduto)
            {
                if (item.Cancelado != true && item.Produto.IDProduto.Value != 4)
                {
                    if (item.ValorUnitario > 0)
                    {
                        listaProduto.Add(item);
                    }

                    if (item.ListaModificacao != null)
                    {
                        foreach (var modificacao in item.ListaModificacao)
                        {
                            if (modificacao.Cancelado != true && modificacao.ValorUnitario > 0)
                                listaProduto.Add(modificacao);
                        }
                    }
                }
            }

            var listaProdutoAgrupado =
                from l in listaProduto
                group l by new { l.Produto.IDProduto, l.ValorUnitario } into g
                select new
                {
                    IDProduto = g.Key.IDProduto,
                    Valor = g.Key.ValorUnitario,
                    Quantidade = g.Sum(x => x.Quantidade)
                };

            var totalAgrupado = listaProdutoAgrupado.Sum(p => Math.Truncate(p.Quantidade.Value * p.Valor.Value * 100m) / 100m);
            var totalSemAgrupar = listaProduto.Sum(p => p.ValorTotal);

            if (totalAgrupado != totalSemAgrupar)

            {
                // Não pode agrupar para não dar divergencia de valor
                listaProdutoAgrupado =
                    from l in listaProduto
                    select new
                    {
                        IDProduto = l.Produto.IDProduto,
                        Valor = l.ValorUnitario,
                        Quantidade = l.Quantidade
                    };
            }

            cfe.infCFe.det = new _det[listaProdutoAgrupado.Count()];
            var cultureInfo = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            for (int i = 0; i < listaProdutoAgrupado.Count(); i++)
            {
                numeroItem++;
                produto = Produto.Carregar(listaProdutoAgrupado.ElementAt(i).IDProduto.Value);

                if (produto.ClassificacaoFiscal == null)
                    throw new Exception("Categoria de Imposto não relacionada ao produto \"" + produto.Nome + "\"");

                if (produto.Unidade == null)
                    throw new Exception("Unidade comercial não relacionada ao produto \"" + produto.Nome + "\"");

                produto.ClassificacaoFiscal = ClassificacaoFiscal.Carregar(produto.ClassificacaoFiscal.IDClassificacaoFiscal.Value);
                produto.Unidade = Unidade.Carregar(produto.Unidade.IDUnidade.Value);

                //produto.ProdutoImposto = ProdutoImposto.Carregar(produto.ProdutoImposto.IDProdutoImposto.Value);

                cfe.infCFe.det[i] = new _det();
                cfe.infCFe.det[i].nItem = numeroItem.ToString();
                cfe.infCFe.det[i].prod = new _prod();
                cfe.infCFe.det[i].prod.cProd = listaProdutoAgrupado.ElementAt(i).IDProduto.ToString();
                cfe.infCFe.det[i].prod.xProd = produto.Nome;
                cfe.infCFe.det[i].prod.NCM = string.IsNullOrEmpty(produto.ClassificacaoFiscal.NCM) ? string.Empty : produto.ClassificacaoFiscal.NCM.Replace(".", string.Empty);


                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.CFOP))
                    throw new Exception("Campo CFOP do produto " + produto.Nome + " não pode estar vazio.");

                cfe.infCFe.det[i].prod.CFOP = produto.ClassificacaoFiscal.TipoTributacao.CFOP;

                if (string.IsNullOrEmpty(produto.Unidade.Simbolo))
                    throw new Exception("Campo uCom do produto " + produto.Nome + " não pode estar vazio.");

                cfe.infCFe.det[i].prod.uCom = produto.Unidade.Simbolo;
                cfe.infCFe.det[i].prod.qCom = listaProdutoAgrupado.ElementAt(i).Quantidade.Value.ToString("0.0000", cultureInfo);
                cfe.infCFe.det[i].prod.vUnCom = listaProdutoAgrupado.ElementAt(i).Valor.Value.ToString("0.00", cultureInfo);


                cfe.infCFe.det[i].prod.indRegra = "T"; // configuracoesSat.InfCFe_det_prod_indRegra;

                cfe.infCFe.det[i].imposto = new _imposto();

                var valor12741 = EnviarSATBase.CalcularImpostos(listaProdutoAgrupado.ElementAt(i).IDProduto.Value, listaProdutoAgrupado.ElementAt(i).Quantidade ?? 0, listaProdutoAgrupado.ElementAt(i).Valor ?? 0);

                if (enviar12741 && valor12741 >= 0)
                {
                    cfe.infCFe.det[i].imposto.vItem12741 = valor12741.ToString("0.00", cultureInfo);
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.ICMS00_Orig) == false)
                {
                    if (cfe.infCFe.det[i].imposto.ICMS == null)
                        cfe.infCFe.det[i].imposto.ICMS = new _ICMS();

                    cfe.infCFe.det[i].imposto.ICMS.ICMS00 = new _ICMS00();
                    cfe.infCFe.det[i].imposto.ICMS.ICMS00.Orig = produto.ClassificacaoFiscal.TipoTributacao.ICMS00_Orig;
                    cfe.infCFe.det[i].imposto.ICMS.ICMS00.CST = produto.ClassificacaoFiscal.TipoTributacao.ICMS00_CST;
                    cfe.infCFe.det[i].imposto.ICMS.ICMS00.pICMS = produto.ClassificacaoFiscal.TipoTributacao.ICMS00_pICMS;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.ICMS40_Orig) == false)
                {
                    if (cfe.infCFe.det[i].imposto.ICMS == null)
                        cfe.infCFe.det[i].imposto.ICMS = new _ICMS();

                    cfe.infCFe.det[i].imposto.ICMS.ICMS40 = new _ICMS40();
                    cfe.infCFe.det[i].imposto.ICMS.ICMS40.Orig = produto.ClassificacaoFiscal.TipoTributacao.ICMS40_Orig;
                    cfe.infCFe.det[i].imposto.ICMS.ICMS40.CST = produto.ClassificacaoFiscal.TipoTributacao.ICMS40_CST;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.ICMSSN102_Orig) == false)
                {
                    if (cfe.infCFe.det[i].imposto.ICMS == null)
                        cfe.infCFe.det[i].imposto.ICMS = new _ICMS();

                    cfe.infCFe.det[i].imposto.ICMS.ICMSSN102 = new _ICMSSN102();
                    cfe.infCFe.det[i].imposto.ICMS.ICMSSN102.Orig = produto.ClassificacaoFiscal.TipoTributacao.ICMSSN102_Orig;
                    cfe.infCFe.det[i].imposto.ICMS.ICMSSN102.CSOSN = produto.ClassificacaoFiscal.TipoTributacao.ICMSSN102_CSOSN;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.ICMSSN900_Orig) == false)
                {
                    if (cfe.infCFe.det[i].imposto.ICMS == null)
                        cfe.infCFe.det[i].imposto.ICMS = new _ICMS();

                    cfe.infCFe.det[i].imposto.ICMS.ICMSSN900 = new _ICMSSN900();
                    cfe.infCFe.det[i].imposto.ICMS.ICMSSN900.Orig = produto.ClassificacaoFiscal.TipoTributacao.ICMSSN900_Orig;
                    cfe.infCFe.det[i].imposto.ICMS.ICMSSN900.CSOSN = produto.ClassificacaoFiscal.TipoTributacao.ICMSSN900_CSOSN;
                    cfe.infCFe.det[i].imposto.ICMS.ICMSSN900.pICMS = produto.ClassificacaoFiscal.TipoTributacao.ICMSSN900_pICMS;
                }

                cfe.infCFe.det[i].imposto.PIS = new _PIS();

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.PISAliq_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.PIS.PISAliq = new _PISAliq();
                    cfe.infCFe.det[i].imposto.PIS.PISAliq.CST = produto.ClassificacaoFiscal.TipoTributacao.PISAliq_CST;
                    cfe.infCFe.det[i].imposto.PIS.PISAliq.vBC = (listaProdutoAgrupado.ElementAt(i).Valor.Value * listaProdutoAgrupado.ElementAt(i).Quantidade.Value).ToString("0.00", cultureInfo);
                    cfe.infCFe.det[i].imposto.PIS.PISAliq.pPIS = produto.ClassificacaoFiscal.TipoTributacao.PISAliq_pPIS;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.PISQtde_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.PIS.PISQtde = new _PISQtde();
                    cfe.infCFe.det[i].imposto.PIS.PISQtde.CST = produto.ClassificacaoFiscal.TipoTributacao.PISQtde_CST;
                    cfe.infCFe.det[i].imposto.PIS.PISQtde.vAliqProd = produto.ClassificacaoFiscal.TipoTributacao.PISQtde_vAliqProd;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.PISNT_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.PIS.PISNT = new _PISNT();
                    cfe.infCFe.det[i].imposto.PIS.PISNT.CST = produto.ClassificacaoFiscal.TipoTributacao.PISNT_CST;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.PISSN_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.PIS.PISSN = new _PISSN();
                    cfe.infCFe.det[i].imposto.PIS.PISSN.CST = produto.ClassificacaoFiscal.TipoTributacao.PISSN_CST;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.PISOutr_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.PIS.PISOutr = new _PISOutr();
                    cfe.infCFe.det[i].imposto.PIS.PISOutr.CST = produto.ClassificacaoFiscal.TipoTributacao.PISOutr_CST;
                    cfe.infCFe.det[i].imposto.PIS.PISOutr.vBC = (listaProdutoAgrupado.ElementAt(i).Valor.Value * listaProdutoAgrupado.ElementAt(i).Quantidade.Value).ToString("0.00", cultureInfo);
                    cfe.infCFe.det[i].imposto.PIS.PISOutr.pPIS = produto.ClassificacaoFiscal.TipoTributacao.PISOutr_pPIS;
                    cfe.infCFe.det[i].imposto.PIS.PISOutr.vAliqProd = produto.ClassificacaoFiscal.TipoTributacao.PISOutr_vAliqProd;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.PISST_pPIS) == false)
                {
                    cfe.infCFe.det[i].imposto.PIS.PISST = new _PISST();
                    cfe.infCFe.det[i].imposto.PIS.PISST.pPIS = produto.ClassificacaoFiscal.TipoTributacao.PISST_pPIS;
                    cfe.infCFe.det[i].imposto.PIS.PISST.vBC = (listaProdutoAgrupado.ElementAt(i).Valor.Value * listaProdutoAgrupado.ElementAt(i).Quantidade.Value).ToString("0.00", cultureInfo);
                    cfe.infCFe.det[i].imposto.PIS.PISST.vAliqProd = produto.ClassificacaoFiscal.TipoTributacao.PISST_vAliqProd;
                }

                cfe.infCFe.det[i].imposto.COFINS = new _COFINS();
                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.COFINSAliq_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.COFINS.COFINSAliq = new _COFINSAliq();
                    cfe.infCFe.det[i].imposto.COFINS.COFINSAliq.CST = produto.ClassificacaoFiscal.TipoTributacao.COFINSAliq_CST;
                    cfe.infCFe.det[i].imposto.COFINS.COFINSAliq.vBC = (listaProdutoAgrupado.ElementAt(i).Valor.Value * listaProdutoAgrupado.ElementAt(i).Quantidade.Value).ToString("0.00", cultureInfo);
                    cfe.infCFe.det[i].imposto.COFINS.COFINSAliq.pCOFINS = produto.ClassificacaoFiscal.TipoTributacao.COFINSAliq_pCOFINS;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.COFINSQtde_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.COFINS.COFINSQtde = new _COFINSQtde();
                    cfe.infCFe.det[i].imposto.COFINS.COFINSQtde.CST = produto.ClassificacaoFiscal.TipoTributacao.COFINSQtde_CST;
                    cfe.infCFe.det[i].imposto.COFINS.COFINSQtde.vAliqProd = produto.ClassificacaoFiscal.TipoTributacao.COFINSQtde_vAliqProd;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.COFINSNT_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.COFINS.COFINSNT = new _COFINSNT();
                    cfe.infCFe.det[i].imposto.COFINS.COFINSNT.CST = produto.ClassificacaoFiscal.TipoTributacao.COFINSNT_CST;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.COFINSSN_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.COFINS.COFINSSN = new _COFINSSN();
                    cfe.infCFe.det[i].imposto.COFINS.COFINSSN.CST = produto.ClassificacaoFiscal.TipoTributacao.COFINSSN_CST;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.COFINSOutr_CST) == false)
                {
                    cfe.infCFe.det[i].imposto.COFINS.COFINSOutr = new _COFINSOutr();
                    cfe.infCFe.det[i].imposto.COFINS.COFINSOutr.CST = produto.ClassificacaoFiscal.TipoTributacao.COFINSOutr_CST;
                    cfe.infCFe.det[i].imposto.COFINS.COFINSOutr.vBC = listaProdutoAgrupado.ElementAt(i).Valor.Value.ToString("0.00", cultureInfo);
                    cfe.infCFe.det[i].imposto.COFINS.COFINSOutr.pCOFINS = produto.ClassificacaoFiscal.TipoTributacao.COFINSOutr_pCOFINS;
                    cfe.infCFe.det[i].imposto.COFINS.COFINSOutr.vAliqProd = produto.ClassificacaoFiscal.TipoTributacao.COFINSOutr_vAliqProd;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.COFINSST_pCOFINS) == false)
                {
                    cfe.infCFe.det[i].imposto.COFINS.COFINSST = new _COFINSST();
                    cfe.infCFe.det[i].imposto.COFINS.COFINSST.pCOFINS = produto.ClassificacaoFiscal.TipoTributacao.COFINSST_pCOFINS;
                    cfe.infCFe.det[i].imposto.COFINS.COFINSST.vBC = (listaProdutoAgrupado.ElementAt(i).Valor.Value * listaProdutoAgrupado.ElementAt(i).Quantidade.Value).ToString("0.00", cultureInfo);
                    cfe.infCFe.det[i].imposto.COFINS.COFINSST.vAliqProd = produto.ClassificacaoFiscal.TipoTributacao.COFINSST_vAliqProd;
                }

                if (string.IsNullOrEmpty(produto.ClassificacaoFiscal.TipoTributacao.ISSQN_vDeducISSQN) == false)
                {
                    cfe.infCFe.det[i].imposto.ISSQN = new _ISSQN();
                    cfe.infCFe.det[i].imposto.ISSQN.vDeducISSQN = produto.ClassificacaoFiscal.TipoTributacao.ISSQN_vDeducISSQN;
                    cfe.infCFe.det[i].imposto.ISSQN.vAliq = produto.ClassificacaoFiscal.TipoTributacao.ISSQN_vAliq;
                    cfe.infCFe.det[i].imposto.ISSQN.cListServ = produto.ClassificacaoFiscal.TipoTributacao.ISSQN_cListServ;
                    cfe.infCFe.det[i].imposto.ISSQN.cServTribMun = produto.ClassificacaoFiscal.TipoTributacao.ISSQN_cServTribMun;
                    cfe.infCFe.det[i].imposto.ISSQN.cNatOp = produto.ClassificacaoFiscal.TipoTributacao.ISSQN_cNatOp;
                    cfe.infCFe.det[i].imposto.ISSQN.indIncFisc = produto.ClassificacaoFiscal.TipoTributacao.ISSQN_indIncFisc;
                }
            }
            #endregion

            #region total
            cfe.infCFe.total = new _total();
            var descAcrEntr = new _DescAcrEntr();

            var valorDesconto = pedido.ValorDesconto.Value;

            var acrDesc = (valorServico + (pedido.ValorEntrega.HasValue ? pedido.ValorEntrega.Value : 0)) - valorDesconto;

            if (acrDesc > 0)
            {
                descAcrEntr.vAcresSubtot = acrDesc.ToString("0.00", cultureInfo);
                cfe.infCFe.total.DescAcrEntr = descAcrEntr;
            }
            else if (acrDesc < 0)
            {
                descAcrEntr.vDescSubtot = Math.Abs(acrDesc).ToString("0.00", cultureInfo);
                cfe.infCFe.total.DescAcrEntr = descAcrEntr;
            }
            #endregion

            #region pgto

            cfe.infCFe.pgto = new _pgto();
            var listapgto = PedidoPagamento.ListaSAT(pedido);
            cfe.infCFe.pgto.MP = new _MP[listapgto.Count];

            Int32 numeroParcela = 0;
            foreach (var item in listapgto)
            {
                cfe.infCFe.pgto.MP[numeroParcela] = new _MP
                {
                    cMP = item.Codigo,
                    vMP = item.Valor.ToString("0.00", cultureInfo)
                };
                numeroParcela++;
            }

            #endregion

            return cfe;
        }
    }
}
