using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.Integracao.iFood
{
    public partial class IntegraIFood
    {
        private void EnviarPrecosDisponibilidade()
        {
            if (config.AtualizaValor == "NAO" && !config.AtuaizarDisponibilidade)
                return;

            var produtos = Produto.ListarPorData(UltimaAlteracao, true, true);
            if (produtos.Count == 0)
            {
                //AddLog("Não há produtos a ser atualizado");
                return;
            }

            if (config.AtualizaValor.StartsWith("V") && config.AtuaizarDisponibilidade)
                AddLog("Atualizando preços e diponibilidade");
            else if (config.AtuaizarDisponibilidade)
                AddLog("Atualizando diponibilidade");

            UltimaAlteracao = DateTime.Now;
            foreach (var prod in produtos)
            {
                if (prod.TipoProduto.TipoProduto != ETipoProduto.Item
                 && prod.TipoProduto.TipoProduto != ETipoProduto.Modificacao)
                    continue;

                string info = $"{prod.IDProduto}: {prod.Nome} => ";
                try
                {
                    decimal valor = 0;
                    if (config.AtualizaValor.StartsWith("V"))
                    {
                        if (config.AtualizaValor == "V3" && prod.ValorUnitario3 > 0)
                        {
                            valor = prod.ValorUnitario3.Value;
                            info += "Valor 3: " + valor.ToString("C");
                        }
                        else if (config.AtualizaValor == "V2" && prod.ValorUnitario2 > 0)
                        {
                            valor = prod.ValorUnitario2.Value;
                            info += "Valor 2: " + valor.ToString("C");
                        }
                        else
                        {
                            if (config.AtualizaValor == "V2")
                                info += "V2(Vazio) ";
                            else if (config.AtualizaValor == "V3")
                                info += "V3(Vazio) ";

                            valor = prod.ValorUnitario.Value;
                            info += "Valor 1: " + valor.ToString("C");
                        }

                        var p = new ProdutoPrecoIFood()
                        {
                            merchantIds = new int[] { int.Parse(config.merchant_id) },
                            externalCode = prod.IDProduto.ToString(),
                            price = valor,
                            startDate = DateTime.Now.AddMinutes(1)
                        };

                        ifoodAPI.ProdutoPreco(p);
                    }

                    if (config.AtuaizarDisponibilidade)
                    {
                        info += $"{(prod.Disponibilidade == true ? " Disponível" : " Indíponivel")}";
                        ifoodAPI.ProdutoDisponibilidade(config.merchant_id, prod.IDProduto.ToString(), prod.Disponibilidade == true);
                    }

                    AddLog(info);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("token expired") && !ex.Message.Contains("Invalid access"))
                        throw new ExceptionPDV(CodigoErro.EE11, ex);

                    info += $" !!! Erro ao atualizar\r\n" + ex.Message;
                    AddLog(new Exception(info, ex));
                }
            }
            UltimaVerificacao = DateTime.Now;
        }

    }
}
