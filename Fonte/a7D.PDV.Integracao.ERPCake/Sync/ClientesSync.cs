using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.ERPCake.Model;
using System.Linq;

namespace a7D.PDV.Integracao.ERPCake.Sync
{
    internal static class ClientesSync
    {
        internal static bool SincronizarClientes(this IntegraERPCake cake)
        {
            // Subir os clientes
            cake.AddLog("Buscando clientes PDV");
            var clientesPDV = EF.Repositorio.Listar<tbCliente>();

            cake.AddLog("Buscando clientes ERP");
            var clientesERP = cake.api.All<Customer>(); // TODO: paginar generico...
            var clientePadrao = clientesERP.FirstOrDefault(c => c.code == "PDV7");
            if (clientePadrao == null)
            {
                clientePadrao = new Customer()
                {
                    name = "(SEM CLIENTE)",
                    code = "PDV7",
                    observation = "Este cliente será vinculado a todos os pedidos quando não for informado um cliente"
                };
                clientePadrao = cake.api.Save(clientePadrao);
                if (clientePadrao == null)
                {
                    cake.AddLog("Erro ao cadastrar o cliente padrão, para pedidos sem cliente");
                    return false;
                }
            }
            //DTO.DefaultCustomer = clientePadrao.id;

            int n = 0;
            foreach (var clientePDV in clientesPDV)
            {
                n++;

                if (!string.IsNullOrEmpty(clientePDV.CodigoERP) && clientePDV.DtInclusao < cake.UltimoSync)
                    continue;

                string info = "";
                if (clientesPDV.Count > 10) // mostra percentual quando a lista tem mais de 10 itens
                    info = $"{(100 * n / (double)clientesPDV.Count).ToString("N1")}% ";

                var clienteERP = clientesERP.FirstOrDefault(p => p.id.ToString() == clientePDV.CodigoERP);

                //clientePDV.Fill(ref clienteERP);
                var result = cake.api.Save(clienteERP);
                if (result != null)
                {
                    info += "OK ";
                    if (clientePDV.CodigoERP != result.id.ToString())
                    {
                        clientePDV.CodigoERP = result.id.ToString();
                        EF.Repositorio.Atualizar(clientePDV);
                    }
                }
                else
                    info += "ERRO ";

                cake.AddLog($"{info} {clientePDV.IDCliente} ERP {clientePDV.CodigoERP} - {clientePDV.NomeCompleto}");
            }

            return true;
        }
    }
}
