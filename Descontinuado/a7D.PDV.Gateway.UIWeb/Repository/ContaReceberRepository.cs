using a7D.PDV.Gateway.UIWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using a7D.PDV.Gateway.UIWeb.Context;

namespace a7D.PDV.Gateway.UIWeb.Repository
{
    public class ContaReceberRepository : BaseRepository<ContaReceber>
    {
        public ContaReceberRepository(GatewayContext _db) : base(_db) { }

        public ContaReceber ObterPorIdBroker(string idBroker)
        {
            return Buscar(cr => cr.IdBroker == idBroker).FirstOrDefault();
        }
    }
}