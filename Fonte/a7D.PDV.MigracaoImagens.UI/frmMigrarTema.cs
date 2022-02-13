using a7D.PDV.BLL;
using a7D.PDV.Model;
using a7D.PDV.Model.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;

namespace a7D.PDV.MigracaoImagens.UI
{
    public partial class frmMigrarTema : Form
    {
        public frmMigrarTema()
        {
            InitializeComponent();
        }

        private void frmMigrarTema_Load(object sender, EventArgs e)
        {
            var temas = TemaCardapio.Listar();
            if (temas == null)
                temas = new List<TemaCardapioInformation>();

            temas.Insert(0, new TemaCardapioInformation { IDTemaCardapio = -1, Nome = "Selecione..." });

            ddlTema.ValueMember = "IDTemaCardapio";
            ddlTema.DisplayMember = "Nome";
            ddlTema.DataSource = temas;

            var idiomas = Idioma.Listar();
            if (idiomas == null)
                idiomas = new List<IdiomaInformation>();

            idiomas.Insert(0, new IdiomaInformation { IDIdioma = -1, Nome = "Selecione..." });

            ddlIdioma.ValueMember = "IDIdioma";
            ddlIdioma.DisplayMember = "Nome";
            ddlIdioma.DataSource = idiomas;

            string strDir = @"C:\PDV7\www\tema";
            if (Directory.Exists(strDir))
            {
                txtCaminho.Text = strDir;
                VerificarConnectionString();
            }
        }

        private void btnSelecionarPasta_Click(object sender, EventArgs e)
        {
            fbdDiretorio.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdDiretorio.ShowNewFolderButton = false;
            fbdDiretorio.ShowDialog();
            if (string.IsNullOrWhiteSpace(fbdDiretorio.SelectedPath) == false
                && Directory.Exists(fbdDiretorio.SelectedPath)
                && fbdDiretorio.SelectedPath != txtCaminho.Text)
            {
                txtCaminho.Text = fbdDiretorio.SelectedPath;
                VerificarConnectionString();
            }
            else
                txtCaminho.Text = string.Empty;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCaminho.Text))
            {
                MessageBox.Show("Selecione o diretório do tema", "Atenção", MessageBoxButtons.OK);
                return;
            }

            if(Convert.ToInt32(ddlTema.SelectedValue) <= 0)
            {
                MessageBox.Show("Selecione o Tema", "Atenção", MessageBoxButtons.OK);
                return;
            }

            if (Convert.ToInt32(ddlIdioma.SelectedValue) <= 0)
            {
                MessageBox.Show("Selecione o idioma", "Atenção", MessageBoxButtons.OK);
                return;
            }

