using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmAbrirComanda : FormTouch
    {
        public PedidoInformation Pedido { get; set; } = null;

        tbCliente Cliente1;
        ComandaInformation Comanda1;
        Boolean ComandaOK;

        public frmAbrirComanda()
        {
            InitializeComponent();
        }

        private void frmAbrirComanda_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            ComandaOK = false;

            List<TipoEntradaInformation> listaTipoEntrada = TipoEntrada.Listar().Where(l => l.Ativo == true).OrderBy(l => l.Nome).ToList();

            cbbTipoEntrada.Items.Add("");
            foreach (var item in listaTipoEntrada)
            {
                item.Nome = "(" + item.IDTipoEntrada.Value.ToString("00") + ") " + item.Nome;
                cbbTipoEntrada.Items.Add(item);
            }

            if (listaTipoEntrada.Count == 1)
            {
                lblTipoEntrada.Visible = false;
                txtCodigoEntrada.Visible = false;
                cbbTipoEntrada.Visible = false;
                cbbTipoEntrada.SelectedIndex = 1;
            }
            else
            {
                cbbTipoEntrada.SelectedValue = "";
            }

            cbbEstado.DataSource = Estado.Listar().OrderBy(l => l.Sigla).ToList();
            cbbEstado.SelectedValue = 25;

            frmPesquisarCliente frm = new frmPesquisarCliente();
            frm.ShowDialog();

            if (frm.Cliente1.IDCliente == null || frm.Cliente1.IDCliente == 0)
            {
                this.Close();
                return;
            }

            Cliente1 = EF.Repositorio.Carregar<tbCliente>(p => p.IDCliente == frm.Cliente1.IDCliente);

            txtNomeCompleto.Text = Cliente1.NomeCompleto;
            txtTelefone1DDD.Text = Cliente1.Telefone1DDD.ToString();
            txtTelefone1Numero.Text = Cliente1.Telefone1Numero.ToString();
            txtDocumento1.Text = Cliente1.Documento1;
            txtRG.Text = Cliente1.RG;
            txtEndereco.Text = Cliente1.Endereco;
            txtEnderecoNumero.Text = Cliente1.EnderecoNumero;
            txtComplemento.Text = Cliente1.Complemento;
            txtBairro.Text = Cliente1.Bairro;
            txtCidade.Text = Cliente1.Cidade;
            txtEmail.Text = Cliente1.Email;
            txtObservacao.Text = Cliente1.Observacao;

            if (Cliente1.DataNascimento.HasValue)
                txtDataNascimento.Text = Cliente1.DataNascimento.Value.ToString("dd/MM/yyyy");

            ckbBloqueado.Checked = Cliente1.Bloqueado;

            if (Cliente1.IDEstado != null)
                cbbEstado.SelectedValue = Cliente1.IDEstado;

            if (Cliente1.Bloqueado == true)
                ckbBloqueado.BackColor = Color.Red;
            else
                ckbBloqueado.BackColor = Color.Transparent;
        }

        private void btnAbrirComanda_Click(object sender, EventArgs e)
        {
            try
            {
                VerificarComanda();

                if (ComandaOK == true && cbbTipoEntrada.SelectedIndex > 0)
                {
                    txtDocumento1.Text = txtDocumento1.Text.Trim();
                    txtRG.Text = txtRG.Text.Trim();

                    string msg = Cliente.ValidarCliente(Cliente1.IDCliente,
                        txtNomeCompleto.Text, txtEndereco.Text + txtEnderecoNumero, txtComplemento.Text, txtBairro.Text, txtCidade.Text,
                        txtDocumento1.Text, txtRG.Text, txtDataNascimento.Text, false);

                    if (!string.IsNullOrEmpty(msg))
                    {
                        MessageBox.Show(msg);
                        return;
                    }

                    var idTipoEntrada = ((TipoEntradaInformation)cbbTipoEntrada.SelectedItem).IDTipoEntrada.Value;
                    TipoEntradaInformation tipoEntrada = TipoEntrada.Carregar(idTipoEntrada);

                    Cliente1.NomeCompleto = txtNomeCompleto.Text;
                    Cliente1.Documento1 = txtDocumento1.Text;
                    Cliente1.RG = txtRG.Text;
                    Cliente1.Endereco = txtEndereco.Text;
                    Cliente1.EnderecoNumero = txtEnderecoNumero.Text;
                    Cliente1.Complemento = txtComplemento.Text;
                    Cliente1.Bairro = txtBairro.Text;
                    Cliente1.Cidade = txtCidade.Text;
                    Cliente1.Observacao = txtObservacao.Text;
                    Cliente1.Bloqueado = ckbBloqueado.Checked;
                    Cliente1.Email = txtEmail.Text;

                    if (string.IsNullOrEmpty(txtDataNascimento.Text))
                        Cliente1.DataNascimento = null;
                    else
                        Cliente1.DataNascimento = DateTime.Parse(txtDataNascimento.Text);

                    if (string.IsNullOrEmpty(txtTelefone1DDD.Text))
                        Cliente1.Telefone1DDD = 0;
                    else
                        Cliente1.Telefone1DDD = Convert.ToInt32(txtTelefone1DDD.Text);

                    if (string.IsNullOrEmpty(txtTelefone1Numero.Text))
                        Cliente1.Telefone1Numero = 0;
                    else
                        Cliente1.Telefone1Numero = Convert.ToInt32(txtTelefone1Numero.Text);

                    if (string.IsNullOrEmpty(cbbEstado.SelectedValue.ToString()))
                        Cliente1.IDEstado = null;
                    else
                        Cliente1.IDEstado = Convert.ToInt32(cbbEstado.SelectedValue);

                    if (ckbBloqueado.Checked)
                    {
                        if (MessageBox.Show("Cuidado!\r\nEsse cliente está bloqueado\r\nDeseja selecionar esse cliente mesmo assim?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                            return;
                    }

                    EF.Repositorio.Atualizar(Cliente1);

                    var cliente = Cliente.Carregar(Cliente1.IDCliente);

                    if (ConfiguracoesSistema.Valores.ComandaComCredito
                     && tipoEntrada.ValorConsumacaoMinima > 0)
                    {
                        var saldo = Saldo.ClienteSaldoLiquido(Cliente1.IDCliente);
                        var consumacaoMin = tipoEntrada.ValorConsumacaoMinima.Value;
                        var valorMin = consumacaoMin;
                        if (saldo < valorMin)
                            valorMin = valorMin - saldo;

                        var frmValor = new frmTecladoValor(valorMin);
                        if (saldo > consumacaoMin)
                        {
                            valorMin = 0;
                            frmValor.PermitirZero = true;
                        }

                        if (saldo != 0)
                        {
                            frmValor.Text = $"Adicionar Saldo? Mínimo: {valorMin.ToString("C")}\r\nAtual: {saldo.ToString("C")}";
                            frmValor.TituloSize = 50;
                        }
                        else
                        {
                            frmValor.Text = $"Consumação Mínima: {consumacaoMin.ToString("C")}";
                        }

                        if (frmValor.ShowDialog() != DialogResult.OK)
                        {
                            MessageBox.Show($"É necessário adicionar pelo menos os créditos da consumação mínima de {tipoEntrada.ValorConsumacaoMinima.Value.ToString("C")}", "CONSUMAÇÃO MINIMA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (frmValor.Valor < valorMin)
                        {
                            MessageBox.Show($"Sado final inferior a consumação mínima de {consumacaoMin.ToString("C")}", "CONSUMAÇÃO MINIMA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (frmValor.Valor > 0)
                        {
                            var produto = Produto.Listar(new ProdutoInformation()
                            {
                                TipoProduto = new TipoProdutoInformation()
                                {
                                    IDTipoProduto = (int)ETipoProduto.Credito,
                                },
                                ValorUnitario = frmValor.Valor,
                            }).FirstOrDefault();

                            if (produto == null)
                            {
                                produto = Produto.Listar(new ProdutoInformation()
                                {
                                    TipoProduto = new TipoProdutoInformation()
                                    {
                                        IDTipoProduto = (int)ETipoProduto.Credito,
                                    },
                                    ValorUnitario = 0,
                                }).FirstOrDefault();
                            }

                            if (produto == null)
                            {
                                MessageBox.Show("Não foi encontrado um produto de crédito disponivel para realizar a venda", "CONSUMAÇÃO MINIMA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var pedido = BLL.Pedido.NovoPedidoBalcao(cliente);
                            var entradaCM = new PedidoProdutoInformation
                            {
                                Produto = produto,
                                Quantidade = 1,
                                Cancelado = false,
                                ValorUnitario = frmValor.Valor,
                                PDV = AC.PDV,
                                Usuario = AC.Usuario,
                                Pedido = pedido,
                                CodigoAliquota = produto.CodigoAliquota
                            };
                            PedidoProduto.Salvar(entradaCM);

                            var frm = NormalOuTouch.FechaPagamento(pedido.IDPedido.Value, false);
                            if (frm != DialogResult.OK)
                            {
                                BLL.Pedido.AlterarStatus(pedido.IDPedido.Value, EStatusPedido.Cancelado);
                                return;
                            }
                        }
                    }

                    Pedido = Comanda.AbrirComanda(Comanda1, cliente, tipoEntrada, AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Verificar \"Comanda\" e \"Tipo de Entrada!\"");
                }
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message}\n{ex.StackTrace}";
                var inner = ex.InnerException;
                while (inner != null)
                {
                    msg += $"\n{inner.Message}\n{inner.StackTrace}";
                    inner = inner.InnerException;
                }
                if (MessageBox.Show("Ocorreu um erro ao alterar o tipo de entrada.\nVer detalhes?", "Erro", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    MessageBox.Show("Erro", msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Close();
            }
        }

        private void txtCodigoComanda_Leave(object sender, EventArgs e)
        {
            VerificarComanda();
        }

        private void VerificarComanda()
        {
            if (txtCodigoComanda.Text != "")
            {
                Comanda1 = ComandaUtil.CarregarPorNumeroOuCodigo(txtCodigoComanda.Text);
                if (Comanda1.IDComanda == null)
                {
                    ComandaOK = false;
                    MessageBox.Show("Comanda não cadastrada!");
                }
                else
                {
                    if (Comanda1.ValorStatusComanda != EStatusComanda.Liberada)
                    {
                        ComandaOK = false;
                        MessageBox.Show("Essa comanda não pode ser usada!\nStatus da comanda: " + Comanda1.StatusComanda.Nome);
                    }
                    else
                        ComandaOK = true;
                }
            };
        }

        private void ApenasNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Name == txtCodigoComanda.Name)
            {
                if (Comanda.KeyPressValid(e))
                    e.Handled = true;

            }
            else
            {
                if (e.KeyChar != (char)8
                 && !char.IsNumber(e.KeyChar)
                 && e.KeyChar != (char)44)
                    e.Handled = true;
            }
        }

        private void txtCodigoEntrada_TextChanged(object sender, EventArgs e)
        {
            cbbTipoEntrada.SelectedIndex = 0;
            for (int i = 1; i < cbbTipoEntrada.Items.Count; i++)
            {
                if (((TipoEntradaInformation)cbbTipoEntrada.Items[i]).IDTipoEntrada.ToString() == txtCodigoEntrada.Text)
                {
                    cbbTipoEntrada.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ckbBloqueado_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbBloqueado.Checked == true)
                ckbBloqueado.BackColor = Color.Red;
            else
                ckbBloqueado.BackColor = Color.Transparent;
        }

        private void txtDataNascimento_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyChar != (char)8
             && e.KeyChar != (char)44
             && !char.IsNumber(e.KeyChar))
                e.Handled = true;

            if ((txtDataNascimento.Text.Length == 2 || txtDataNascimento.Text.Length == 5) && e.KeyChar != (char)8)
                txtDataNascimento.Text += "/";

            txtDataNascimento.SelectionStart = txtDataNascimento.Text.Length + 1;
        }
    }
}
