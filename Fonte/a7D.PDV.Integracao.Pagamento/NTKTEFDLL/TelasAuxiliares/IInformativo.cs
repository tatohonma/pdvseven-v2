namespace a7D.PDV.Integracao.Pagamento.NTKTEFDLL
{
    public interface IInformativo
    {
        //int ContadorSegundos { get; set; }

        void AlteraMensagem(string texto);
        void AlteraMensagemVoltaPdv(string texto);
        //void CopiaGetData(PW_GetData[] getData);
        //void CopiaLista(List<TransacaoConcluida> list);
        //void CopiaTxtBox(TextBox txt);
        short GetResultAndChange();
        //void InitializeComponent();
        //void RedirecionaUserControl(object sender, EventArgs e);
        //void setForcaDigitada(bool estado);
        bool VerificaCancelamento();
        short VerificaResult();
    }
}