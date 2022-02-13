using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace a7D.PDV.SATService.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer timer = new DispatcherTimer();
        private bool executando = false;
        public ObservableCollection<DataGridItem> itens = new ObservableCollection<DataGridItem>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
            dgItens.ItemsSource = itens;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (executando)
                return;

            executando = true;

            var sat = new SAT.SAT();

            var lista = SATServer.ListarPendentes().Take(10);
            var listaErro = SATServer.ListarComErro().Take(10);

            foreach (var s in lista)
            {
                Processar(sat, s);
            }

            foreach (var se in listaErro)
            {
                Processar(sat, se);
            }
            executando = false;
        }

        private void Processar(SAT.SAT sat, SATServerInformation s)
        {
            var item = itens.FirstOrDefault(a => a.ID == s.IDSATServer.Value);
            if (item == null)
            {
                item = new DataGridItem
                {
                    ID = s.IDSATServer.Value,
                    PDV = s.IDPDV.Value,
                    Usuario = s.Usuario.Nome,
                    Processando = true,
                    Tipo = s.TipoSolicitacao == 1 ? "Enviar Venda" : "Cancelamento",
                    Erro = s.Erro
                };
                itens.Add(item);
            }

            item.Processando = true;

            try
            {
                if (s.TipoSolicitacao == 1)
                {
                    s.Pedido = Pedido.Carregar(s.Pedido.IDPedido.Value);
                    s.RetornoSAT = sat.Venda(s.Pedido, true ,s.IDPDV.Value, s.Usuario.IDUsuario.Value).Enviar();
                    s.Processado = true;
                    s.Erro = string.Empty;
                    s.ErroST = string.Empty;
                    SATServer.Salvar(s);
                }
                else if (s.TipoSolicitacao == 2)
                {
                    s.Pedido = Pedido.Carregar(s.Pedido.IDPedido.Value);
                    var pedido = s.Pedido;
                    s.RetornoSAT_cancelamento = sat.Cancelamento(s.RetornoSAT, s.IDPDV.Value, s.Usuario.IDUsuario.Value).Enviar(out pedido);
                    s.Pedido = pedido;
                    Pedido.Salvar(s.Pedido);
                    s.Processado = true;
                    s.Erro = string.Empty;
                    s.ErroST = string.Empty;
                    SATServer.Salvar(s);
                }
                item.Sucesso = true;
            }
            catch (Exception ex)
            {
                s.Processado = false;
                s.Erro = ex.Message;
                s.ErroST = ex.StackTrace;
                item.Sucesso = false;
                var inner = ex.InnerException;
                while (inner != null)
                {
                    s.ErroST += inner.StackTrace;
                    inner = ex.InnerException;
                }
                SATServer.Salvar(s);
            }

            item.Processando = false;
            item.Erro = s.Erro;
        }

        public class DataGridItem
        {
            public int ID { get; set; }
            public string Usuario { get; set; }
            public int PDV { get; set; }
            public string Tipo { get; set; }
            public bool Processando { get; set; }
            public bool Sucesso { get; set; }
            public string Erro { get; set; }
        }
    }
}
