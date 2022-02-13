using a7D.PDV.BLL;
using a7D.PDV.Fiscal.Comunicacao.SAT;
using a7D.PDV.Model;
using System;

namespace a7D.PDV.SAT
{
    public static class RetornoSatFactory
    {
        public static RetornoSATInformation GerarRetornoVenda(string retStr, bool salvarRetorno = false)
        {
            RetornoSATInformation retornoSAT = new RetornoSATInformation();
            string[] arrRet = retStr.Split('|');

            // numeroSessao|EEEEE|CCCC|mensagem|cod|mensagemSEFAZ|arquivoCFeBase64|timeS tamp|chaveConsulta|valorTotalCFe|CPFCNPJValue|assinaturaQRCODE
            if (arrRet.Length <= 1)
                throw new Exception(retStr);

            retornoSAT.TipoSolicitacaoSAT = new TipoSolicitacaoSATInformation { IDTipoSolicitacaoSAT = 1 };
            retornoSAT.numeroSessao = arrRet[0];

            if (arrRet.Length > 1) retornoSAT.EEEEE = arrRet[1];
            if (arrRet.Length > 2) retornoSAT.CCCC = arrRet[2];
            if (arrRet.Length > 3) retornoSAT.mensagem = arrRet[3];
            if (arrRet.Length > 4) retornoSAT.cod = arrRet[4];
            if (arrRet.Length > 5) retornoSAT.mensagemSEFAZ = arrRet[5];

            if (retornoSAT.EEEEE == "06000")
            {
                //Emitido com sucesso
                retornoSAT.arquivoCFeSAT = arrRet[6];
                retornoSAT.timeStamp = arrRet[7];
                retornoSAT.chaveConsulta = arrRet[8];
                retornoSAT.valorTotalCFe = arrRet[9];
                retornoSAT.CPFCNPJValue = arrRet[10];
                retornoSAT.assinaturaQRCODE = arrRet[11];

                if (salvarRetorno)
                    RetornoSAT.Salvar(retornoSAT);
            }
            else
            {
                if (salvarRetorno)
                    RetornoSAT.Salvar(retornoSAT);

                string msgErro = "";

                if (!string.IsNullOrEmpty(retornoSAT.EEEEE))
                    msgErro += CodigosSAT.Descricao(retornoSAT.EEEEE) + "\n";

                if (!string.IsNullOrEmpty(retornoSAT.CCCC))
                    msgErro += CodigosSAT.Descricao(retornoSAT.CCCC) + "\n";

                if (!string.IsNullOrEmpty(retornoSAT.cod))
                    msgErro += CodigosSAT.Descricao(retornoSAT.cod) + "\n";

                msgErro += retornoSAT.mensagem;

                var ex = new ExceptionPDV(CodigoErro.E503, msgErro);
                ex.Data.Add("retStr", retStr);
                throw ex;
            }

            return retornoSAT;
        }

        public static RetornoSATInformation GerarRetornoCancelamento(string retStr)
        {
            RetornoSATInformation retornoSAT = new RetornoSATInformation();
            string[] arrRet = retStr.Split('|');

            retornoSAT.TipoSolicitacaoSAT = new TipoSolicitacaoSATInformation { IDTipoSolicitacaoSAT = 2 };

            for (int i = 0; i < arrRet.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        retornoSAT.numeroSessao = arrRet[0];
                        break;
                    case 1:
                        retornoSAT.EEEEE = arrRet[1];
                        break;
                    case 2:
                        retornoSAT.CCCC = arrRet[2];
                        break;
                    case 3:
                        retornoSAT.mensagem = arrRet[3];
                        break;
                    case 4:
                        retornoSAT.cod = arrRet[4];
                        break;
                    case 5:
                        retornoSAT.mensagemSEFAZ = arrRet[5];
                        break;
                    case 6:
                        retornoSAT.arquivoCFeSAT = arrRet[6];
                        break;
                    case 7:
                        retornoSAT.timeStamp = arrRet[7];
                        break;
                    case 8:
                        retornoSAT.chaveConsulta = arrRet[8];
                        break;
                    case 9:
                        retornoSAT.valorTotalCFe = arrRet[9];
                        break;
                    case 10:
                        retornoSAT.CPFCNPJValue = arrRet[10];
                        break;
                    case 11:
                        retornoSAT.assinaturaQRCODE = arrRet[11];
                        break;
                }
            }

            RetornoSAT.Salvar(retornoSAT);
            return retornoSAT;
        }
    }
}
