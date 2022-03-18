using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmPesquisarCliente : FormTouch
    {
        List<ClienteInformation> ListaCliente;
        public ClienteInformation Cliente1;
        public bool Delivery { get; set; }
        public bool Consulta { get; set; }

        public frmPesquisarCliente()
        {
            InitializeComponent();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idCliente;

            if (e.RowIndex >= 0)
            {
                idCliente = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["IDCliente"].Value);
                if (Consulta)
                {
                    var frm = new frmAdicionarCliente(idCliente);
                    frm.ShowDialog();
                }
                else
                {
                    Cliente1 = Cliente.Carregar(idCliente);
                    this.Close();
                }
            }
        }

        private void Listar()
        {
            string nome = txtNome.Text.ToLower();
            string numero = txtNumero.Text;
            string documento = txtDocumento.Text;

            var list = from l in ListaCliente
                       //where
                       //     l.NomeCompleto.ToLower().Contains(nome) &&
                       //     l.Telefone1Numero.ToString().Contains(numero) &&
                       //     ((l.Documento1 == null && documento == "") || (l.Documento1 != null && l.Documento1.Contains(documento)))
                       //orderby l.NomeCompleto
                       select new { l.IDCliente, l.NomeCompleto, l.Telefone1, l.Documento1, l.Email };

            dgvClientes.DataSource = list.ToArray();
        }

        private void frmPesquisarCliente_Load(object sender, EventArgs e)
        {
            GA.Post(this);

            this.LoadLocationSize(EConfig._ClientePesquisaLocationSize);

            txtNumero.Focus();

            ListaCliente = new List<ClienteInformation>();
            Cliente1 = new ClienteInformation();
        }

        private void txtCampoPesquisa_Leave(object sender, EventArgs e)
        {
            FimPesquisa(sender);
        }

        private void FimPesquisa(object sender)
        {
            TextBox txt = (TextBox)sender;
            int idCliente;
            string nome;
            string numero;
            string documento;

            if (!string.IsNullOrWhiteSpace(txt.Text))
            {
                nome = txtNome.Text;
                numero = txtNumero.Text;
                documento = txtDocumento.Text;

                if (dgvClientes.Rows.Count == 0)
                {
                    DialogResult r = MessageBox.Show("Nenhum cliente encontrado! Deseja cadastrar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        frmAdicionarCliente frm = new frmAdicionarCliente(nome, numero, documento, txtEmail.Text);
                        frm.Delivery = Delivery;
                        if( frm.ShowDialog()== DialogResult.OK)
                        {
                            if (frm.Cliente1.IDCliente > 0)
                                Cliente1 = Cliente.Carregar(frm.Cliente1.IDCliente);

                            if (Consulta)
                            {
                                ListaCliente.Add(Cliente1);
                                Listar();
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                    }
                }
                else if (dgvClientes.Rows.Count == 1)
                {
                    idCliente = Convert.ToInt32(dgvClientes.Rows[0].Cells["IDCliente"].Value);
                    if(Consulta)
                    {
                        var frm = new frmAdicionarCliente(idCliente);
                        frm.ShowDialog();
                    }
                    else
                    {
                        Cliente1 = Cliente.Carregar(idCliente);
                        this.Close();
                    }
                }
            }
        }

        private void ApenasNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8 && e.KeyChar != (char)44 && !char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
                e.Handled = true;
        }

        private void txtCampoPesquisa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FimPesquisa(sender);
        }

        private void txtNumero_TextChanged(object sender, EventArgs e)
        {
            txtDocumento.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtEmail.Text = string.Empty;

            if (txtNumero.Text.Length >= 5)
            {
                ListaCliente = Cliente.ListarResumido(telefone1Numero: Convert.ToInt32(txtNumero.Text));
            }
            else if (txtNumero.Text.Length < 5)
            {
                ListaCliente = new List<ClienteInformation>();
            }

            Listar();
        }

        private void txtDocumento_TextChanged(object sender, EventArgs e)
        {
            txtNumero.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtEmail.Text = string.Empty;

            if (txtDocumento.Text.Length >= 3)
            {
                ListaCliente = Cliente.ListarResumido(documento1: txtDocumento.Text);
            }
            else if (txtDocumento.Text.Length < 3)
            {
                ListaCliente = new List<ClienteInformation>();
            }

            Listar();
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            txtNumero.Text = string.Empty;
            txtDocumento.Text = string.Empty;
            txtEmail.Text = string.Empty;

            if (txtNome.Text.Length >= 3)
            {
                ListaCliente = Cliente.ListarResumido(nomeCompleto: txtNome.Text);
            }
            else if (txtNome.Text.Length < 3)
            {
                ListaCliente = new List<ClienteInformation>();
            }

            Listar();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtNumero.Text = string.Empty;
            txtDocumento.Text = string.Empty;
            txtNome.Text = string.Empty;

            if (txtEmail.Text.Length >= 3)
            {
                ListaCliente = Cliente.ListarResumido(email: txtEmail.Text);
            }
            else if (txtEmail.Text.Length < 3)
            {
                ListaCliente = new List<ClienteInformation>();
            }

            Listar();
        }

        private void frmPesquisarCliente_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveLocationSize(EConfig._ClientePesquisaLocationSize);
        }
    }
}
