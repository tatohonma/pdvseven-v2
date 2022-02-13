using a7D.PDV.EF.Models;
using System;
using System.Threading.Tasks;

namespace a7D.PDV.PainelMesaComanda.UI
{
    public class MesaComandasTotal
    {
        public string Mesa { get; set; }
        public int Comandas { get; set; }
        public decimal Total { get; set; }
    }

    public static class MesaComandas
    {
        static pdv7Context pdv = new pdv7Context();

        public static Task<MesaComandasTotal[]> LerMesas()
        {
            string sql = @"SELECT 
    p.ReferenciaLocalizacao Mesa,
    COUNT(DISTINCT co.Numero) Comandas,
	SUM(pp.ValorUnitario) Total
FROM
    tbPedido p(NOLOCK)
    INNER JOIN tbComanda co(NOLOCK) ON p.GUIDIdentificacao = co.GUIDIdentificacao
    INNER JOIN tbPedidoProduto pp(NOLOCK) ON p.IDPedido = pp.IDPedido
WHERE
    p.IDStatusPedido = 10
GROUP BY
    p.ReferenciaLocalizacao";

            return pdv.Database.SqlQuery<MesaComandasTotal>(sql).ToArrayAsync();
        }

        public class NumeroQuantidadeTotalPrimeiro
        {
            public int Numero { get; set; }
            public decimal Quantidade { get; set; }
            public decimal Total { get; set; }
            public DateTime Primeiro { get; set; }
        }

        public static Task<NumeroQuantidadeTotalPrimeiro[]> LerComandasPorMesa(string mesa)
        {
            string sql = @"SELECT 
	co.Numero,
	SUM(pp.Quantidade) Quantidade,
	SUM(pp.ValorUnitario) Total,
	MIN(p.DtPedido) Primeiro
FROM 
	tbPedido p (NOLOCK) 
	INNER JOIN tbComanda co (NOLOCK) ON p.GUIDIdentificacao=co.GUIDIdentificacao
	INNER JOIN tbPedidoProduto pp (NOLOCK) ON p.IDPedido=pp.IDPedido
WHERE
    p.IDStatusPedido=10 AND
	p.ReferenciaLocalizacao=@p0
GROUP BY 
	co.Numero";

            return pdv.Database.SqlQuery<NumeroQuantidadeTotalPrimeiro>(sql, mesa).ToArrayAsync();
        }
    }
}