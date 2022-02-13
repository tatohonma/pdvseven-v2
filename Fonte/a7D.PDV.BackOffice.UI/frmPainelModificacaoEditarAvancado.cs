using a7D.PDV.BLL;
using a7D.PDV.BLL.Extension;
using a7D.PDV.BLL.Utils;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using a7D.PDV.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmPainelModificacaoEditarAvancado : Form
    {

        BindingList<ViewLista> listaBindingPaineis = new BindingList<ViewLista>();
        BindingList<ViewLista> listaBindingItens = new BindingList<ViewLista>();
        int cbbTipoItemIndexAntigo = 0;

        List<ViewLista> ItensSelecionados = new List<ViewLista>();
        List<ViewLista> ItensCarregados = new List<ViewLista>();
        List<ProdutoInformation> listaProdutos;
        List<ProdutoInformation> listaModificacoes;
        List<CategoriaAtiva> listaCategorias;
        List<ViewLista> PaineisCarregados = new List<ViewLista>();
        List<ViewLista> PaineisSelecionados = new List<ViewLista>();
        private PainelModificacaoInformation PainelModificacao1;

        private class ViewLista
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public int Ordem { get; set; }
            public ViewLista() { }
            public ViewLista(int id, string nome, int ordem)
            {
                ID = id; Nome = nome; Ordem = ordem;
            }

            public ViewLista(int id, string nome)
            {
                ID = id; Nome = nome;
            }
        }

        public frmPainelModificacaoEditarAvancado()
        {
            InitializeComponent();
            PainelModificacao1 = new PainelModificacaoInformation();
            PainelModificacao1.ListaProduto = new List<PainelModificacaoProdutoInformation>();
            PainelModificacao1.ListaCategoria = new List<PainelModificacaoCategoriaInformation>();
            PainelModificacao1.PaineisRelacionados = new List<PainelModificacaoRelacionadoInformation>();
        }
        public frmPainelModificacaoEditarAvancado(Int32 idPainelModificacao, List<PainelModificacaoInformation> paineisModificacao)
        {
            InitializeComponent();
            PainelModificacao1 = PainelModificacao.CarregarCompleto(idPainelModificacao);
            txtMax.Text = PainelModificacao1.Maximo.ToString();
            txtMin.Text = PainelModificacao1.Minimo.ToString();
            txtNome.Text = PainelModificacao1.Nome;
            txtTitulo.Text = PainelModificacao1.Titulo;
            if (PainelModificacao1.IDTipoItem != null)
            {
                cbbTipoItens.SelectedIndex = PainelModificacao1.IDTipoItem.Value - 1;
                cbbTipoItemIndexAntigo = PainelModificacao1.IDTipoItem.Value - 1;
            }
            else
                cbbTipoItens.SelectedIndex = 0;

            ckbIgnorarValor.Checked = PainelModificacao1.IgnorarValorItem.HasValue && PainelModificacao1.IgnorarValorItem.Value;
            var idValor = PainelModificacao1.IDValorUtilizado;

            if (PainelModificacao1.ListaProduto?.Count() > 0)
            {
                ItensSelecionados = PainelModificacao1.ListaProduto
                    .Where(p => p.Produto != null)
                    .Select(p => new ViewLista(p.Produto.IDProduto.Value, p.Produto.Nome, p.Ordem.Value)).ToList();
            }
            else if (PainelModificacao1.ListaCategoria?.Count > 0)
            {
                ItensSelecionados = PainelModificacao1.ListaCategoria
                    .Select(p => new ViewLista(p.Categoria.IDCategoriaProduto.Value, p.Categoria.Nome, p.Ordem.Value)).ToList();
            }

            if (PainelModificacao1.PaineisRelacionados?.Count() > 0)
                PaineisSelecionados = PainelModificacao1.PaineisRelacionados
                 .Select(p => new ViewLista(p.PainelModificacao2.IDPainelModificacao.Value,
                     p.PainelModificacao2.Nome
                 //                     p.Ordem != null ? p.Ordem.Value : 0
                 )).ToList();

            AjustarOrdemZerosNulo(PaineisSelecionados);
            AjustarOrdemZerosNulo(ItensSelecionados);




        }

        private void frmPainelModificacaoEditarAvancado_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            //await Task.Run(new Action(() =>
            //{
            //    PaineisCarregados = PainelModificacao.ListarCompleto().OrderBy(p => p.Nome)
            //  .Select(p => new ViewLista(p.IDPainelModificacao.Value, p.Nome)).ToList();

            //    foreach (var item in PaineisSelecionados)
            //    {
            //        var aux = PaineisCarregados.Where(p => p.ID == item.ID).FirstOrDefault();
            //        if (aux != null)
            //            PaineisCarregados.Remove(aux);

            //    }
            //}));

            PaineisCarregados = PainelModificacao.ListarCompleto().OrderBy(p => p.Nome)
            .Select(p => new ViewLista(p.IDPainelModificacao.Value, p.Nome)).ToList();

            foreach (var item in PaineisSelecionados)
            {
                var aux = PaineisCarregados.Where(p => p.ID == item.ID).FirstOrDefault();
                if (aux != null)
                    PaineisCarregados.Remove(aux);

            }


            cbbCalculoValor.DataSource = PainelModificacaoOperacao.Listar();
            cbbCalculoValor.ValueMember = "IDPainelModificacaoOperacao";
            cbbCalculoValor.DisplayMember = "Nome";

            if (PainelModificacao1.PainelModificacaoOperacao != null)
                cbbCalculoValor.SelectedIndex = PainelModificacao1.PainelModificacaoOperacao.IDPainelModificacaoOperacao.Value - 1;

            cbbTabelaPrecos.SelectedIndex = 0;
            if (PainelModificacao1.IDValorUtilizado != null)
            {
                cbbTabelaPrecos.SelectedIndex = PainelModificacao1.IDValorUtilizado.Value - 1;
            }

            ltbPainel1.DataSource = PaineisCarregados;
            BindListaPaineis1();
            dgvItens.DataSource = ItensSelecionados;
            //bindSource = new BindingSource(PaineisSelecionados, null);
            listaBindingPaineis = new BindingList<ViewLista>(PaineisSelecionados);
            dgvPaineis.DataSource = listaBindingPaineis;

        }

        private async void cbbTipoItens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItensSelecionados.Count > 0 && cbbTipoItens.SelectedIndex != cbbTipoItemIndexAntigo)
            {
                var result = MessageBox.Show("Alterando este campo, a lista da direita será removida e deverá ser feita novamente. Um painel de modificação só permite agrupar itens do mesmo tipo.", "Atenção", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    ItensSelecionados.Clear();
                    BindListaItem2();
                    await MudancaDropDownTipoItem();
                }
                else
                {
                    ((ComboBox)sender).SelectedIndex = cbbTipoItemIndexAntigo;
                }
            }
            else
            {
                await MudancaDropDownTipoItem();
            }





        }

        private async Task MudancaDropDownTipoItem()
        {
            cbbTipoItemIndexAntigo = cbbTipoItens.SelectedIndex;
            ltbItens1.DataSource = null;
            ptbLoad.Visible = true;
            var index = cbbTipoItens.SelectedIndex;
            switch (index)
            {
                case 0:
                    if (listaModificacoes == null)
                    {

                        listaModificacoes = await Task.Run(() => { return Produto.ListarApenasModificacao().OrderBy(l => l.Nome).ToList(); });
                    }
                    ItensCarregados = listaModificacoes.Select(p => { return new ViewLista(p.IDProduto.Value, p.Nome); }).ToList();
                    break;
                case 1:
                    if (listaProdutos == null)
                    {
                        int[] ids = { 1, 2, 3, 4 };
                        listaProdutos = await Task.Run(() => { return Produto.ListarCompleto().Where(p => !ids.Contains(p.IDProduto.Value) && p.TipoProduto.IDTipoProduto == (int)ETipoProduto.Item).OrderBy(l => l.Nome).ToList(); });
                    }
                    ItensCarregados = listaProdutos.Select(p => { return new ViewLista(p.IDProduto.Value, p.Nome); }).ToList();
                    break;
                case 2:
                    if (listaCategorias == null)
                    {
                        listaCategorias = await Task.Run(() => { return CategoriaProduto.CategoriasAtivas().OrderBy(l => l.Nome).ToList(); });
                    }
                    ItensCarregados = listaCategorias.Select(p => { return new ViewLista(p.IDCategoriaProduto, p.Nome); }).ToList();
                    break;
            }
            ltbItens1.DataSource = ItensCarregados;
            BindListaItem1();
            ptbLoad.Visible = false;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            var selecionado = ((ViewLista)ltbItens1.SelectedItem);
            if (selecionado != null && selecionado.ID != 0 && ItensCarregados.Count() > 0)
            {
                var itemSelecionado = (ViewLista)ltbItens1.SelectedItem;
                itemSelecionado.Ordem = ItensSelecionados.Count;
                ItensSelecionados.Add(itemSelecionado);

                var itemToRemove = ItensCarregados.Single(i => i.ID == itemSelecionado.ID);
                ItensCarregados.Remove(itemToRemove);
                BindListaItem1();
                BindListaItem2();
            }
        }

        private void btnAddItemTodos_Click(object sender, EventArgs e)
        {

            ItensSelecionados.AddRange(ItensCarregados);
            ItensCarregados.Clear();
            AjustarOrdem(ItensSelecionados);
            BindListaItem1();
            BindListaItem2();
        }



        private void btnRemoveItemTodos_Click(object sender, EventArgs e)
        {
            ItensCarregados.AddRange(ItensSelecionados);
            ItensSelecionados.Clear();
            BindListaItem1();
            BindListaItem2();
        }

        private void txtBuscarItem_TextChanged(object sender, EventArgs e)
        {
            ltbItens1.DataSource = ItensCarregados.Where(
                p => p.Nome.RemoveDiacritics().Contains(txtBuscarItem.Text.RemoveDiacritics())).ToList();
        }

        private void txtBuscaPainel_TextChanged(object sender, EventArgs e)
        {
            ltbPainel1.DataSource = PaineisCarregados.Where(
                p => p.Nome.RemoveDiacritics().Contains(txtBuscaPainel.Text.RemoveDiacritics())).ToList();
        }

        private void btnAddPainel_Click(object sender, EventArgs e)
        {
            if (((ViewLista)ltbPainel1.SelectedItem).ID != 0 && PaineisCarregados.Count() > 0)
            {

                var painelSelecionado = (ViewLista)ltbPainel1.SelectedItem;
                painelSelecionado.Ordem = PaineisSelecionados.Count;
                //PaineisSelecionados.Add(painelSelecionado);
                //bindSource.Insert(bindSource.Count, painelSelecionado);
                listaBindingPaineis.Add(painelSelecionado);


                var painelToRemove = PaineisCarregados.Single(i => i.ID == painelSelecionado.ID);
                PaineisCarregados.Remove(painelToRemove);

                BindListaPaineis1();

                //dgvPaineis.Invalidate();
                //dgvPaineis.DataSource = bindSource. ;
                //dgvPaineis.Refresh();
                //BindListaPaineis2();
            }
        }

        private void btnAddPainelTodos_Click(object sender, EventArgs e)
        {
            PaineisSelecionados.AddRange(PaineisCarregados);
            PaineisCarregados.Clear();
            AjustarOrdem(PaineisSelecionados);
            BindListaPaineis1();
            BindListaPaineis2();

        }


        private void btnRemovePainelTodos_Click(object sender, EventArgs e)
        {
            PaineisCarregados.AddRange(PaineisSelecionados);
            PaineisSelecionados.Clear();
            BindListaPaineis1();
            BindListaPaineis2();
        }

        private void BindListaPaineis2(int selectedRow = 0)
        {
            PaineisSelecionados = PaineisSelecionados.OrderBy(p => p.Ordem).ToList();
            listaBindingPaineis = new BindingList<ViewLista>(PaineisSelecionados);
            dgvPaineis.DataSource = listaBindingPaineis;
            if (selectedRow != 0)
            {
                dgvPaineis.Rows[selectedRow].Selected = true;
            }
        }

        private void BindListaPaineis1()
        {
            ltbPainel1.DataSource = null;
            ltbPainel1.DataSource = PaineisCarregados;
            ltbPainel1.ValueMember = "ID";
            ltbPainel1.DisplayMember = "Nome";
        }
        private void BindListaItem1()
        {

            ltbItens1.DataSource = null;
            ltbItens1.DataSource = ItensCarregados;
            ltbItens1.ValueMember = "ID";
            ltbItens1.DisplayMember = "Nome";
        }

        private void BindListaItem2(int selectedRow = 0)
        {
            ItensSelecionados = ItensSelecionados.OrderBy(p => p.Ordem).ToList();
            listaBindingItens = new BindingList<ViewLista>(ItensSelecionados);
            dgvItens.DataSource = listaBindingItens;

            if (selectedRow != 0)
            {
                dgvItens.Rows[selectedRow].Selected = true;
            }
        }

        private Boolean Validar()
        {
            String msg = "";
            decimal min, max;
            bool minParse, maxParse;

            if (txtNome.Text == "")
                msg += "Campo \"Nome\" é obrigatório. \n";

            minParse = decimal.TryParse(txtMin.Text, out min);
            if (txtMin.Text != "" && minParse == false)
                msg += "Campo \"Mínimo\" deve ser numérico. \n";

            maxParse = decimal.TryParse(txtMax.Text, out max);
            if (txtMax.Text != "" && maxParse == false)
                msg += "Campo \"Máximo\" deve ser numérico. \n";

            if (minParse && !maxParse)
                msg += "Se há um valor mínimo precisa especificar um valor máximo\n";
            else if (!minParse && maxParse)
                msg += "Se há um valor máximo precisa especificar um valor mínimo\n";
            if (minParse && maxParse)
            {
                if (min > max)
                    msg += "Mínimo não pode ser maior que o máximo\n";
                if (max < min)
                    msg += "Máximo não pode ser menor que o mínimo\n";
            }

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar() == true)
            {
                PainelModificacao1.Nome = txtNome.Text;
                PainelModificacao1.Titulo = txtTitulo.Text;
                PainelModificacao1.IDTipoItem = cbbTipoItens.SelectedIndex + 1;
                PainelModificacao1.IDValorUtilizado = cbbTabelaPrecos.SelectedIndex + 1;

                if (string.IsNullOrWhiteSpace(txtMin.Text))
                    PainelModificacao1.Minimo = null;
                else
                    PainelModificacao1.Minimo = Convert.ToInt32(txtMin.Text);

                if (string.IsNullOrWhiteSpace(txtMax.Text))
                    PainelModificacao1.Maximo = null;
                else
                    PainelModificacao1.Maximo = Convert.ToInt32(txtMax.Text);

                PainelModificacao1.PainelModificacaoOperacao = new PainelModificacaoOperacaoInformation { IDPainelModificacaoOperacao = Convert.ToInt32(cbbCalculoValor.SelectedValue) };
                PainelModificacao1.IgnorarValorItem = ckbIgnorarValor.Checked;

                int idTipoItens = Convert.ToInt16(cbbTipoItens.SelectedIndex);
                if (idTipoItens == 0 || idTipoItens == 1)
                {
                    PainelModificacao1.ListaCategoria = null;


                    PainelModificacao1.ListaProduto = ItensSelecionados
                        .Select(p => new PainelModificacaoProdutoInformation
                        {
                            Produto = new ProdutoInformation { IDProduto = p.ID },
                            Ordem = p.Ordem,
                            PainelModificacao = PainelModificacao1
                        }).ToList();
                }
                else if (idTipoItens == 2)
                {
                    PainelModificacao1.ListaProduto = null;
                    PainelModificacao1.ListaCategoria = ItensSelecionados
                        .Select(p => new PainelModificacaoCategoriaInformation
                        {
                            Categoria = new CategoriaProdutoInformation { IDCategoriaProduto = p.ID },
                            Ordem = p.Ordem,
                            PainelModificacao = PainelModificacao1
                        }).ToList();
                }

                PainelModificacao1.PaineisRelacionados = PaineisSelecionados
                    .Select(p => new PainelModificacaoRelacionadoInformation
                    {
                        PainelModificacao1 = PainelModificacao1,
                        PainelModificacao2 = new PainelModificacaoInformation
                        {
                            IDPainelModificacao = p.ID
                        }
                        //Ordem = p.Ordem
                    }).ToList();


                PainelModificacao.Salvar(PainelModificacao1);
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void OrganizarColunasItens()
        {

            dgvItens.Sort(dgvItens.Columns["Ordem"], ListSortDirection.Ascending);
        }

        private void OrganizarColunasPaineis()
        {
            dgvPaineis.Sort(dgvPaineis.Columns["Ordem"], ListSortDirection.Ascending);
        }

        private void dgvItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Int32 idPainelModificacaoProduto;
                ViewLista itemSelecionado;

                if (e.RowIndex >= 0)
                {
                    switch (dgvItens.Columns[e.ColumnIndex].Name)
                    {
                        case "Subir":
                            idPainelModificacaoProduto = Convert.ToInt32(dgvItens["IDPainelModificacaoProduto", e.RowIndex].Value);
                            itemSelecionado = ItensSelecionados.First(l => l.ID == idPainelModificacaoProduto);


                            if (itemSelecionado.Ordem != 0)
                            {
                                var ordemAux = itemSelecionado.Ordem;
                                var itemAnterior = ItensSelecionados.Where(p => p.Ordem == (ordemAux - 1)).FirstOrDefault();
                                itemSelecionado.Ordem = ordemAux - 1;
                                itemAnterior.Ordem = ordemAux;
                                BindListaItem1();
                                BindListaItem2(e.RowIndex - 1);
                            }
                            break;


                        case "Descer":
                            idPainelModificacaoProduto = Convert.ToInt32(dgvItens["IDPainelModificacaoProduto", e.RowIndex].Value);
                            itemSelecionado = ItensSelecionados.First(l => l.ID == idPainelModificacaoProduto);
                            if (itemSelecionado.Ordem != ItensSelecionados.Count() - 1)
                            {
                                var ordemAux = itemSelecionado.Ordem;
                                var proximoItem = ItensSelecionados.Where(p => p.Ordem == (ordemAux + 1)).FirstOrDefault();
                                itemSelecionado.Ordem = ordemAux + 1;
                                proximoItem.Ordem = ordemAux;
                                BindListaItem1();
                                BindListaItem2(e.RowIndex + 1);


                            }
                            break;

                        case "Remover":
                            dgvItens.ClearSelection();
                            idPainelModificacaoProduto = Convert.ToInt32(dgvItens["IDPainelModificacaoProduto", e.RowIndex].Value);
                            itemSelecionado = ItensSelecionados.First(l => l.ID == idPainelModificacaoProduto);
                            ItensSelecionados.Remove(itemSelecionado);
                            ItensCarregados.Add(itemSelecionado);
                            AjustarOrdem(ItensSelecionados);
                            BindListaItem1();
                            BindListaItem2();

                            break;
                    }
                }
            }
            catch (Exception)
            {

            }

        }



        private void dgvPaineis_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Int32 idPainelSelecionado;
                ViewLista painelSelecionado;


                if (e.RowIndex >= 0)
                {
                    idPainelSelecionado = Convert.ToInt32(dgvPaineis["IDPainelModificacaoRelacionado", e.RowIndex].Value);
                    painelSelecionado = PaineisSelecionados
                        .First(l => l.ID == idPainelSelecionado);

                    switch (dgvPaineis.Columns[e.ColumnIndex].Name)
                    {
                        case "SubirPainel":
                            if (painelSelecionado.Ordem != 0)
                            {
                                var ordemAux = painelSelecionado.Ordem;
                                var itemAnterior = PaineisSelecionados.Where(p => p.Ordem == (ordemAux - 1)).FirstOrDefault();
                                painelSelecionado.Ordem = ordemAux - 1;
                                itemAnterior.Ordem = ordemAux;
                                BindListaPaineis1();
                                BindListaPaineis2(e.RowIndex - 1);
                            }
                            break;
                        case "DescerPainel":
                            if (painelSelecionado.Ordem != PaineisSelecionados.Count() - 1)
                            {
                                var ordemAux = painelSelecionado.Ordem;
                                var proximoItem = PaineisSelecionados.Where(p => p.Ordem == (ordemAux + 1)).FirstOrDefault();
                                painelSelecionado.Ordem = ordemAux + 1;
                                proximoItem.Ordem = ordemAux;
                                BindListaPaineis1();
                                BindListaPaineis2(e.RowIndex + 1);
                            }
                            break;

                        case "RemoverPainel":
                            PaineisSelecionados.Remove(painelSelecionado);
                            PaineisCarregados.Add(painelSelecionado);
                            AjustarOrdem(PaineisSelecionados);
                            BindListaPaineis1();
                            BindListaPaineis2();
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }


        }

        /// <summary>
        /// Ordena automativamente caso tenha mais de 1 item com ordem = 0
        /// </summary>
        /// <param name="lista"></param>
        private void AjustarOrdemZerosNulo(List<ViewLista> lista)
        {
            if (lista != null)
            {
                if (lista.Where(p => p.Ordem == 0).Count() > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lista[i].Ordem = i;
                    }
                }
            }
        }

        private void AjustarOrdem(List<ViewLista> lista)
        {
            if (lista != null && lista.Count() != 0)
            {

                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Ordem = i;
                }

            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            txtTitulo.Text = txtNome.Text;
        }
    }
}
