using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.MigracaoImagens.UI
{
    public partial class frmMigrarProdutos : Form
    {
        public frmMigrarProdutos()
        {
            InitializeComponent();
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
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCaminho.Text))
                MessageBox.Show("Selecione o diretório das imagens", "Atenção", MessageBoxButtons.OK);

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

                foreach (var file in d.GetFiles("*.png"))
                {
                    AtualizarProgresso(++atual, total);
                    if (file.Name.Contains("_thumb"))
                        continue;
                    using (var img = Image.FromFile(file.FullName))
                    {
                        try
                        {
                            var idProduto = Convert.ToInt32(file.Name.Substring(0, file.Name.IndexOf('.')));
                            var produto = Produto.Carregar(idProduto);
                            if (produto.Nome == null)
                                continue;
                            var produtoImagem = ProdutoImagem.CarregarPorProduto(idProduto);

                            if (produtoImagem.IDProdutoImagem.HasValue == false)
                                produtoImagem = new ProdutoImagemInformation { Produto = produto, Imagem = new ImagemInformation() };

                            var data = new ConversorImagemParaByteArray(img).Dados;

                            var imagem = produtoImagem.Imagem;
                            imagem.Dados = data;
                            imagem.Nome = produto.Nome;
                            imagem.Extensao = "png";
                            imagem.Largura = img.Width;
                            imagem.Altura = img.Height;
                            imagem.Tamanho = data.Length;

                            produto.DtUltimaAlteracao = DateTime.Now;

                            Produto.Salvar(produto);

                            Imagem.Salvar(imagem);

                            ProdutoImagem.Salvar(produtoImagem);
                        }
                        catch (FormatException)
                        {
                            continue;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
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

        private void frmMigrar_Load(object sender, EventArgs e)
        {
            string strDir = @"C:\PDV7\backOffice\ImagensProdutos";
            if (Directory.Exists(strDir))
            {
                txtCaminho.Text = strDir;
                VerificarConnectionString();
            }
        }

        private void VerificarConnectionString()
        {
            var caminhoBackoffice = txtCaminho.Text.Substring(0, txtCaminho.Text.LastIndexOf('\\')) + @"\a7D.PDV.BackOffice.UI.exe";
            if (File.Exists(caminhoBackoffice) == false)
            {
                caminhoBackoffice = ProcurarBackoffice();
                if (string.IsNullOrWhiteSpace(caminhoBackoffice))
                    return;
            }

            var configBackOffice = ConfigurationManager.OpenExeConfiguration(caminhoBackoffice);

            var connectionStringBackoffice = configBackOffice.ConnectionStrings.ConnectionStrings["connectionString"];
            if (connectionStringBackoffice == null)
            {
                MessageBox.Show("ConnectionString do Backoffice não encontrada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var connectionStringAtual = ConfigurationManager.ConnectionStrings["connectionString"];

            if (connectionStringAtual == null || connectionStringBackoffice.ConnectionString != connectionStringAtual.ConnectionString)
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
            if (connectionStringAtual == null)
            {
                MessageBox.Show("Nenhuma conexão ao banco configurada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            if (new VerificarTabelaImagem(connectionStringAtual.ConnectionString).Existe == false)
            {
                MessageBox.Show("Tabela de Imagens (tbImagem) não encontrada. Verifique se o banco de dados foi atualizado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private string ProcurarBackoffice()
        {
            var fd = new OpenFileDialog();
            fd.InitialDirectory = txtCaminho.Text;
            fd.CheckFileExists = true;
            fd.Filter = "Backoffice|a7D.PDV.BackOffice.UI.exe; *.exe";
            fd.Title = "Informe o local de instalação do Backoffice";
            fd.ShowDialog();

            if (string.IsNullOrWhiteSpace(fd.FileName))
            {
                var resp = MessageBox.Show("Backoffice não encontrado. Deseja procurar novamente?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (resp == DialogResult.No)
                {
                    var resp2 = MessageBox.Show("O BackOffice é necessário para verificar se o banco de dados em que as imagens serão inseridas está correto. Tem certeza que deseja pular esta etapa?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp2 == DialogResult.No)
                        return ProcurarBackoffice();
                    else
                        return string.Empty;
                }
                return ProcurarBackoffice();
            }
            return fd.FileName;
        }
    }
}
