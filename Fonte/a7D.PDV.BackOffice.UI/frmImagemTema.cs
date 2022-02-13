using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using a7D.PDV.Model;
using System.Threading;
using a7D.PDV.Model.BLL;
using System.Transactions;
using a7D.PDV.BLL.Utils;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmImagemTema : Form
    {
        private TemaCardapioInformation TemaCardapio1 { get; set; }
        private List<ImagemTemaInformation> ListaImagens { get; set; }
        private Form ImgForm { get; set; }
        private PictureBox PictureBoxHover { get; set; }
        public bool CarregarNovamente { get; set; }
        public int IdIdioma { get; set; }
        private volatile bool _cancelar;
        private Thread CarregarThread;

        private string Filtro { get; set; }

        public frmImagemTema(int idTemaCardapio)
        {
            InitializeComponent();
            TemaCardapio1 = TemaCardapio.Carregar(idTemaCardapio);

            _cancelar = false;

            ListaImagens = new List<ImagemTemaInformation>();

            ImgForm = new Form();
            ImgForm.FormBorderStyle = FormBorderStyle.None;
            ImgForm.Size = new Size(800, 600);
            ImgForm.StartPosition = FormStartPosition.CenterScreen;
            ImgForm.BackColor = Color.FromArgb(0xCC, 0xCC, 0xCC);
            PictureBoxHover = new PictureBox();

            ImgForm.Controls.Add(PictureBoxHover);
            PictureBoxHover.BackColor = Color.Transparent;
            PictureBoxHover.SizeMode = PictureBoxSizeMode.AutoSize;
            PictureBoxHover.Anchor = AnchorStyles.None;
            //PictureBoxHover.Dock = DockStyle.Fill;
        }

        private void frmImagemTema_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            var idiomas = Idioma.Listar();

            idiomas.Insert(0, new IdiomaInformation { IDIdioma = 0, Nome = "Todos os idiomas" });

            cbIdiomas.DataSource = idiomas;
            cbIdiomas.ValueMember = "IDIdioma";
            cbIdiomas.DisplayMember = "Nome";

            cbIdiomas.SelectedIndexChanged += cbIdiomas_SelectedIndexChanged;

            CarregarNovamente = true;
            IdIdioma = 0;

            AtualizarImagens();
        }

        private void CarregarImagensAsync()
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() =>
                {
                    pbLoading.Show();
                }));
            else
                pbLoading.Show();

            if (CarregarNovamente)
                ListaImagens = ImagemTema.ListarPorTema(TemaCardapio1.IDTemaCardapio.Value);

            var listaFiltrada = ListaImagens;

            if (IdIdioma > 0)
                listaFiltrada = listaFiltrada.Where(i => i.Idioma.IDIdioma == IdIdioma).ToList();

            if (!string.IsNullOrWhiteSpace(Filtro))
                listaFiltrada = listaFiltrada.Where(i => i.Nome.ToLowerInvariant().Contains(Filtro)).ToList();

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    dgvPrincipal.Rows.Clear();
                }));
            }
            else
            {
                dgvPrincipal.Rows.Clear();
            }

            foreach (var it in listaFiltrada)
            {
                if (_cancelar)
                    break;
                var linha = new object[] { it.IDImagemTema, null, it.Nome, it.Idioma?.Nome };
                it.Imagem = Imagem.CarregarCompleto(it.Imagem.IDImagem.Value);

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        InserirLinha(linha, it.Idioma, it.Imagem);
                    }));
                }
                else
                    InserirLinha(linha, it.Idioma, it.Imagem);
            }

            if (InvokeRequired)
                BeginInvoke(new Action(() =>
                {
                    pbLoading.Hide();
                }));
            else
                pbLoading.Hide();
        }

        private void CenterPictureBox(PictureBox picBox, Image picImage)
        {
            picBox.Image = picImage;
            picBox.Location = new Point((picBox.Parent.ClientSize.Width / 2) - (picImage.Width / 2),
                                        (picBox.Parent.ClientSize.Height / 2) - (picImage.Height / 2));
            picBox.Refresh();
        }

        private void InserirLinha(object[] linha, IdiomaInformation idioma, ImagemInformation imagem)
        {
            try
            {
                dgvPrincipal.Rows.Add(linha);
                var row = dgvPrincipal.Rows[dgvPrincipal.Rows.Count - 1];

                var img = row.Cells["clImagem"] as DataGridViewImageCell;

                if (imagem.Dados?.Length > 0 && img != null)
                {
                    img.Value = new ConversorByteArrayParaImagem(imagem.Dados).Imagem;
                }
            }
            catch { }
        }

        private void dgvPrincipal_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                PictureBoxHover.Image = null;
                ImgForm.Hide();
            }
        }

        private void frmImagemTema_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancelar = true;
            foreach (DataGridViewRow row in dgvPrincipal.Rows)
            {
                var img = row.Cells["clImagem"] as DataGridViewImageCell;
                var imagem = img.Value as Image;
                imagem?.Dispose();
            }
        }

        private void AtualizarImagens()
        {
            _cancelar = true;
            while (CarregarThread?.IsAlive == true) { }
            _cancelar = false;
            CarregarThread = new Thread(CarregarImagensAsync);
            CarregarThread.Start();
            while (!CarregarThread.IsAlive) { }
        }

        private void dgvPrincipal_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var cell = dgvPrincipal.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var image = (Image)cell.Value;
                if (image != null)
                {
                    if (image.Width > ImgForm.Width || image.Height > ImgForm.Height)
                        image = ImageUtil.RedimensionarImagem(image, ImgForm.Width, ImgForm.Height);

                    CenterPictureBox(PictureBoxHover, image);
                    ImgForm.Show();
                }
            }
        }

        private void cbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            IdIdioma = (cbIdiomas.SelectedItem as IdiomaInformation).IDIdioma.Value;
            CarregarNovamente = false;
            AtualizarImagens();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            Alterar();
        }

        private void Alterar()
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que deseja alterar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                var result = new frmImagemTemaEditar(ListaImagens.FirstOrDefault(i => i.IDImagemTema == Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells[0].Value))).ShowDialog();
                CarregarNovamente = result == DialogResult.OK;
                AtualizarImagens();
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            var result = new frmImagemTemaEditar(TemaCardapio1.IDTemaCardapio.Value).ShowDialog();
            CarregarNovamente = result == DialogResult.OK;
            AtualizarImagens();
        }

        private void dgvPrincipal_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Alterar();
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            Filtro = txtFiltro.Text.ToLowerInvariant();
            CarregarNovamente = false;
            AtualizarImagens();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvPrincipal.SelectedRows.Count < 1)
                MessageBox.Show("Selecione o registro que deseja excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                if (MessageBox.Show("Deseja realmente excluir esta imagem?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {

                        var imagemTema = ListaImagens.FirstOrDefault(i => i.IDImagemTema == Convert.ToInt32(dgvPrincipal.SelectedRows[0].Cells[0].Value));
                        ImagemTema.Excluir(imagemTema);
                        Imagem.Excluir(imagemTema.Imagem);

                        ListaImagens.Remove(imagemTema);

                        AtualizarImagens();
                    }
                    catch (Exception ex)
                    {
                        Logs.ErroBox(CodigoErro.E013, ex);
                    }
                }
            }
        }

        private void baixarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = dgvPrincipal.SelectedRows[0];
            if (row != null && row.Index != -1)
            {
                var imagemTema = ListaImagens.FirstOrDefault(i => i.IDImagemTema == Convert.ToInt32(row.Cells[0].Value));
                var sfd = new SaveFileDialog();
                sfd.CheckPathExists = true;
                sfd.FileName = imagemTema.Nome.Replace(" ", "_");
                if (sfd.FileName.Length > 50)
                    sfd.FileName = sfd.FileName.Substring(0, 50);
                sfd.Filter = "Imagens (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

                sfd.DefaultExt = imagemTema.Imagem.Extensao;

                var resp = sfd.ShowDialog();

                if (resp == DialogResult.OK)
                {
                    using (var img = new ConversorByteArrayParaImagem(imagemTema.Imagem.Dados).Imagem)
                    {
                        img.Save(sfd.FileName);
                    }
                }
            }
        }

        private void dgvPrincipal_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == MouseButtons.Right)
            {
                var c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                if (!c.Selected)
                {
                    c.DataGridView.ClearSelection();
                    c.DataGridView.CurrentCell = c;
                    c.Selected = true;
                }
            }
        }
    }
}
