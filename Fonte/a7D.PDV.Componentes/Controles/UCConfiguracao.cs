using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public partial class UCConfiguracao : UserControl
    {
        private ConfiguracaoBDInformation ConfigPadrao { get; set; } = null;
        private ConfiguracaoBDInformation Config { get; set; } = null;

        private Action<string> AlterarValor { get; set; }
        private Action<string> AlterarValorDebounced { get; set; }

        private int IDPDV { get; } = -1;

        private Control control;

        public UCConfiguracao()
        {
            InitializeComponent();
            AlterarValor = new Action<string>(valor =>
            {
                if (string.IsNullOrWhiteSpace(valor) && ConfigPadrao.Obrigatorio == true)
                {
                    if (control is TextBoxBase)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            var tBase = (TextBoxBase)control;
                            if (tBase.CanUndo)
                            {
                                if (control is ComboBox cbb)
                                {
                                    cbb.SelectedIndexChanged += null;
                                }
                                else if (control is TextBox txt)
                                {
                                    txt.TextChanged += null;
                                }


                                tBase.Undo();
                                if (control is ComboBox cbb1)
                                {
                                    cbb1.SelectedIndexChanged += ComboBoxChange;
                                }
                                else if (control is TextBox txt1)
                                {
                                    txt1.TextChanged += TextChange;
                                }
                            }
                        }));
                    }
                    MessageBox.Show("Esta configuração é obrigatória", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (IDPDV < 0)
                {
                    ConfigPadrao.Valor = valor;
                    ConfiguracaoBD.Salvar(ConfigPadrao);
                    if (ConfigPadrao.TipoPDV?.IDTipoPDV != null)
                    {
                        var configuracoesDiferentes =
                            ConfiguracaoBD.ListarConfiguracoesPorTipoEChave(ConfigPadrao.TipoPDV.IDTipoPDV, ConfigPadrao.Chave)
                                .Where(c => c.Valor != ConfigPadrao.Valor);
                        var diferentes = string.Join("\n", configuracoesDiferentes.Select(c => $"({c.PDV?.IDPDV}){c.PDV?.Nome}: {(c.ValoresAceitos == "0|1" ? (c.Valor == "1" ? "Ligado" : "Desligado") : c.Valor)}"));
                        if (!string.IsNullOrWhiteSpace(diferentes))
                        {
                            if (MessageBox.Show($"Os seguintes PDVs tem configurações diferentes:\n{diferentes}\nDeseja aplicar o valor padrão para estes PDVs?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                foreach (var config in configuracoesDiferentes)
                                {
                                    ConfiguracaoBD.Excluir(config);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var construir = false;
                    if (Config == null)
                    {
                        Config = new ConfiguracaoBDInformation
                        {
                            Chave = ConfigPadrao.Chave,
                            Valor = valor,
                            Titulo = ConfigPadrao.Titulo,
                            ValoresAceitos = ConfigPadrao.ValoresAceitos,
                            Obrigatorio = ConfigPadrao.Obrigatorio,
                            PDV = new PDVInformation { IDPDV = IDPDV },
                            TipoPDV = ConfigPadrao.TipoPDV
                        };
                        construir = true;
                    }
                    else
                    {
                        Config.Valor = valor;
                    }
                    ConfiguracaoBD.Salvar(Config);
                    if (construir)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            ConstruirValor();
                        }));
                    }
                }
            });
            AlterarValorDebounced = Debounce(AlterarValor);
        }

        public UCConfiguracao(ConfiguracaoBDInformation config, int idPDV = -1) : this()
        {
            ConfigPadrao = config;
            if (idPDV > 0)
            {
                IDPDV = idPDV;
                Config = ConfiguracaoBD.BuscarConfiguracao(config.Chave, config.TipoPDV.IDTipoPDV, idPDV);
            }
        }

        private void UCConfiguracao_Load(object sender, EventArgs e)
        {
            var config = Config ?? ConfigPadrao;
            lblChave.Text = string.IsNullOrWhiteSpace(config.Titulo) ? config.Chave : config.Titulo;
            ConstruirValor();
        }

        private void TextChange(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            AlterarValorDebounced(txt.Text);
        }

        private void SwitchChange(object sender, EventArgs e)
        {
            var sw = sender as Switch;
            AlterarValor(sw.Checked ? "1" : "0");
        }

        private void ComboBoxChange(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            var valorSelecionado = cb.SelectedItem as string;
            var titulos = ConfigPadrao.ListaTitulosAceitos().ToList();
            var indice = titulos.FindIndex(v => string.Compare(v, valorSelecionado, true) == 0);
            string valor = default(string);
            if (indice >= 0)
                valor = ConfigPadrao.ListaValoresAceitos().ToList()[indice];
            else
                valor = string.Empty;
            AlterarValor(valor);
        }

        private static Action<T> Debounce<T>(Action<T> func, int milliseconds = 500)
        {
            var last = 0;
            return arg =>
            {
                var current = Interlocked.Increment(ref last);
                Task.Delay(milliseconds).ContinueWith(task =>
                {
                    if (current == last) func(arg);
                    task.Dispose();
                });
            };
        }

        private void ConstruirValor()
        {
            var config = Config ?? ConfigPadrao;

            if (control != null)
                tbLayout.Controls.Remove(control);

            if (config.ConfiguracaoSistema() | (config.ConfiguracaoPadrao() && IDPDV > 0))
                llAlterar.Visible = false;
            else if (config.ConfiguracaoPadrao() & IDPDV < 0)
            {
                llAlterar.Text = "Aplicar padrão";
                llAlterar.LinkClicked += (sender, e) =>
                {
                    if (MessageBox.Show("Aplicar configuração padrão para os demais PDVs?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (var c in ConfiguracaoBD.ListarConfiguracoesPorTipoEChave(ConfigPadrao.TipoPDV.IDTipoPDV, ConfigPadrao.Chave))
                        {
                            if (!c.ConfiguracaoPadrao())
                                ConfiguracaoBD.Excluir(c);
                        }
                    }
                };
            }
            else
            {
                llAlterar.Visible = true;
                llAlterar.LinkClicked += (sender, e) =>
                {
                    if (Config != null)
                    {
                        if (MessageBox.Show($"Utilizar configuração padrão para {(string.IsNullOrWhiteSpace(Config.Titulo) ? Config.Chave : Config.Titulo)}?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            ConfiguracaoBD.Excluir(Config);
                            Config = null;
                            ConstruirValor();
                        }
                    }
                };
            }

            var valoresAceitos = config.ListaValoresAceitos();

            if (valoresAceitos.Count() == 0)
            {
                var txtValor = new TextBox
                {
                    Text = config.Valor
                };

                tbLayout.Controls.Add(txtValor, 1, 0);
                txtValor.Dock = DockStyle.Fill;

                txtValor.TextChanged += TextChange;
                control = txtValor;
                return;
            }

            if (config.ValoresAceitos == "0|1")
            {
                var sw = new Switch();
                sw.Width = 60;
                sw.Checked = config.Valor == "1";
                tbLayout.Controls.Add(sw, 1, 0);
                sw.CheckedChanged += SwitchChange;
                sw.Dock = DockStyle.Left;
                control = sw;
                return;
            }


            var listaAceitos = valoresAceitos.ToList();
            var titulosAceitos = config.ListaTitulosAceitos().ToList();
            var listaExibicao = new List<string>(titulosAceitos);

            if (config.Obrigatorio == false)
                listaExibicao.Insert(0, "");

            var selectedIndex = -1;

            if (!string.IsNullOrWhiteSpace(config.Valor))
            {
                selectedIndex = listaAceitos.IndexOf(config.Valor);
            }

            if (config.Obrigatorio == false)
                selectedIndex++;

            var cbb = new ComboBox
            {
                DataSource = listaExibicao.ToArray(),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            tbLayout.Controls.Add(cbb, 1, 0);

            cbb.Dock = DockStyle.Fill;
            cbb.SelectedIndex = selectedIndex;

            cbb.SelectedIndexChanged += ComboBoxChange;
            control = cbb;
        }

        private void pbUsarPadrao_Click(object sender, EventArgs e)
        {

        }

        private void lblChave_MouseEnter(object sender, EventArgs e)
        {
            //var config = Config ?? ConfigPadrao;
            //if (!string.IsNullOrWhiteSpace(config.Titulo))
            //    lblChave.Text = config.Chave;
        }

        private void lblChave_MouseLeave(object sender, EventArgs e)
        {
            //var config = Config ?? ConfigPadrao;
            //lblChave.Text = string.IsNullOrWhiteSpace(config.Titulo) ? config.Chave : config.Titulo;
        }
    }
}
