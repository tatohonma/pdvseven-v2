using MuxxLib;
using MuxxLib.Enum_s;
using MuxxLib.Estruturas;
using MuxxLib.Interface;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace a7D.PDV.Integracao.Pagamento.NTKTEFDLL
{

    public partial class Menu : Window, ITipoDeDado
    {

        #region Atributos

        private bool _JanelaAtiva;
        public ushort Identificador;
        public short resultCode;
        public StringBuilder ResultMessage;
        public List<string> Values;
        public PW_GetData getData;
        private TaskScheduler _IdTela;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        #region Construtores

        public Menu(ref PW_GetData getData, TaskScheduler taskTela, IModalTEF modal)
        {
            InitializeComponent();
            this.Owner = (Window)modal;
            this._IdTela = taskTela;
            this.resultCode = 0;
            this.ResultMessage = new StringBuilder();
            this.Values = new List<string>();
            this.getData = getData;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            this.Focusable = true;
            this.Focus();
        }

        #endregion

        #region Metodos

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
            if (this.getResultCode() == (short)PWRET.CANCEL)
            {
                if (_IdTela == null)
                    this.Close();
                else
                {
                    Task aux = Task.Factory.StartNew(() => this.Close(), CancellationToken.None, TaskCreationOptions.None, _IdTela);
                    Task.WaitAll(aux);
                    aux.Dispose();
                }
                this.resultCode = (short)PWRET.CANCEL;
                this.ResultMessage = new StringBuilder(" OPERACAO ABORTADA\n    PELO USUARIO");
            }
            message = this.ResultMessage.ToString();
            return this.getResultCode();
        }

        private void MostraTela()
        {
            this.setPrompt(this.getData.szPrompt.Trim());
            this.Populate(this.getData.stMenu, this.getData.bNumOpcoesMenu, this.getData.wIdentificador);
            this.Topmost = true;
            ((IModalTEF)Owner)?.HideWait();
            this.ShowDialog();
        }

        public short getResultCode()
        {
            return resultCode;
        }

        private void setPrompt(string text)
        {
            if (text != null && text != "")
                lbPrompt.Text = text;
            else
                lbPrompt.Text = "SELECIONE:";
        }

        private void Populate(PW_Menu menu, byte bNumOpcoesMenu, ushort wIdentificador)
        {
            List<string> text = new List<string>();
            string tempText, tempValue;
            int aux;
            this.Identificador = wIdentificador;
            for (byte b = 0; b < bNumOpcoesMenu; b++)
            {
                switch (b)
                {
                    case (0):
                        tempText = menu.szTexto1;
                        tempValue = menu.szValor1;
                        break;

                    case (1):
                        tempText = menu.szTexto2;
                        tempValue = menu.szValor2;
                        break;

                    case (2):
                        tempText = menu.szTexto3;
                        tempValue = menu.szValor3;
                        break;

                    case (3):
                        tempText = menu.szTexto4;
                        tempValue = menu.szValor4;
                        break;

                    case (4):
                        tempText = menu.szTexto5;
                        tempValue = menu.szValor5;
                        break;

                    case (5):
                        tempText = menu.szTexto6;
                        tempValue = menu.szValor6;
                        break;

                    case (6):
                        tempText = menu.szTexto7;
                        tempValue = menu.szValor7;
                        break;

                    case (7):
                        tempText = menu.szTexto8;
                        tempValue = menu.szValor8;
                        break;

                    case (8):
                        tempText = menu.szTexto9;
                        tempValue = menu.szValor9;
                        break;

                    case (9):
                        tempText = menu.szTexto10;
                        tempValue = menu.szValor10;
                        break;

                    default:
                        tempText = "ERR";
                        tempValue = "ERR";
                        break;
                }
                aux = Convert.ToInt32(b) + 1;
                text.Add(string.Format("{0} - {1}", aux, tempText));
                Values.Add(tempValue);
            }
            libMenu.ItemsSource = text;
        }

        #endregion

        #region Eventos da Tela

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (this._JanelaAtiva)
            {
                if (e.Key == Key.Escape)
                {
                    resultCode = (short)PWRET.CANCEL;
                    this.Close();
                    this._JanelaAtiva = false;
                }
                else if (e.Key > Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    if (KeyInterop.VirtualKeyFromKey(e.Key) - 0x60 <= libMenu.Items.Count)
                    {
                        int select = KeyInterop.VirtualKeyFromKey(e.Key) - 0x60;
                        libMenu.SelectedIndex = select - 1;
                        libMenu_DoubleClick(sender, new EventArgs());
                    }
                }
                else if (e.Key == Key.Enter)
                    libMenu_DoubleClick(sender, new EventArgs());
            }
        }

        private void libMenu_DoubleClick(object sender, EventArgs eventArgs)
        {
            if (this._JanelaAtiva)
            {
                this.resultCode = (short)Interop.PW_iAddParam(Identificador, Values[(libMenu.SelectedIndex)]);
                this.Close();
                this._JanelaAtiva = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            this._JanelaAtiva = true;
            libMenu.Focus();
            libMenu.SelectedIndex = 0;
            var listBoxItem = (ListBoxItem)libMenu.ItemContainerGenerator.ContainerFromItem(libMenu.SelectedItem);
            listBoxItem.Focus();
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            ((IModalTEF)Owner)?.ShowWait();
        }
    }
}
