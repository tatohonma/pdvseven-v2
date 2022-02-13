using MuxxLib;
using MuxxLib.Estruturas;
using MuxxLib.Interface;
using MuxxLib.Operações;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace a7D.PDV.Integracao.Pagamento.NTKTEFDLL
{
    public class FactoryNoScreen : FactoryBase
    {
        private TaskScheduler taskTela;
        public IModalTEF modal;

        public override object Modal { get => modal; set => modal = (IModalTEF)value; }

        public FactoryNoScreen(bool task)
        {
            if (task)
                this.taskTela = TaskScheduler.FromCurrentSynchronizationContext();
        }

        protected override ITipoDeDado LeituraDados(PW_GetData pW_GetData, bool hide)
        {
            var wIdentificador = pW_GetData.wIdentificador;
            switch (wIdentificador)
            {
                case 245:   // Senha Logista
                    return new LeituraDefault(wIdentificador, "1111");
                case 246:   // Senha NTK
                    return new LeituraDefault(wIdentificador, "314159");
                case 8017:  // CPF,CNPJ
                case 28:    // CPF/CNPJ
                    return new LeituraDefault(wIdentificador, "07383312000170");
                case 17:    // PONTO CAPTURA
                    //return new LeituraDefault(wIdentificador, "53017");
                    if (taskTela == null)
                        return new LeituraDados(hide, pW_GetData, taskTela, modal);
                    else
                    {
                        ITipoDeDado Tipo = null;
                        Task TelaLeituraSenha = Task.Factory.StartNew(() =>
                        {
                            Tipo = new LeituraDados(hide, pW_GetData, taskTela, modal);
                        }, CancellationToken.None, TaskCreationOptions.None, taskTela);
                        Task.WaitAll(TelaLeituraSenha);
                        TelaLeituraSenha.Dispose();
                        return Tipo;
                    }
                case 32513:  // USAR PINPAD?
                    return new LeituraDefault(wIdentificador, "1");
                case 32514:  // BUSCA AUTOMATICA PINPAD
                    return new LeituraDefault(wIdentificador, "0");
                case 27:    // Nome Servidor
                    return new LeituraDefault(wIdentificador, "app.tpgw.ntk.com.br:17502");
                    //case 193: // TRANSAÇÃO DIGITADA
                    //case 194: // DATA VENCIMENTO
                    //case 60:  // PARCELAS
                    //case 58:  // TELEFONE
                    //case 56:  // CODIGO AFILIAÇÃO
                    //case 37:  // VALOR
                    //    return null;
            }

            if (pW_GetData.szPrompt == "CNPJ/CPF:")
                return new LeituraDefault(wIdentificador, "07383312000170");

            throw new Exception("Não implementado: " + pW_GetData.wIdentificador + "\r\n" + pW_GetData.szPrompt);
        }

        protected override ITipoDeDado Menu(PW_GetData pW_GetData)
        {
            return new MenuDefault(pW_GetData);
        }
    }
}
