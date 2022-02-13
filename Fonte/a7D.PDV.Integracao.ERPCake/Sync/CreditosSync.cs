using a7D.PDV.Integracao.ERPCake.Model;
using System;
using System.Linq;

namespace a7D.PDV.Integracao.ERPCake.Sync
{
    public static class CreditosSync
    {
        private static int idCategoriaDespesa;
        private static int idBancoContaCliente;

        public static void SincronizarCreditos(this ICakeBase cake)
        {
            if (idCategoriaDespesa == 0)
            {
                var listaCategorias = cake.api.All<CashFlow_Category>();
                var categoria = listaCategorias.FirstOrDefault(b => b.name == "Pedidos");
                if (categoria == null)
                {
                    categoria = new CashFlow_Category()
                    {
                        name = "Pedidos",
                    };
                    cake.AddLog($"Categoria '{categoria.name}' criada");
                    idCategoriaDespesa = cake.api.Save(categoria).id.Value;
                }
                else
                {
                    idCategoriaDespesa = categoria.id.Value;
                }
            }

            if (idBancoContaCliente == 0)
            {
                var listaBancos = cake.api.All<Bank>();
                var banco = listaBancos.FirstOrDefault(b => b.name == "Conta Cliente");
                if (banco == null)
                {
                    banco = new Bank()
                    {
                        name = "Conta Cliente",
                        base_bank = 26, // Outros Bancos
                        agency = 0,
                        account = 0,
                    };
                    cake.AddLog($"Banco '{banco.name}' criado");
                    idBancoContaCliente = cake.api.Save(banco).id.Value;
                }
                else
                {
                    idBancoContaCliente = banco.id.Value;
                }
            }

            AdicionaCreditosComoDebitos(cake);

            BaixaCreditosLiquidados(cake);
        }

        private static void AdicionaCreditosComoDebitos(ICakeBase cake)
        {
            // Busca todos os creditos que ainda não subiram
            var creditos = EF.Repositorio.ListarConfig<EF.Models.tbSaldo>(
                tb => tb.Include(nameof(EF.Models.tbSaldo.Cliente)),
                s => s.Tipo == "C" && s.CodigoERP == null);

            foreach (var credito in creditos)
            {
                if (!int.TryParse(credito.Cliente.CodigoERP, out int idCliente))
                    continue;

                var despesa = new CashFlow()
                {
                    registered_date = credito.dtMovimento,
                    DueDateConvert = credito.dtMovimento,
                    incoming = false,
                    amount = credito.Valor,
                    amount_total = credito.Valor,
                    discount = 0,
                    category = idCategoriaDespesa,
                    customer = idCliente,
                    description = "Crédito Pedido " + credito.IDPedido,
                    bank_account = idBancoContaCliente
                };

                if (credito.Liquidado != null)
                {
                    // Se já estiver liquidado sobe descontado
                    despesa.received = true;
                    despesa.DateReceivedConvert = credito.Liquidado;
                    despesa.discount = credito.Valor;
                    despesa.amount_total = 0;
                    cake.AddLog($"Crédito de Cliente #{credito.IDSaldo} já Liquidado, Pedido: {credito.Pedido} R$ {credito.Valor}");
                }
                else
                    cake.AddLog($"Novo Crédito de Cliente #{credito.IDSaldo}, Pedido: {credito.Pedido} R$ {credito.Valor}");

                credito.CodigoERP = cake.api.Save(despesa).id.ToString();
                EF.Repositorio.Atualizar(credito);
            }
        }

        private static void BaixaCreditosLiquidados(ICakeBase cake)
        {
            // Busca todos os creditos que já subiram
            var creditos = EF.Repositorio.ListarConfig<EF.Models.tbSaldo>(
                tb => tb.Include(nameof(EF.Models.tbSaldo.Cliente)),
                s => s.Tipo == "C" && s.CodigoERP != null && !s.CodigoERP.StartsWith("-"));

            foreach (var credito in creditos)
            {
                try
                {
                    int id = int.Parse(credito.CodigoERP);
                    var despesa = cake.api.GetByID<CashFlow>(id);

                    despesa.received = true;
                    despesa.DateReceivedConvert = credito.Liquidado;
                    despesa.amount = despesa.discount = credito.Valor;
                    despesa.amount_total = 0;
                    cake.api.Save(despesa);

                    credito.CodigoERP = (-id).ToString(); // Numeros negativos é a baixa no cake
                    EF.Repositorio.Atualizar(credito);
                    cake.AddLog($"Baixa Crédito de Cliente #{credito.IDSaldo} R$ {credito.Valor}");
                }
                catch(Exception ex)
                {
                    ex = new Exception($"Erro Crédito de Cliente #{credito.IDSaldo} " + ex.Message, ex);
                    cake.AddLog(ex);
                }
            }
        }
    }
}
