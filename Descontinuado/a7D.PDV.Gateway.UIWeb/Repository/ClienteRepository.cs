using a7D.PDV.Gateway.UIWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using a7D.PDV.Gateway.UIWeb.Context;

namespace a7D.PDV.Gateway.UIWeb.Repository
{
    public class ClienteRepository : BaseRepository<Cliente>
    {
        public ClienteRepository(GatewayContext _db) : base(_db) { }

        public Cliente EncontrarOuInserir(string idCliente, string nomeCliente)
        {
            if (string.IsNullOrWhiteSpace(idCliente) || string.IsNullOrWhiteSpace(nomeCliente))
                return null;

            var cliente = Buscar(c => c.IdBroker == idCliente).FirstOrDefault();

            if (cliente != null)
                return cliente;

            cliente = new Cliente
            {
                IdBroker = idCliente,
                Nome = nomeCliente
            };

            return Adicionar(cliente);
        }

        public Cliente BuscarPorIdBroker(string idBroker)
        {
            var cliente = Buscar(c => c.IdBroker == idBroker).FirstOrDefault();
            return cliente;
        }
    }
}