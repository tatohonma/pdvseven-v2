using System;

namespace a7D.PDV.Integracao.EasyChopp.Model
{
    public class Cliente : Retorno
    {
        public int idCliente { get; set; }
        public string dsNomeCliente { get; set; }
        public string dsEmail { get; set; }
        public DateTime? dtNascimento { get; set; }
        public string dsSexo { get; set; }
        public string nrTelefone { get; set; }
        public string dsTipoDocumento { get; set; }
        public string nrDocumento { get; set; }
        public string idTAG { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtSituacao { get; set; }

        public override string ToString()
        {
            return stIntegracao ? $"OK {idCliente}: {nrDocumento} {idTAG} {dsNomeCliente} {vlSaldoAtual}" : dsError;
        }
    }

    public class ClienteList : Retorno
    {
        public Cliente[] Clientes { get; set; }
    }
}
