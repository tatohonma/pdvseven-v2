using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;

namespace a7D.PDV.Fiscal.Comunicacao.SAT
{
    public class ComunicacaoSat
    {
        [HandleProcessCorruptedStateExceptions]
        public static string RetornoOuErro(Func<IntPtr> func)
        {
            string resposta = null;
            try
            {
                var retPtr = func.Invoke();
                if (retPtr == IntPtr.Zero)
                    return null;

                resposta = Marshal.PtrToStringAnsi(retPtr);
                resposta = Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(resposta));

                Marshal.Release(retPtr);
            }
            catch (AccessViolationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resposta;
        }

        public static string ConsultarSat(int numeroSessao)
        => RetornoOuErro(() => APISat.ConsultarSAT(numeroSessao));

        //public static string TesteFimAFimSat(int numeroSessao, string codigoDeAtivacao, string versaoXml)
        //=> RetornoOuErro(() => APISat.TesteFimAFim(numeroSessao, codigoDeAtivacao, ObterXMLExemplo(versaoXml).XMLEXEMPLO()));

        public static string ExtrairLogsSat(int numeroSessao, string codigoDeAtivacao)
        => RetornoOuErro(() => APISat.ExtrairLogs(numeroSessao, codigoDeAtivacao));

        public static string EnviarCancelamento(string codigoDeAtivacao, int numeroSessao, string chave, string dadosCancelamento)
        => RetornoOuErro(() => APISat.CancelarUltimaVenda(numeroSessao, codigoDeAtivacao, chave, dadosCancelamento));

        public static string ConsultarSessao(string codigoDeAtivacao, int cNumeroSessao)
        => RetornoOuErro(() => APISat.ConsultarNumeroSessao(new Random().Next(1, 999999), codigoDeAtivacao, cNumeroSessao));

        public static string EnviarVenda(string codigoDeAtivacao, string dadosVenda, int numeroSessao)
        {
            var retStr = RetornoOuErro(() => APISat.EnviarDadosVenda(numeroSessao, codigoDeAtivacao, dadosVenda));

            if (string.IsNullOrWhiteSpace(retStr))
                throw new Exception("Retorno do SAT vazio");

            return retStr;
        }

        //private static ICFe ObterXMLExemplo(string versaoXml)
        //{
        //    if (versaoXml == "0.07")
        //        return new _007.Venda.CFe();
        //    else if (versaoXml == "0.08")
        //        return new _008.Venda.CFe();
        //    else
        //        return null;
        //}
    }
}
