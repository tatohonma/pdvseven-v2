using a7D.PDV.Model.BLL;
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
using a7D.PDV.BLL;
using a7D.PDV.BLL.Utils;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmImagemTemaEditar : Form
    {
        private ImagemTemaInformation ImagemTema1 { get; set; }
        private Image ImagemParaExibicao { get; set; }
        private string Extensao { get; set; }
        private Image ImagemInalterada { get; set; }

        public frmImagemTemaEditar(int idTemaCardapio)
        {
            InitializeComponent();
            ImagemTema1 = new ImagemTemaInformation
            {
                TemaCardapio = new TemaCardapioInformation { IDTemaCardapio = idTemaCardapio },
                Idioma = new IdiomaInformation { IDIdioma = 0 }
            };
        }

        public frmImagemTemaEditar(ImagemTemaInformation imagemTema) : this(imagemTema.TemaCardapio.IDTemaCardapio.Value)
        {
            ImagemTema1 = imagemTema;
        }

        private void frmImagemTemaEditar_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Anchor = AnchorStyles.None;

            var idiomas = Idioma.Listar();
            cbIdioma.DataSource = idiomas;
            cbIdioma.DisplayMember = "Nome";
            cbIdioma.ValueMember = "IDIdioma";

            txtNome.Text = ImagemTema1.Nome;
            txtDescricao.Text = ImagemTema1.Descricao;
            if (ImagemTema1.Idioma.IDIdioma == 0)
                cbIdioma.SelectedIndex = 0;
            else
                cbIdioma.SelectedValue = ImagemTema1.Idioma.IDIdioma;

            if (ImagemTema1.Imagem?.IDImagem.HasValue == true)
            {
                ImagemInalterada = new ConversorByteArrayParaImagem(ImagemTema1.Imagem.Dados).Imagem;
                Extensao = ImagemTema1.Imagem.Extensao;
                ExibirImagem();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEscolherImagem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;

            fd.Filter = "Imagens (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            var result = fd.ShowDialog();

            if (result == DialogResult.OK)
            {
                ImagemInalterada = Image.FromStream(fd.OpenFile());
                Extensao = fd.DefaultExt;
                ExibirImagem();
                if (string.IsNullOrEmpty(txtNome.Text))
                {
                    txtNome.Text = fd.SafeFileName.Split('.')[0];
                }
            }
        }

        private void ExibirImagem()
        {
            if (ImagemInalterada.Width > panel1.Width || ImagemInalterada.Height > panel1.Height)
                ImagemParaExibicao = ImageUtil.RedimensionarImagem(ImagemInalterada, panel1.Width, panel1.Height);
            else
                ImagemParaExibicao = ImagemInalterada;
            CenterPictureBox(pictureBox1, ImagemParaExibicao);
        }

        private void btnLimparImagem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tem certeza?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DisposeImagens();
            }
        }

        private void frmImagemTemaEditar_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeImagens();
        }

        private void DisposeImagens()
        {
            pictureBox1.Image = null;
            ImagemParaExibicao?.Dispose();
            ImagemParaExibicao = null;

            ImagemInalterada?.Dispose();
            ImagemInalterada = null;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                try
                {
                    ImagemTema1.Nome = txtNome.Text;
                    ImagemTema1.Descricao = txtDescricao.Text;
                    ImagemTema1.Idioma = cbIdioma.SelectedItem as IdiomaInformation;

                    if (ImagemTema1.Imagem == null || ImagemTema1.Imagem?.IDImagem.HasValue == false)
                        ImagemTema1.Imagem = Imagem.CriarNova(ImagemInalterada);
                    else
                        ImagemTema1.Imagem.AtualizarImagem(ImagemInalterada);

                    ImagemTema1.Imagem.Extensao = Extensao;
                    ImagemTema1.Imagem.Nome = txtNome.Text;
                    Imagem.Salvar(ImagemTema1.Imagem);

                    ImagemTema.Salvar(ImagemTema1);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    Logs.ErroBox(CodigoErro.E013, ex);
                }
            }
        }

        private bool Validar()
        {
            var msg = "";
            if (string.IsNullOrWhiteSpace(txtNome.Text))
                msg += "Informe o nome da imagem\n";
            if (ImagemInalterada == null)
                msg += "Selecione uma imagem\n";
            if (cbIdioma.SelectedItem as IdiomaInformation == null)
                msg += "Selecione um idioma";

            var valido = string.IsNullOrWhiteSpace(msg);
            if (!valido)
                MessageBox.Show(msg, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return valido;
        }

        private void CenterPictureBox(PictureBox picBox, Image picImage)
        {
            picBox.Image = picImage;
            picBox.Location = new Point((picBox.Parent.ClientSize.Width / 2) - (picImage.Width / 2),
                                        (picBox.Parent.ClientSize.Height / 2) - (picImage.Height / 2));
            picBox.Refresh();
        }

        private void txtNome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                e.Handled = true;
            base.OnKeyDown(e);
        }

        private void txtNome_Leave(object sender, EventArgs e)
        {
            txtNome.Text.Replace(' ', '_');
        }

        private void salvarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImagemInalterada != null)
            {
                var sfd = new SaveFileDialog();
                sfd.CheckPathExists = true;
                sfd.FileName = txtNome.Text.Replace(" ", "_");
                if (sfd.FileName.Length > 50)
                    sfd.FileName = sfd.FileName.Substring(0, 50);
                sfd.Filter = "Imagens (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

                sfd.DefaultExt = Extensao;

                var resp = sfd.ShowDialog();

                if (resp == DialogResult.OK)
                {
                    ImagemInalterada.Save(sfd.FileName);
                }
            }
        }
    }
}