            if (Directory.Exists(txtCaminho.Text))
            {
                DirectoryInfo d = new DirectoryInfo(txtCaminho.Text);
                float total = d.GetFiles("*.png").Length;
                float atual = 0f;

                btnIniciar.Enabled = false;
                btnSelecionarPasta.Enabled = false;
                txtCaminho.Enabled = false;
                UseWaitCursor = true;

                lblProgresso.Show();

                pbarProgresso.Show();
                pbarProgresso.Value = 0;
                pbarProgresso.Maximum = Convert.ToInt32(total);
                pbarProgresso.Step = 1;

                var temaCardapio = TemaCardapio.Carregar(Convert.ToInt32(ddlTema.SelectedValue));
                var idioma = Idioma.Carregar(Convert.ToInt32(ddlIdioma.SelectedValue));

                foreach (var file in d.GetFiles("*.png"))
                {
                    AtualizarProgresso(++atual, total);
                    if (file.Name.Contains("_thumb"))
                        continue;
                    using (var img = Image.FromFile(file.FullName))
                    {
                        var nome = file.Name.Substring(0, file.Name.IndexOf('.'));
                        var imagemTema = ImagemTema.CarregarPorNome(nome, temaCardapio.IDTemaCardapio.Value, idioma.IDIdioma.Value);

                        if (imagemTema == null || !imagemTema.IDImagemTema.HasValue)
                            imagemTema = new ImagemTemaInformation {
                                Imagem = new ImagemInformation(),
                                TemaCardapio = temaCardapio,
                                Idioma = idioma
                            };

                        var data = new ConversorImagemParaByteArray(img).Dados;

                        var imagem = imagemTema.Imagem;
                        imagem.Dados = data;
                        imagem.Nome = nome;
                        imagem.Extensao = "png";
                        imagem.Largura = img.Width;
                        imagem.Altura = img.Height;
                        imagem.Tamanho = data.Length;

                        Imagem.Salvar(imagemTema.Imagem);

                        imagemTema.Nome = imagem.Nome;

                        ImagemTema.Salvar(imagemTema);
                    }
                }
                MessageBox.Show("Migração concluída com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("O diretório não existe", "Erro", MessageBoxButtons.OK);

            lblProgresso.Hide();
            pbarProgresso.Hide();

            txtCaminho.Enabled = true;
            btnSelecionarPasta.Enabled = true;
            btnIniciar.Enabled = true;
            UseWaitCursor = false;
        }

        private void AtualizarProgresso(float atual, float total)
        {
            lblProgresso.Text = string.Format("{0} de {1} ({2}%)", atual, total, ((atual / total) * 100f).ToString("n2"));
            lblProgresso.Refresh();
            pbarProgresso.PerformStep();
        }

        private void VerificarConnectionString()
        {
            var caminhoWebService = txtCaminho.Text.Substring(0, txtCaminho.Text.LastIndexOf('\\'));

            if (Directory.Exists(caminhoWebService) == false)
            {
                caminhoWebService = ProcurarWebService();
                if (string.IsNullOrWhiteSpace(caminhoWebService))
                    return;
            }

            VirtualDirectoryMapping vdm = new VirtualDirectoryMapping(caminhoWebService, true);
            WebConfigurationFileMap wcfm = new WebConfigurationFileMap();
            wcfm.VirtualDirectories.Add("/", vdm);

            var configWebService = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");

            var connectionStringBackoffice = configWebService.ConnectionStrings.ConnectionStrings["connectionString"];
            if (connectionStringBackoffice == null)
            {
                MessageBox.Show("ConnectionString do WebService (www) não encontrada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var connectionStringAtual = ConfigurationManager.ConnectionStrings["connectionString"];

            if (connectionStringBackoffice.ConnectionString != connectionStringAtual.ConnectionString)
            {
                var resp = MessageBox.Show("As configurações de banco de dados divergem. Deseja atualizar a configuração atual?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    config.ConnectionStrings.ConnectionStrings.Remove("connectionString");
                    config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("connectionString", connectionStringBackoffice.ConnectionString, connectionStringBackoffice.ProviderName));

                    config.Save(ConfigurationSaveMode.Modified);

                    ConfigurationManager.RefreshSection("ConnectionStrings");
                    MessageBox.Show("Configurações atualizadas. Por favor inicie o programa novamente", "Sucesso", MessageBoxButtons.OK);
                    Application.Exit();
                    return;
                }
            }
            if (new VerificarTabelaImagem(connectionStringAtual.ConnectionString).Existe == false)
            {
                MessageBox.Show("Tabela de Imagens (tbImagem) não encontrada. Verifique se o banco de dados foi atualizado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private string ProcurarWebService()
        {
            var fd = new FolderBrowserDialog();
            fd.RootFolder = Environment.SpecialFolder.MyComputer;
            fd.ShowDialog();

            if (string.IsNullOrWhiteSpace(fd.SelectedPath))
            {
                var resp = MessageBox.Show("WebService (www) não encontrado. Deseja procurar novamente?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (resp == DialogResult.No)
                {
                    var resp2 = MessageBox.Show("O WebService (www) é necessário para verificar se o banco de dados em que as imagens serão inseridas está correto. Tem certeza que deseja pular esta etapa?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp2 == DialogResult.No)
                        return ProcurarWebService();
                    else
                        return string.Empty;
                }
                return ProcurarWebService();
            }
            return fd.SelectedPath;
        }

        private void txtCaminho_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
