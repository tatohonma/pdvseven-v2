using a7D.PDV.Gateway.UIWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using a7D.PDV.Gateway.UIWeb.Context;

namespace a7D.PDV.Gateway.UIWeb.Repository
{
    public class ItemPedidoRepository : BaseRepository<ItemPedido>
    {
        public ItemPedidoRepository(GatewayContext _db) : base(_db) { }

        public List<ItemPedido> ListarPorPedido(int idPedido)
        {
            return _db.ItensPedido.Include("ContaReceber").Include("ContaReceber.Cliente").Where(ip => ip.IdPedido == idPedido).ToList();
        }
    }
}