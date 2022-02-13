using a7D.PDV.BLL.ValueObject;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.BLL
{
    public class Saldo
    {

        /*
select * from tbsaldo;
select * from tbPedido;

select c.idSaldo, c.idCliente, c.dtMovimento, c.valor, 
	(select sum(valor) from tbsaldo d where d.tipo='D' AND d.IDPai=c.idSaldo) saldo
from tbsaldo c where c.tipo='C';
         */

        public static decimal ClienteSaldoBruto(int idCliente)
        {
            using (var pdv = new pdv7Context())
            {
                return pdv.Database.SqlQuery<decimal?>("SELECT SUM(CASE Tipo WHEN 'C' THEN valor ELSE -valor END) FROM tbsaldo WHERE iDCliente=" + idCliente).FirstOrDefault() ?? 0;
            }
        }

        public static decimal ClienteTotalEmAberto(int idCliente, int? idPedidoIgnorar)
        {
            // TODO: Itens com modificações não preve que o produto PAI tenha mais de 1 unidade
            using (var pdv = new pdv7Context())
            {
                string sql = @"SELECT SUM(pp.Quantidade*pp.ValorUnitario)
FROM tbPedidoProduto pp
INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido
WHERE p.IDStatusPedido=10 AND pp.Cancelado=0 AND p.IDCliente=" + idCliente;

                if (idPedidoIgnorar.HasValue)
                    sql += " AND p.IDPedido<>" + idPedidoIgnorar;

                return pdv.Database.SqlQuery<decimal?>(sql).FirstOrDefault() ?? 0;
            }
        }

        public static decimal ClienteSaldoLiquido(int idCliente, int? idPedidoIgnorar = null)
        {
            return ClienteSaldoBruto(idCliente) - ClienteTotalEmAberto(idCliente, idPedidoIgnorar);
        }

        public static bool AdicionaPagamentoPorSaldo(PedidoInformation pedido)
        {
            if (pedido.Cliente == null
             || pedido.ListaPagamento.Any()
             || pedido.ListaProduto.Any(pp => pp.Produto.TipoProduto.TipoProduto == ETipoProduto.Credito))
                return false;

            var total = pedido.ValorTotalTemp;
            if (total == 0)
                return true;

            var tipoPagamento = TipoPagamento.CarregarPorGateway((int)EGateway.ContaCliente, true);
            if (tipoPagamento == null)
                return false;

            var saldo = ClienteSaldoLiquido(pedido.Cliente.IDCliente.Value, pedido.IDPedido);
            decimal final = saldo - total;
            //if (final < 0)
            //{
            //    //return false;
            //    MessageBox.Show($"ATENÇÃO!\r\nO saldo do cliente será de {final.ToString("C")}!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

            var pagamento = new PedidoPagamentoInformation()
            {
                TipoPagamento = tipoPagamento,
                Autorizacao = "Saldo: " + saldo + " Final: " + final,
                Bandeira = tipoPagamento.Bandeira,
                ContaRecebivel = tipoPagamento.ContaRecebivel,
                MeioPagamentoSAT = tipoPagamento.MeioPagamentoSAT,
                IDGateway = (int)EGateway.ContaCliente,
                Status = StatusModel.Novo,
                Pedido = pedido,
                Valor = total,
                Excluido = false
            };

            pedido.ListaPagamento.Add(pagamento);
            return true;
        }

        /// <summary>
        /// Obtem o extrato de um cliente como referencia a tabela de movimento de saldos
        /// </summary>
        /// <remarks>
        /// Veja o teste unitário: Comanda_Consulta_Saldo()
        /// </remarks>
        public static ExtratoCreditos[] ExtratoCreditos(int idCliente)
        {
            using (var pdv = new pdv7Context())
            {
                string sql = $@"SELECT
    s.IDSaldo, 
    s.dtMovimento Data, 
    ped.IDTipoPedido,
    s.Valor, 
    s.Tipo, 
    s.IDPedido, 
    ped.GUIDAgrupamentoPedido Agrupamento, 
    c.Numero Comanda,
    m.Numero Mesa
FROM tbsaldo s
INNER JOIN tbPedido ped ON ped.IDPedido=s.IDPedido
LEFT JOIN tbComanda c ON c.GUIDIdentificacao=ped.GUIDIdentificacao
LEFT JOIN tbMesa m ON m.GUIDIdentificacao=ped.GUIDIdentificacao
WHERE s.IDCliente={idCliente} 

UNION ALL 

SELECT
    999999 IDSaldo, 
    ped.DtPedido Data, 
    ped.IDTipoPedido,
    sum(pp.Quantidade*pp.ValorUnitario) Valor, 
    'D' Tipo, 
    ped.IDPedido, 
    ped.GUIDAgrupamentoPedido Agrupamento, 
    c.Numero Comanda,
    m.Numero Mesa
FROM tbPedidoProduto pp
INNER JOIN tbPedido ped ON ped.IDPedido=pp.IDPedido
LEFT JOIN tbComanda c ON c.GUIDIdentificacao=ped.GUIDIdentificacao
LEFT JOIN tbMesa m ON m.GUIDIdentificacao=ped.GUIDIdentificacao
WHERE ped.IDCliente={idCliente} AND ped.IDStatusPedido=10 AND pp.Cancelado=0
GROUP BY 
    ped.DtPedido, 
    ped.IDTipoPedido,
    ped.IDPedido, 
    ped.GUIDAgrupamentoPedido, 
    c.Numero,
    m.Numero

ORDER BY 1
";
                var extrato = pdv.Database.SqlQuery<ExtratoCreditos>(sql).ToArray();

                // Processa o saldo individualmente por pedido!
                // ATENÇÃO: Os ID do saldo se repete pois os debitos podem ser debitados parcialmente em cada pedido
                decimal saldo = 0m;
                foreach (var item in extrato)
                {
                    if (item.IDTipoPedido == (int)ETipoPedido.Mesa)
                        item.Origem = $"Mesa {item.Mesa}";
                    else if (item.IDTipoPedido == (int)ETipoPedido.Comanda)
                        item.Origem = $"Comanda {item.Comanda}";
                    else if (item.IDTipoPedido == (int)ETipoPedido.Balcao)
                        item.Origem = $"Balcão";
                    else if (item.IDTipoPedido == (int)ETipoPedido.Delivery)
                        item.Origem = $"Delivery";

                    if (item.Tipo == "C")
                        saldo += item.Valor;
                    else
                        saldo -= item.Valor;

                    item.Saldo += saldo;
                }
                return extrato;
            }
        }

        /// <summary>
        /// Obtem o extrato de um cliente como referencia os itens do pedidos existentes na tabela de saldos
        /// </summary>
        /// <remarks>
        /// Veja o teste unitário: Comanda_Consulta_Saldo()
        /// </remarks>
        public static ExtratoItens[] ExtratoItens(int idCliente) //, string agrupamento = null)
        {
            using (var pdv = new pdv7Context())
            {
                string sql = $@"SELECT 
    ped.IDPedido, 
    pp.DtInclusao Data, 
    CASE p.IDTipoProduto 
		WHEN 50 THEN 'Crédito' 
		WHEN 30 THEN 'Serviço' 
		ELSE 'Débito' END Tipo, 
    p.Nome Item, 
    pp.Quantidade*pp.ValorUnitario Valor
FROM tbPedido ped 
INNER JOIN tbPedidoProduto pp ON pp.IDPedido=ped.IDPedido
INNER JOIN tbProduto p ON p.IDProduto=pp.IDProduto
WHERE ped.IDCliente={idCliente} 
AND pp.Cancelado=0
AND (ped.IDStatusPedido=10 
  OR ped.IDPedido IN (SELECT IDPedido FROM tbSaldo WHERE IDCliente={idCliente}))

UNION

SELECT 
    ped.IDPedido, 
    pp.DataPagamento Data, 
	CASE pp.IDMetodo 
		WHEN 15 THEN 'Débito'
		ELSE 'Pagamento' END Tipo, 
        '( '+ ISNULL(g.Nome, tp.Nome) +' )' Item, 
    pp.Valor
FROM tbPedido ped 
INNER JOIN tbPedidoPagamento pp ON pp.IDPedido=ped.IDPedido
INNER JOIN tbTipoPagamento tp ON tp.IDTipoPagamento=pp.IDTipoPagamento
LEFT JOIN tbGateway g ON g.IDGateway=pp.IDGateway
WHERE pp.Excluido=0
AND ped.IDPedido IN (SELECT IDPedido FROM tbSaldo WHERE IDCliente={idCliente})

ORDER BY IDPedido, Data;
";
                // A compra de credito é sempre do cliente, mas o consumo pode ser um pagamento de outro
                // É possivel ver o pagamento, mas não o que o outro cliente consumiu
                return pdv.Database.SqlQuery<ExtratoItens>(sql).ToArray();
            }
        }

        public static SaldoClientes[] BuscaClientes(string busca) //, string agrupamento = null)
        {
            using (var pdv = new pdv7Context())
            {
                string sql = @"select 
	 c.IDCliente
	,c.NomeCompleto
	,CONCAT('(', c.Telefone1DDD, ') ', c.Telefone1Numero) Telefone
	,c.Documento1 Documento
	,s.Saldo-ISNULL(t.Pendente,0) Saldo
FROM tbCliente c
INNER JOIN (SELECT IDCliente, SUM(CASE Tipo WHEN 'C' THEN Valor ELSE -Valor END) Saldo FROM tbSaldo GROUP BY IDCliente) s ON s.IDCliente=c.IDCliente
LEFT JOIN (select p.IDCliente, SUM(pp.Quantidade*pp.ValorUnitario) Pendente FROM tbPedidoProduto pp INNER JOIN tbPedido p ON p.IDPedido=pp.IDPedido WHERE pp.Cancelado=0 AND p.IDStatusPedido=10 GROUP BY p.IDCliente) t ON t.IDCliente=c.IDCliente
WHERE 
c.Documento1 like @busca 
OR c.NomeCompleto like @busca 
OR CONCAT(c.Telefone1DDD, c.Telefone1Numero) LIKE @busca
ORDER BY c.NomeCompleto";
                return pdv.Database.SqlQuery<SaldoClientes>(sql, new SqlParameter("@busca", "%" + busca + "%")).ToArray();
            }
        }

        public static bool AdicionarCreditoSeExistir(PedidoInformation pedido)
        {
            var creditos = pedido.ListaProduto.Where(p => p.Produto.TipoProduto.TipoProduto == ETipoProduto.Credito);
            if (creditos.Count() == 0)
                return false;

            if (pedido.Cliente == null || pedido.Cliente.IDCliente == 0)
                throw new ExceptionPDV(CodigoErro.AE32);

            decimal totalCreditos = creditos.Sum(p => p.ValorTotal);
            var movimento = new tbSaldo()
            {
                dtMovimento = DateTime.Now,
                IDPedido = pedido.IDPedido.Value,
                IDCliente = pedido.Cliente.IDCliente.Value,
                Tipo = "C",
                Valor = totalCreditos
            };
            Repositorio.Inserir(movimento);
            return true;
            //movimento.IDPai = movimento.IDSaldo;
            //Repositorio.Atualizar(movimento);
        }

        public static int RegistrarDebitoComoPagamento(PedidoPagamentoInformation pagamento)
        {
            return RegistrarDebito(pagamento.Pedido.IDPedido.Value, pagamento.Pedido.Cliente.IDCliente.Value, pagamento.Valor.Value, pagamento.IDPedidoPagamento.Value);
        }

        private static int RegistrarDebito(int idPedido, int idCliente, decimal total, int idPedidoPagamento)
        {
            using (var pdv = new pdv7Context())
            {
                var movimento = new tbSaldo()
                {
                    dtMovimento = DateTime.Now,
                    IDPedido = idPedido,
                    IDCliente = idCliente,
                    Valor = total,
                    IDPedidoPagamento = idPedidoPagamento,
                    Tipo = "D",
                };
                Repositorio.Inserir(movimento);
                return movimento.IDSaldo;
            }
        }

        public static void LiquidarDebitos(int idCliente)
        {
            bool buscaEgrava = true;
            var dtLimite = DateTime.Now.Date.AddDays(-ConfiguracoesSistema.Valores.ValidadeCreditos);
            var dtExpiracao = DateTime.Now;
            var sb = new StringBuilder();
            bool alterado = false;
            sb.AppendLine($"Cliente: {idCliente}");
            while (buscaEgrava)
            {
                buscaEgrava = false; // Evita loop invinito, e por padrão não salva liquidação incompletas!

                using (var pdv7 = new pdv7Context())
                {
                    var creditosNaoLiquidados = pdv7.Saldos
                        .Where(s => s.IDCliente == idCliente && s.Tipo == "C" && s.Liquidado == null)
                        .OrderBy(s => s.dtMovimento) // Teoricamente já deve vir em ordem!
                        .ToArray(); // Trabalho desconectado

                    var debitosALiquidar = pdv7.Saldos
                        .Where(s => s.IDCliente == idCliente && s.Tipo == "D" && s.IDPai <= 0)
                        .OrderBy(s => s.dtMovimento) // Por causa das divisões de debitos tem que ser sempre em ordem
                        .ToArray(); // Trabalho desconectado

                    // Sem creditos ou sem debitos?
                    if (!creditosNaoLiquidados.Any()) // || !debitosALiquidar.Any())
                        break;

                    // Só liquida o credito completo
                    // atualizando todos os debitos e creditos ao mesmo tempo
                    var credito = creditosNaoLiquidados.First();
                    var dtLiquidado = DateTime.Now;
                    var saldo = credito.Valor;

                    sb.AppendLine($"{credito.IDSaldo}: {credito.dtMovimento} R$ +{credito.Valor}");
                    foreach (var debito in debitosALiquidar)
                    {
                        if (debito.Valor <= saldo)
                        {
                            sb.AppendLine($"\t{debito.IDSaldo}: {debito.dtMovimento} R$ -{debito.Valor}");
                            saldo -= debito.Valor;
                            debito.IDPai = credito.IDSaldo;
                            debito.Liquidado = dtLiquidado;

                            // Caso ideal, facil debitos==creditos
                            if (saldo == 0)
                            {
                                credito.Liquidado = dtLiquidado;
                                pdv7.Saldos.Attach(credito);
                                pdv7.Entry(credito).State = EntityState.Modified;

                                // Precisa reiniciar tudo!
                                buscaEgrava = true;
                                break;
                            }

                            // continua buscando debitos
                        }
                        else // por eliminatórioa: debito.Valor > saldo
                        {
                            // Precisa criar um novo debito com a diferença (Clona e divide o debito)
                            var debito2 = new tbSaldo()
                            {
                                dtMovimento = debito.dtMovimento,
                                Valor = debito.Valor - saldo,
                                Tipo = "D",
                                IDPai = -debito.IDSaldo, // Isso será pago comalgum outro credito
                                IDPedido = debito.IDPedido,
                                IDPedidoPagamento = debito.IDPedidoPagamento,
                                IDCliente = debito.IDCliente
                            };

                            // Precisa reiniciar tudo!
                            pdv7.Saldos.Add(debito2);
                            buscaEgrava = true;

                            sb.AppendLine($"\t{debito.IDSaldo}: {debito.dtMovimento} R$ -{debito.Valor} => -{saldo}");

                            // Altera o atual só com a parte a ser debitada
                            debito.IDPai = credito.IDSaldo;
                            debito.Liquidado = dtLiquidado;
                            debito.Valor = saldo;
                            saldo = 0;

                            sb.AppendLine($"\tNOVO: {debito2.dtMovimento} R$ -{debito2.Valor}");

                            // Acabou o credito e este poderá ser liquidado!
                            credito.Liquidado = DateTime.Now;
                            pdv7.Saldos.Attach(credito);
                            pdv7.Entry(credito).State = EntityState.Modified;
                            break;
                        }
                    }

                    // Credit Expirado?
                    if (saldo > 0 && credito.dtMovimento < dtLimite)
                    {
                        var debito3 = new tbSaldo()
                        {
                            dtMovimento = dtExpiracao,
                            Valor = saldo,
                            Tipo = "D",
                            IDPai = credito.IDSaldo, // EXPIRADO!
                            IDPedido = credito.IDPedido,
                            IDPedidoPagamento = null,
                            IDCliente = credito.IDCliente
                        };

                        // Precisa reiniciar tudo!
                        pdv7.Saldos.Add(debito3);

                        sb.AppendLine($"\tEXPIRADO: {debito3.dtMovimento} R$ -{debito3.Valor}");

                        credito.Liquidado = DateTime.Now;
                        pdv7.Saldos.Attach(credito);
                        pdv7.Entry(credito).State = EntityState.Modified;
                        buscaEgrava = true;
                    }

                    // Se acabou os debitos, não faz nada, não grava as alterações liquidando nada!
                    if (buscaEgrava)
                    {
                        // Grava as alterações antes da nova busca
                        pdv7.SaveChanges();
                        alterado = true;
                    }
                }
            }

            if (alterado)
            {
                // Primeiro grava o Log!
                Logs.Info(CodigoInfo.I013, sb.ToString(), "Movimento de Saldo Cliente " + idCliente);

                // Dá baixa nos pagamentos com debitos liquidados para que no SYNC do CAKE possa faturar os pedidos em debito
                var sql = @"UPDATE tbPedidoPagamento SET IDSaldoBaixa=s.IDSaldo
FROM tbPedidoPagamento pp
INNER JOIN tbSaldo s ON s.IDPedidoPagamento=pp.IDPedidoPagamento
WHERE pp.IDSaldoBaixa IS NULL AND NOT s.Liquidado IS NULL";
                Repositorio.Execute(sql);
            }
        }

        public static void ExpirarCreditos()
        {
            var clientes = EF.Repositorio.Query<int>("SELECT DISTINCT idCliente FROM tbSaldo WHERE Tipo='C' AND Liquidado IS NULL");
            foreach (var id in clientes)
                LiquidarDebitos(id);
        }
    }
}