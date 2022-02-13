using MuxxLib;
using MuxxLib.Enum_s;
using MuxxLib.Estruturas;
using MuxxLib.Interface;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace a7D.PDV.Integracao.Pagamento.NTKTEFDLL
{
    public partial class LeituraDados : Window, ITipoDeDado
    {

        public static string SenhaLogista = Constantes.SENHALOGISTA;

        #region Atributos

        private PW_GetData _Data;
        private ushort _Identificador;
        private short _ResultCode;
        private byte _TamanhoMinimo;
        private byte _TamanhoMaximo;
        private StringBuilder _ResultMessage;
        private bool _Password;
        private TaskScheduler _IdTela;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        #region Contrustores

        public LeituraDados(bool hideEntry, PW_GetData data, TaskScheduler taskTela, IModalTEF modal)
        {
            InitializeComponent();
            this.Owner = (Window)modal;
            this._Data = data;
            this._Password = false;
            this._ResultMessage = new StringBuilder();
            this._ResultCode = 0;
            this._IdTela = taskTela;
            this.Topmost = true;
            ConfiguraçõesIniciais(hideEntry);
        }

        public LeituraDados(bool hideEntry, Window modal)
        {
            this.Owner = modal?.Owner;
            ((IModalTEF)Owner)?.HideWait();
            InitializeComponent();
        }

        #endregion

        #region Metodos

        private void ConfiguraçõesIniciais(bool hideEntry)
        {
            this.AlteraIdentificador(this._Data.wIdentificador);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            char entry = '&';
            ConfiguraEntradaDeDados(hideEntry);
            ConfiguraDirecaoDeEscrita();
            //if (this._Data.szMascaraDeCaptura != null) //UTILIZAR ESTE IF QUANDO TODAS AS MASCARAS FOREM TRATADAS
            //   this.txtbox_LeituraDados.Mask = this._Data.szMascaraDeCaptura.Replace('@', entry).Replace("R", "");
            if (this._Data.szMascaraDeCaptura != null && this._Identificador != 37 && this._Identificador != 96)
                this.txtbox_LeituraDados.Mask = this._Data.szMascaraDeCaptura.Replace('@', entry).Replace("R", "").Replace(".", ",");
            _TamanhoMinimo = this._Data.bTamanhoMinimo;
            _TamanhoMaximo = this._Data.bTamanhoMaximo;
        }

        private bool PermitirApenasNumeros()
        {
            switch (this._Identificador)
            {
                case 8017:  //CPF,CNPJ
                case 17:  //PONTO CAPTURA
                case 32513:  //USAR PINPAD?
                case 32514:  //BUSCA AUTOMATICA PINPAD
                case 28:  //CPF/CNPJ
                case 193:  //TRANSAÇÃO DIGITADA
                case 194:  //DATA VENCIMENTO
                case 60: //PARCELAS
                case 58:  //TELEFONE
                case 56:  //CODIGO AFILIAÇÃO
                case 37:  //VALOR
                    return true;
                default:
                    return false;
            }
        }

        private void ConfiguraDirecaoDeEscrita()
        {
            if (this._Data.bIniciaPelaEsquerda == 0)
            {
                txtbox_LeituraDadosSensiveis.FlowDirection = FlowDirection.RightToLeft;
                txtbox_LeituraDados.FlowDirection = FlowDirection.RightToLeft;
            }
        }

        private void ConfiguraEntradaDeDados(bool hideEntry)
        {
            if (hideEntry)
            {
                if (this._Identificador == 245)
                    this.SetTitulo("DIGITE A SENHA DO LOGISTA:");
                else
                    this.SetTitulo("DIGITE A SENHA TÉCNICA:");
                this._Password = true;
                this.txtbox_LeituraDadosSensiveis.Visibility = System.Windows.Visibility.Visible;
                txtbox_LeituraDadosSensiveis.Focus();
            }
            else
            {
                this.SetTitulo(this._Data.szPrompt);
                this.txtbox_LeituraDados.Visibility = System.Windows.Visibility.Visible;
                txtbox_LeituraDados.Focus();
            }
        }

        //public void AjustaTransacDigitada(string numero)
        //{
        //   this.txtbox_LeituraDados.Text = numero;
        //   this.SetTitulo("DIGITE O NÚMERO DO CARTÃO");
        //   this._Identificador = (ushort)PWINFO.CARDFULLPAN;
        //   this.txtbox_LeituraDados.Visibility = System.Windows.Visibility.Visible;
        //   this.txtbox_LeituraDados.TextAlignment = TextAlignment.Center;
        //   this._TamanhoMinimo = 16;
        //   this.txtbox_LeituraDados.SelectionStart = this.txtbox_LeituraDados.Text.Length;
        //   this.txtbox_LeituraDados.Focus();
        //}

        //ALTERA MASCARA DO TEXTBOX
        //public void SetMascara(string mask)
        //{
        //   this.txtbox_LeituraDados.Mask = mask;
        //}

        //private void CentralizaJanela()
        //{
        //   Window pai = Window.GetWindow(this);
        //   double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        //   double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        //   double windowWidth = this.Width;
        //   double windowHeight = this.Height;
        //   pai.Left = (screenWidth / 2) - (pai.Width / 2);
        //   pai.Top = (screenHeight / 2) - (pai.Height / 2);
        //}

        //EXECUTA OPERAÇÃO
        public short Operacao(ushort i, ref string message)
        {
            if (_IdTela == null)
                MostraTela();
            else
            {
                Task montaTela = Task.Factory.StartNew(() => MostraTela(), CancellationToken.None, TaskCreationOptions.None, _IdTela);
                Task.WaitAll(montaTela);
                montaTela.Dispose();
            }
            if (this.GetResultCode() == (short)PWRET.CANCEL)
            {
                if (_IdTela == null)
                    this.Close();
                else
                {
                    Task aux = Task.Factory.StartNew(() => this.Close(), CancellationToken.None, TaskCreationOptions.None, _IdTela);
                    Task.WaitAll(aux);
                    aux.Dispose();
                }
                this._ResultCode = (short)PWRET.CANCEL;
                this._ResultMessage = new StringBuilder("OPERACAO ABORTADA\nPELO USUARIO");
                Interop.PW_iPPAbort();
            }
            message = this._ResultMessage.ToString();
            return this.GetResultCode();
        }

        private void MostraTela()
        {
            if (this._Data.wIdentificador != 32513)
                this.txtbox_LeituraDados.Text = this._Data.szValorInicial;
            else
                this.txtbox_LeituraDados.Text = "";

            ((IModalTEF)Owner)?.HideWait();

            if (this._Data.wIdentificador == 27)
                LeituraIP_Porta();
            else
                this.ShowDialog();
        }

        //REALIZA A LEITURA DO IP E PORTA SEPARADAMENTE
        private void LeituraIP_Porta()
        {
            string ip = "";
            string porta = "";
            string text = "";
            if (!string.IsNullOrEmpty(this.txtbox_LeituraDados.Text))
            {
                text = this.txtbox_LeituraDados.Text;
                int index = this.txtbox_LeituraDados.Text.IndexOf(":");
                ip = text.Substring(0, index);
                porta = text.Substring(index, (text.Length - index));
                porta = porta.Replace(":", "");
            }
            string IP_Porta;
            LeituraDados telaIP = this;
            telaIP.SetTitulo("Digite o IP");
            telaIP.txtbox_LeituraDados.Text = ip;
            telaIP.ShowDialog();
            if (telaIP._ResultCode == (short)PWRET.CANCEL)
                return;
            IP_Porta = telaIP.txtbox_LeituraDados.Text;
            LeituraDados telaPorta = new LeituraDados(false, this);
            telaPorta.SetTitulo("Digite a Porta");
            telaPorta.txtbox_LeituraDados.Visibility = System.Windows.Visibility.Visible;
            telaPorta.txtbox_LeituraDados.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            telaPorta.txtbox_LeituraDados.Focus();
            telaPorta.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            telaPorta.txtbox_LeituraDados.Text = porta;
            telaPorta.ShowDialog();
            if (telaPorta._ResultCode == (short)PWRET.CANCEL)
                return;
            IP_Porta = string.Format("{0}{1}{2}", IP_Porta, ":", telaPorta.txtbox_LeituraDados.Text);
            this._ResultCode = Interop.PW_iAddParam(this._Identificador, IP_Porta);
        }

        private void AlteraIdentificador(ushort wIdentificador)
        {
            this._Identificador = wIdentificador;
        }

        public short GetResultCode()
        {
            return _ResultCode;
        }

        public void SetTitulo(string text)
        {
            if (text != null && text != "")
                lbl_Titulo.Content = text;
            else
                lbl_Titulo.Content = "SELECIONE:";
        }

        public void SetIdentificado(ushort id)
        {
            this._Identificador = id;
        }

        private void ConfirmaLeituraDadosSensiveis()
        {
            if (txtbox_LeituraDadosSensiveis.Password.Length < _TamanhoMinimo)
            {
                lbl_Titulo.Content = String.Format("Tamanho menor que o permitido: {0}", _TamanhoMinimo);
                lbl_Titulo.UpdateLayout();
                return;
            }
            if (_TamanhoMaximo != 0 && txtbox_LeituraDadosSensiveis.Password.Length > _TamanhoMaximo)
            {
                lbl_Titulo.Content = String.Format("Tamanho maior que o permitido: {0}", _TamanhoMaximo);
                lbl_Titulo.UpdateLayout();
                return;
            }
            if (this._Identificador == 245)
            {
                if (txtbox_LeituraDadosSensiveis.Password != SenhaLogista)
                {
                    lbl_Titulo.Content = String.Format("Senha incorreta");
                    lbl_Titulo.UpdateLayout();
                    return;
                }
            }
            else if (txtbox_LeituraDadosSensiveis.Password != Constantes.SENHA)
            {
                lbl_Titulo.Content = String.Format("Senha incorreta");
                lbl_Titulo.UpdateLayout();
                return;
            }
            ExecutaAddParam(txtbox_LeituraDadosSensiveis.Password);
        }

        private bool VerificaDados(string dados)
        {
            bool dadosCorretos = true;
            if (dados.Length < _TamanhoMinimo)
                dadosCorretos = TamanhoMenor(lbl_Titulo.Content.ToString());
            if (_TamanhoMaximo != 0 && dados.Length > _TamanhoMaximo)
                dadosCorretos = TamanhoMaior(lbl_Titulo.Content.ToString());
            return dadosCorretos;
        }

        private void ConfirmaLeituraDados()
        {
            string info = txtbox_LeituraDados.Text.ToString();
            if (this._Identificador != 0)
            {
                if (this._Identificador == 27)
                {
                    if (!info.Contains(':'))
                    {
                        this.Close();
                    }
                }
                else
                {
                    if (this._Identificador != 27)
                        info = LimpaDadosDeEntrada(info);
                    if (VerificaDados(info))
                        ExecutaAddParam(info);
                }
            }
            else
            {
                this.Close();
            }
        }

        private void ExecutaAddParam(string info)
        {
            if (this._Identificador == (short)PWINFO.CARDFULLPAN)
            {
                Interop.PW_iPPAbort();
                //Interop.PW_iAddParam((ushort)PWINFO.CARDENTMODE, "01");
            }
            this._ResultCode = Interop.PW_iAddParam(this._Identificador, info);
            this.Close();
        }

        private static string LimpaDadosDeEntrada(string info)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            info = rgx.Replace(info, "");
            info = info.Replace("-", "");
            return info;
        }

        private bool TamanhoMenor(string textAntigo)
        {
            if (!textAntigo.Contains("Tamanho menor que o permitido:"))
            {
                lbl_Titulo.Content = String.Format("{0}\nTamanho menor que o permitido: {1}", textAntigo, _TamanhoMinimo);
                lbl_Titulo.UpdateLayout();
            }
            return false;
        }

        private bool TamanhoMaior(string textAntigo)
        {
            if (!textAntigo.Contains("Tamanho maior que o permitido:"))
            {
                lbl_Titulo.Content = String.Format("{0}\nTamanho maior que o permitido: {1}", textAntigo, _TamanhoMaximo);
                lbl_Titulo.UpdateLayout();
            }
            return false;
        }

        #endregion

        #region Eventos da Tela

        private void btn_Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (!_Password)
                ConfirmaLeituraDados();
            else
                ConfirmaLeituraDadosSensiveis();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _ResultCode = (short)PWRET.CANCEL;
                this.Close();
            }
            else if (e.Key == Key.Enter)
                btn_Confirmar_Click(sender, new RoutedEventArgs());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CentralizaJanela();
            if (_TamanhoMaximo == 0)
                this.txtbox_LeituraDados.MaxLength = 20;
            else
                this.txtbox_LeituraDados.MaxLength = this._TamanhoMaximo;
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void ValidacaoNumerica(object sender, TextCompositionEventArgs e)
        {
            if (PermitirApenasNumeros())
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }

        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            ((IModalTEF)Owner)?.ShowWait();
        }
    }
}
