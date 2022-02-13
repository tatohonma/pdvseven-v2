using a7D.PDV.Gateway.UIWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using a7D.PDV.Gateway.UIWeb.Context;

namespace a7D.PDV.Gateway.UIWeb.Repository
{
    public class PedidoRepository : BaseRepository<Pedido>
    {
        public PedidoRepository(GatewayContext _db) : base(_db)
        {
        }

        public Pedido BuscarPedido(IList<ItemPedido> itens)
        {
            var pedidos = Buscar(p =>
               p.Status == "pending").ToList();
            pedidos = pedidos.Where(p => p.Itens.Count == itens.Count).ToList();
            pedidos = pedidos.Where(p =>
                p.Itens.All(item => itens.FirstOrDefault(i => i.IdContaReceber == item.IdContaReceber) != null)
            ).ToList();
            try { return pedidos.First(); }
            catch { }
            return null;
        }

        public List<Pedido> BuscarPorIdBroker(string idBroker = "")
        {
            var q = _db.ItensPedido
                    .Where(ip => ip.ContaReceber.Pendente == true);
            if (string.IsNullOrWhiteSpace(idBroker) == false)
            {
                q = q.Where(ip => ip.ContaReceber.Cliente.IdBroker == idBroker);
            }
            
            var pedidos = q.Select(ip => ip.Pedido).ToList();
            var idPedidos = pedidos.Distinct(new PedidoComparer()).Select(p => p.IdPedido).ToList();
            return Buscar(p => idPedidos.Contains(p.IdPedido)).ToList();
        }

        public IList<Pedido> BuscarPorCliente(Cliente cliente)
        {
            return BuscarPorCliente(cliente.IdCliente);
        }

        public IList<Pedido> BuscarPorCliente(int idCliente)
        {
            var pedidos = _db.ItensPedido
                    .Where(ip => ip.ContaReceber.Cliente.IdCliente == idCliente)
                    .Select(ip => ip.Pedido)
                    .ToList();
            return pedidos.Distinct(new PedidoComparer()).ToList();
        }

        class PedidoComparer : IEqualityComparer<Pedido>
        {
            public bool Equals(Pedido x, Pedido y)
            {
                return x.IdPedido == y.IdPedido;
            }

            public int GetHashCode(Pedido obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}