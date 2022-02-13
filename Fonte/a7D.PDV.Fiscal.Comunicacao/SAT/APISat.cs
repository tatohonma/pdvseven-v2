using System;
using System.Runtime.InteropServices;

namespace a7D.PDV.Fiscal.Comunicacao.SAT
{
    public class APISat
    {
        [DllImport("sat.dll")]
        public static extern IntPtr EnviarDadosVenda(int numeroSessao, string codigoDeAtivacao, string dadosVenda);

        [DllImport("sat.dll")]
        public static extern IntPtr ConsultarSAT(int numeroSessao);

        [DllImport("sat.dll")]
        public static extern IntPtr CancelarUltimaVenda(int numeroSessao, string codigoDeAtivacao, string chave, string dadosCancelamento);

        [DllImport("sat.dll")]
        public static extern IntPtr AssociarAssinatura(int numeroSessao, string codigoDeAtivacao, string CNPJvalue, string assinaturaCNPJs);

        [DllImport("sat.dll")]
        public static extern IntPtr TesteFimAFim(int numeroSessao, string codigoDeAtivacao, string dadosVenda);

        [DllImport("sat.dll")]
        public static extern IntPtr AtivarSAT(int numeroSessao, int subComando, string codigoDeAtivacao, string CNPJ, int cUF);

        [DllImport("sat.dll")]
        public static extern IntPtr ExtrairLogs(int numeroSessao, string codigoDeAtivacao);

        [DllImport("sat.dll")]
        public static extern IntPtr BloquearSAT(int numeroSessao, string codigoDeAtivacao);

        [DllImport("sat.dll")]
        public static extern IntPtr DesbloquearSAT(int numeroSessao, string codigoDeAtivacao);

        [DllImport("sat.dll")]
        public static extern IntPtr ConsultarStatusOperacional(int numeroSessao, string codigoDeAtivacao);

        [DllImport("sat.dll")]
        public static extern IntPtr ConsultarNumeroSessao(int numeroSessao, string codigoDeAtivacao, int cNumeroDeSessao);
    }
}