using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Threading.Tasks;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class ListaPedidoComandaSemCheckin : UserControl
    {
        public delegate void PedidoSelecionadoEventHandler(object sender, EventArgs e);
        public event PedidoSelecionadoEventHandler PedidoSelecionado;

        private Filtro FiltroAtual { get; set; }

        private List<ComandaInformation> ListaComandas = new List<ComandaInformation>();
        public string GUIDIdentificacao_selecionado { get; set; }
        public int NumeroComanda { get; set; }
        private bool Atualizando { get; set; }
        private bool Filtrando { get; set; }
        private bool ManterPosicao { get; }

        public ListaPedidoComandaSemCheckin()
        {
            if (ConfiguracoesCaixa.Valores != null)
                ManterPosicao = ConfiguracoesCaixa.Valores.ManterPosicaoLista;

            InitializeComponent();
        }

        public void AtualizarLista()
        {
            if (ListaComandas.Count == 0)
                CarregarComandas();
            AtualizarStatusComandas();
        }

        private void CarregarComandas()
        {
            ListaComandas = Comanda.Listar();
            foreach (var comanda in ListaComandas.OrderBy(c => c.Numero))
            {
                var item = new string[] {
                    comanda.Numero.ToString(),
                    comanda.IDComanda.ToString(),
                    comanda.GUIDIdentificacao,
                    comanda.StatusComanda.IDStatusComanda.ToString()
                };

                lvComandas.Items.Add(new ListViewItem(item, 3));
            }

        }

        private void Filtrar()
        {
            while (Atualizando) { }
            AtualizarStatusComandas(false);
            Filtrando = true;
            
            lvComandas.BeginUpdate();
            List<SortWrapper> lista = new List<SortWrapper>();
            foreach (ListViewItem item in lvComandas.Items)
            {
                lista.Add(new SortWrapper(item));
            }

            switch (FiltroAtual)
            {
                case Filtro.ASC:
                    lista = lista.OrderBy(i => i.IDComanda).ToList();
                    break;
                case Filtro.DESC:
                    lista = lista.OrderByDescending(i => i.IDComanda).ToList();
                    break;
                case Filtro.OCUPADAS:
                    lista = lista.OrderByDescending(i => i.Status == 20).ThenBy(i => i.IDComanda).ToList();
                    break;
                case Filtro.LIVRES:
                    lista = lista.OrderByDescending(i => i.Status == 10).ThenBy(i => i.IDComanda).ToList();
                    break;
                case Filtro.CONTA:
                    lista = lista.OrderByDescending(i => i.Status == 30).ThenBy(i => i.IDComanda).ToList();
                    break;
            }


            lvComandas.Items.Clear();

            foreach (var item in lista)
            {
                lvComandas.Items.Add(item.Item);
            }
            lvComandas.EndUpdate();
            Filtrando = false;
        }

        private void AtualizarStatusComandas(bool filtrar = false)
        {
            if (Filtrando)
                return;

            Atualizando = true;
            txtComanda_Leave(null, null);

            var listaComandas = Comanda.Listar();
            ListViewItem selected = null;
            if (ManterPosicao && lvComandas.SelectedItems.Count > 0)
                selected = lvComandas.SelectedItems[0];

            foreach (ListViewItem item in lvComandas.Items)
            {
                var idComanda = Convert.ToInt32(item.SubItems[1].Text);
                var comanda = listaComandas.Find(l => l.IDComanda == idComanda);
                if (comanda == null)
                    continue;

                item.SubItems[3].Text = comanda.StatusComanda.IDStatusComanda.ToString();

                var indexImg = 0;

                if (comanda.GUIDIdentificacao != GUIDIdentificacao_selecionado)
                {
                    switch (comanda.ValorStatusComanda)
                    {
                        case EStatusComanda.Liberada:
                            indexImg = 0;
                            break;
                        case EStatusComanda.EmUso:
                            indexImg = 2;
                            break;
                        case EStatusComanda.Cancelada:
                            indexImg = 1;
                            break;
                        case EStatusComanda.Perdida:
                            indexImg = 6;
                            break;
                        case EStatusComanda.ContaSolicitada:
                            indexImg = 8;
                            break;
                        default:
                            indexImg = 1;
                            break;
                    }
                }
                else
                {
                    switch (comanda.ValorStatusComanda)
                    {
                        case EStatusComanda.Liberada:
                            indexImg = 3;
                            break;
                        case EStatusComanda.EmUso:
                            indexImg = 5;
                            break;
                        case EStatusComanda.Cancelada:
                            indexImg = 4;
                            break;
                        case EStatusComanda.Perdida:
                            indexImg = 7;
                            break;
                        case EStatusComanda.ContaSolicitada:
                            indexImg = 9;
                            break;
                        default:
                            indexImg = 4;
                            break;
                    }
                }
                item.ImageIndex = indexImg;
            }

            Atualizando = false;
            if (filtrar)
                Filtrar();
            selected?.EnsureVisible();
        }

        private void lvComandas_Click(object sender, EventArgs e)
        {
            GUIDIdentificacao_selecionado = lvComandas.FocusedItem.SubItems[2].Text;
            NumeroComanda = Convert.ToInt32(lvComandas.FocusedItem.SubItems[0].Text);
            AtualizarStatusComandas();

            txtComanda.Focus();

            PedidoSelecionado(sender, e);
        }

        private void txtComanda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtComanda.Text))
            {
                var listaComandas = Comanda.Listar();

                var comanda = ComandaUtil.CarregarPorNumeroOuCodigo(txtComanda.Text);
                if (comanda?.IDComanda != null)
                {
                    GUIDIdentificacao_selecionado = comanda.GUIDIdentificacao;
                    AtualizarStatusComandas();
                    PedidoSelecionado(sender, e);
                    txtComanda.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Comanda não cadastrada!");
                }
            }
        }

        private void txtComanda_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (Comanda.KeyPressValid(e))
                e.Handled = true;
        }

        enum Filtro
        {
            ASC = 0,
            DESC = 1,
            OCUPADAS = 2,
            LIVRES = 3,
            CONTA = 4
        }

        internal class SortWrapper
        {
            public int Numero { get; set; }
            public int IDComanda { get; set; }
            public string GUIDIdentificacao { get; set; }
            public int Status { get; set; }
            public ListViewItem Item { get; set; }

            public SortWrapper(ListViewItem item)
            {
                Item = item;
                Numero = Convert.ToInt32(item.SubItems[0].Text);
                IDComanda = Convert.ToInt32(item.SubItems[1].Text);
                GUIDIdentificacao = item.SubItems[2].Text;
                Status = Convert.ToInt32(item.SubItems[3].Text);
            }
        }

        private async void txtComanda_Leave(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            txtComanda.Focus();
        }
    }
}
