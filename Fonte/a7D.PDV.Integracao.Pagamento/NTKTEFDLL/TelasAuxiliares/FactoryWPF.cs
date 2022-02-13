using MuxxLib;
using MuxxLib.Estruturas;
using MuxxLib.Interface;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace a7D.PDV.Integracao.Pagamento.NTKTEFDLL
{
    public class FactoryWPF : FactoryBase
    {
        private TaskScheduler taskTela;
        public IModalTEF modal;

        public override object Modal { get => modal; set => modal = (IModalTEF)value; }

        public FactoryWPF(bool task)
        {
            if (task)
                this.taskTela = TaskScheduler.FromCurrentSynchronizationContext();
        }

        internal void WaitModal(ITEF tef, int nParcelas, bool sempreAtivo)
        {
            modal = new ModalTEF(tef, nParcelas, sempreAtivo);
            ((Window)modal).ShowDialog();
        }

        protected override ITipoDeDado LeituraDados(PW_GetData pW_GetData, bool hide)
        {
            if (taskTela == null)
                return new LeituraDados(hide, pW_GetData, taskTela, modal);

            ITipoDeDado Tipo = null;
            Task TelaLeituraSenha = Task.Factory.StartNew(() =>
            {
                Tipo = new LeituraDados(hide, pW_GetData, taskTela, modal);
            }, CancellationToken.None, TaskCreationOptions.None, taskTela);
            Task.WaitAll(TelaLeituraSenha);
            TelaLeituraSenha.Dispose();
            return Tipo;
        }

        protected override ITipoDeDado Menu(PW_GetData pW_GetData)
        {
            //if (pW_GetData.stMenu.szTexto1 == "REDE")
            //    return new MenuDefault(pW_GetData);

            if (taskTela == null)
                return new Menu(ref pW_GetData, taskTela, modal);

            ITipoDeDado Tipo = null;
            Task TelaMenu = Task.Factory.StartNew(() =>
            {
                Tipo = new Menu(ref pW_GetData, taskTela, modal);
            }, CancellationToken.None, TaskCreationOptions.None, taskTela);
            Task.WaitAll(TelaMenu);
            TelaMenu.Dispose();
            return Tipo;
        }
    }
}