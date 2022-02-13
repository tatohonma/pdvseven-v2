using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public class BotaoItem
    {
        public int ID;
        public string Name;
        public bool Enable;
        public object Tag;

        public BotaoItem(int id, string nome, bool ativo, object tag)
        {
            this.ID = id;
            this.Name = nome;
            this.Enable = ativo;
            this.Tag = tag;
        }
    }

    public static class BotaoGrid
    {
        public static readonly Color Azul = Color.FromArgb(0, 192, 192);
        public static readonly Color Cinza = Color.FromArgb(162, 175, 180);

        public static int Altura { get; set; } = 50;
        public static int Largura { get; set; } = 100;
        public static int TamanhoFonte { get; set; } = 11;

        public static void Configure(string touchParametros)
        {
            if (string.IsNullOrEmpty(touchParametros))
                return;

            var er = new Regex(@"(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*");
            var m = er.Match(touchParametros);
            if (m.Success)
            {
                if (m.Groups.Count > 4)
                    Produtos.TempoCache = int.Parse(m.Groups[4].Value);
                if (m.Groups.Count > 3)
                    Altura = int.Parse(m.Groups[3].Value);
                if (m.Groups.Count > 2)
                    Largura = int.Parse(m.Groups[2].Value);
                if (m.Groups.Count > 1)
                    TamanhoFonte = int.Parse(m.Groups[1].Value);
            }
        }

        public static void CriaBotoes(UserControl ctrl, IEnumerable<BotaoItem> itens, EventHandler click, ref int botoesPorLinhaAtual, Color background, bool button)
        {

            int botoesPorLinha = (int)Math.Floor((decimal)ctrl.Width / Largura);

            if (botoesPorLinha < 1)
                botoesPorLinha = 1;

            if (botoesPorLinhaAtual == botoesPorLinha)
                return;

            botoesPorLinhaAtual = botoesPorLinha;
            ctrl.Controls.Clear();
            int i = 0;
            foreach (var item in itens)
            {
                int linha = (Int32)Math.Floor(i / (decimal)botoesPorLinha);
                ButtonBase btn;
                var point = new Point(Largura * i - (botoesPorLinha * Largura * linha), Altura * linha);
                if (button)
                    btn = NovoBotao<Button>(item, point, background);
                else
                {
                    btn = NovoBotao<RadioButton>(item, point, background);
                    ((RadioButton)btn).Appearance = Appearance.Button;
                }
                btn.Font = new Font(ctrl.Font.Name, TamanhoFonte, FontStyle.Regular, GraphicsUnit.Point);
                btn.Click += click;
                ctrl.Controls.Add(btn);
                i++;
            }
        }

        private static TButtonBase NovoBotao<TButtonBase>(BotaoItem btn, Point position, Color background) where TButtonBase : ButtonBase, new()
        {
            var rdb = new TButtonBase()
            {
                Name = "btn_" + btn.ID,
                Text = btn.Name,
                Enabled = btn.Enable,
                Tag = btn.Tag,
                Width = Largura,
                Height = Altura,
                Location = position,
                BackColor = background,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = false
            };
            return rdb;
        }
    }
}
