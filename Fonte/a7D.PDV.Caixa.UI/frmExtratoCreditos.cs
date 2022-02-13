using a7D.PDV.BLL;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.Componentes;
using a7D.PDV.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmExtratoCreditos : FormTouch
    {
        PedidoInformation pedido { get; set; }
        int idCliente { get; set; }
        ExtratoCreditos[] extrato { get; set; }
        ExtratoItens[] itens { get; set; }

        public frmExtratoCreditos()
        {
            InitializeComponent();
        }

        public frmExtratoCreditos(int ClienteID) : this()
        {
            this.idCliente = ClienteID;
        }

        private async void frmDetalhesPedidoProduto_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                extrato = Saldo.ExtratoCreditos(idCliente);
                itens = Saldo.ExtratoItens(idCliente);
            });

            GA.Post(this);
            CarregarSaldos();
        }

        private void CarregarSaldos()
        {
            dgvSaldo.Rows.Clear();
            foreach (var item in extrato)
            {
                dgvSaldo.Rows.Add(new object[] {
                    item.Data,      // 0
                    item.IDSaldo == 999999 ? "aberto": (object)item.IDSaldo, // 1
                    item.IDPedido,  // 2
                    item.Origem,    // 3
                    item.Tipo,      // 4
                    item.Valor,     // 5
                    item.Saldo,     // 6
                    item.Agrupamento// 7
                });
            }
        }

        private void dgvSaldo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSaldo.SelectedRows.Count > 0)
            {
                var pedido = int.Parse(dgvSaldo.SelectedRows[0].Cells[2].Value.ToString());
                CarregarComanda(pedido);
            }
        }

        private void CarregarComanda(int pedido)
        {
            dgvItem.Rows.Clear();
            var filtro = itens.Where(i => i.IDPedido == pedido);
            foreach (var item in filtro)
            {
                dgvItem.Rows.Add(new object[]
                {
                    item.Data,
                    item.Tipo,
                    item.Item,
                    item.Valor
                });
            }
        }
    }
}