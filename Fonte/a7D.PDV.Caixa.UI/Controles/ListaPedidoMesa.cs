using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;

namespace a7D.PDV.Caixa.UI.Controles
{
    public partial class ListaPedidoMesa : UserControl
    {
        public delegate void PedidoSelecionadoEventHandler(object sender, System.EventArgs e);
        public event PedidoSelecionadoEventHandler PedidoSelecionado;
        private Filtro FiltroAtual { get; set; }
        private bool Filtrando { get; set; }
        private bool Atualizando { get; set; }
        private bool ManterPosicao { get; }


        public String GUIDIdentificacao_selecionado;
        List<MesaInformation> ListaMesas = new List<MesaInformation>();

        public ListaPedidoMesa()
        {
            if (ConfiguracoesCaixa.Valores != null)
                ManterPosicao = ConfiguracoesCaixa.Valores.ManterPosicaoLista;

            InitializeComponent();
        }

        public void AtualizarLista()
        {
            if (ListaMesas.Count == 0)
                CarregarMesas();

            AtualizarStatusMesas();
        }

        private void CarregarMesas()
        {
            ListaMesas = Mesa.Listar();
            String[] mesa;

            foreach (var item in ListaMesas.OrderBy(m => m.Numero))
            {
                mesa = new String[4];
                mesa[0] = item.Numero.ToString();
                mesa[1] = item.IDMesa.ToString();
                mesa[2] = item.GUIDIdentificacao;
                mesa[3] = item.StatusMesa.IDStatusMesa.ToString();

                ltvMesas.Items.Add(new ListViewItem(mesa, 3));
            }
        }

        private void Filtrar()
        {
            while (Atualizando) { }
            AtualizarStatusMesas(false);
            Filtrando = true;

            ltvMesas.BeginUpdate();
            List<SortWrapper> lista = new List<SortWrapper>();
            foreach (ListViewItem item in ltvMesas.Items)
            {
                lista.Add(new SortWrapper(item));
            }

            switch (FiltroAtual)
            {
                case Filtro.ASC:
                    lista = lista.OrderBy(i => i.IDMesa).ToList();
                    break;
                case Filtro.DESC:
                    lista = lista.OrderByDescending(i => i.IDMesa).ToList();
                    break;
                case Filtro.OCUPADAS:
                    lista = lista.OrderByDescending(i => i.Status == 20).ThenBy(i => i.IDMesa).ToList();
                    break;
                case Filtro.LIVRES:
                    lista = lista.OrderByDescending(i => i.Status == 10).ThenBy(i => i.IDMesa).ToList();
                    break;
                case Filtro.CONTA:
                    lista = lista.OrderByDescending(i => i.Status == 30).ThenBy(i => i.IDMesa).ToList();
                    break;
            }


            ltvMesas.Items.Clear();

            foreach (var item in lista)
            {
                ltvMesas.Items.Add(item.Item);
            }
            ltvMesas.EndUpdate();
            Filtrando = false;
        }

        private void ltvMesas_Click(object sender, EventArgs e)
        {
            GUIDIdentificacao_selecionado = ltvMesas.FocusedItem.SubItems[2].Text;
            AtualizarStatusMesas();

            PedidoSelecionado(sender, e);
        }

        private void AtualizarStatusMesas(bool filtrar = false)
        {
            if (Filtrando)
                return;

            txtMesa_Leave(null, null);

            Atualizando = true;
            ListViewItem selected = null;

            if (ManterPosicao && ltvMesas.SelectedItems.Count > 0)
                selected = ltvMesas.SelectedItems[0];

            List<MesaInformation> listaMesas = Mesa.Listar();

            Int32 idMesa;
            MesaInformation mesa;
            Int32 indexImg;

            foreach (ListViewItem item in ltvMesas.Items)
            {
                idMesa = Convert.ToInt32(item.SubItems[1].Text);
                mesa = listaMesas.Find(l => l.IDMesa == idMesa);
                item.SubItems[3].Text = mesa.StatusMesa.IDStatusMesa.ToString();

                if (mesa.GUIDIdentificacao != GUIDIdentificacao_selecionado)
                {
                    switch ((EStatusMesa)mesa.StatusMesa.IDStatusMesa)
                    {
                        case EStatusMesa.Liberada:
                            indexImg = 0;
                            break;
                        case EStatusMesa.EmAtendimento:
                            indexImg = 2;
                            break;
                        case EStatusMesa.ContaSolicitada:
                            indexImg = 1;
                            break;
                        default:
                            indexImg = 1;
                            break;
                    }
                }
                else
                {
                    switch ((EStatusMesa)mesa.StatusMesa.IDStatusMesa)
                    {
                        case EStatusMesa.Liberada:
                            indexImg = 3;
                            break;
                        case EStatusMesa.EmAtendimento:
                            indexImg = 5;
                            break;
                        case EStatusMesa.ContaSolicitada:
                            indexImg = 4;
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

        private void txtMesa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtMesa.Text != "")
            {
                List<MesaInformation> listaMesas = Mesa.Listar();

                if (!Int32.TryParse(txtMesa.Text, out int numeroMesa))
                    return;

                MesaInformation mesa = Mesa.CarregarPorNumero(numeroMesa);

                if (mesa != null && mesa.IDMesa != null)
                {
                    GUIDIdentificacao_selecionado = mesa.GUIDIdentificacao;
                    AtualizarStatusMesas();

                    PedidoSelecionado(sender, e);

                    txtMesa.Text = "";
                }
                else
                {
                    MessageBox.Show("Mesa não cadastrada!");
                }
            }
        }

        private void txtMesa_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void cbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltroAtual = (Filtro)cbFiltro.SelectedIndex;
            while (Filtrando) { }
            Filtrar();
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
            public int IDMesa { get; set; }
            public string GUIDIdentificacao { get; set; }
            public int Status { get; set; }
            public ListViewItem Item { get; set; }

            public SortWrapper(ListViewItem item)
            {
                Item = item;
                Numero = Convert.ToInt32(item.SubItems[0].Text);
                IDMesa = Convert.ToInt32(item.SubItems[1].Text);
                GUIDIdentificacao = item.SubItems[2].Text;
                Status = Convert.ToInt32(item.SubItems[3].Text);
            }
        }

        private async void txtMesa_Leave(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            txtMesa.Focus();
        }
    }
}
