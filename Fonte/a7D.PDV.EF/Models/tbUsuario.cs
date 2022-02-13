using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public class tbUsuario : IERP, IERPSync
    {
        public tbUsuario()
        {
            this.tbCaixas = new List<tbCaixa>();
            this.tbFechamentoes = new List<tbFechamento>();
            this.tbPedidoes = new List<tbPedido>();
            this.tbPedidoes1 = new List<tbPedido>();
            this.tbPedidoes2 = new List<tbPedido>();
            this.tbPedidoProdutoes = new List<tbPedidoProduto>();
            this.tbPedidoProdutoes1 = new List<tbPedidoProduto>();
            this.tbPedidoProdutoes2 = new List<tbPedidoProduto>();
        }

        public int IDUsuario { get; set; }
        public string CodigoERP { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool PermissaoAdm { get; set; }
        public bool PermissaoCaixa { get; set; }
        public bool PermissaoGarcom { get; set; }
        public bool PermissaoGerente { get; set; }

        public int DireitosCaixa { get; set; }
        public int DireitosBackOffice { get; set; }

        public bool Ativo { get; set; }
        public DateTime DtUltimaAlteracao { get; set; }
        public bool Excluido { get; set; }
        public virtual ICollection<tbCaixa> tbCaixas { get; set; }
        public virtual ICollection<tbFechamento> tbFechamentoes { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes1 { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes2 { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes1 { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes2 { get; set; }

        public bool RequerAlteracaoERP(DateTime dtSync) => DtUltimaAlteracao > dtSync;

        public int myID() => IDUsuario;

        public override string ToString() => $"{IDUsuario}: {Nome}";
    }
}
