using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Ativacao.API.Services
{
    public static class ClientesService
    {
        public static DateTime dtStart = DateTime.Now;
        public static List<ClienteInfo> clientes = new List<ClienteInfo>();

        public static void Registra(string chave, string versao, string status, bool erro)
        {
            try
            {
                var cliente = clientes.FirstOrDefault(c => c.chave == chave);
                if (cliente == null)
                    clientes.Add(new ClienteInfo(chave, versao, status, erro));
                else
                    cliente.Update(versao, status, erro);
            }
            catch (Exception)
            {
            }
        }
    }

    public class ClienteInfo
    {
        public string chave { get; private set; }
        public DateTime data { get; private set; }
        public string versao { get; private set; }
        public string status { get; private set; }
        public int erros { get; private set; }

        public ClienteInfo(string chave, string versao, string status, bool erro)
        {
            this.data = DateTime.Now;
            this.chave = chave;
            this.versao = versao;
            this.status = status;
            this.erros = erro ? 1 : 0;
        }

        public void ClearErro()
        {
            this.erros = 0;
        }

        internal void Update(string versao, string status, bool erro)
        {
            this.data = DateTime.Now;
            this.versao = versao;
            this.status = status;
            if (erro)
                this.erros++;
        }
    }

}