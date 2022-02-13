using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public partial class Categorias : UserControl
    {
        public event EventHandler CategoriaSelecionada;
        public bool ListarCreditos { get; set; }

        private List<BotaoItem> itens;
        private int botoesPorLinhaAtual = 0;

        public Categorias()
        {
            InitializeComponent();
        }

        public void ListarCategorias()
        {
            itens = CategoriaProduto
                .Listar()
                .OrderBy(l => l.Nome)
                .Select(i => new BotaoItem(i.IDCategoriaProduto.Value, i.Nome, i.Disponibilidade == true, i))
                .ToList();

            if (Produto.ContarSemCategoria(ListarCreditos) > 0)
                itens.Insert(0, new BotaoItem(0, "SEM CATEGORIA", true, null));

            Categorias_Resize(null, null);

            if (Controls.Count > 0)
            {
                ((RadioButton)Controls[0]).Checked = true;
                btn_CheckedChanged(Controls[0], null);
            }
        }

        private void btn_CheckedChanged(object sender, EventArgs e)
        {
            var rdb = (RadioButton)sender;
            if (rdb.Checked)
                CategoriaSelecionada(sender, e);
        }

        private void Categorias_Resize(object sender, EventArgs e)
        {
            if (DesignMode || itens == null)
                return;

            botoesPorLinhaAtual = 0;
            BotaoGrid.CriaBotoes(this, itens, btn_CheckedChanged, ref botoesPorLinhaAtual, BotaoGrid.Azul, false);
        }
    }
}
