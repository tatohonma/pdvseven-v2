using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using System;
using System.Collections.Generic;

namespace a7D.PDV.Model
{
    [Serializable]
    public class PDVInformation
    {
        public PDVInformation()
        {
            //this.tbAcaos = new List<tbAcao>();
            this.tbCaixas = new List<tbCaixa>();
            this.tbConfiguracaoBDs = new List<tbConfiguracaoBD>();
            this.tbFechamentoes = new List<tbFechamento>();
            //this.tbIntegracaoSATs = new List<tbIntegracaoSAT>();
            this.tbPedidoProdutoes = new List<tbPedidoProduto>();
            this.tbPedidoProdutoes1 = new List<tbPedidoProduto>();
            this.tbTemaCardapioPDVs = new List<tbTemaCardapioPDV>();
        }

        public int? IDPDV { get; set; }

        public string Nome { get; set; }

        public string Dados { get; set; }

        //public virtual ICollection<tbAcao> tbAcaos { get; set; }
        public virtual ICollection<tbCaixa> tbCaixas { get; set; }
        public virtual ICollection<tbConfiguracaoBD> tbConfiguracaoBDs { get; set; }
        public virtual ICollection<tbFechamento> tbFechamentoes { get; set; }
        //public virtual ICollection<tbIntegracaoSAT> tbIntegracaoSATs { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes1 { get; set; }
        public virtual ICollection<tbTemaCardapioPDV> tbTemaCardapioPDVs { get; set; }

        public TipoPDVInformation TipoPDV { get; set; }
        public string ChaveHardware { get; set; }
        public DateTime? UltimoAcesso { get; set; }
        public DateTime? UltimaAlteracao { get; set; }
        public bool? Ativo { get; set; }
        public string Versao { get; set; }

        public override string ToString()
        {
            return $"{IDPDV}: {Ativo} - {(TipoPDV == null ? "?" : TipoPDV.Tipo.ToString())} H:{ChaveHardware} V:{Versao} - {Nome}";
        }

        public bool AutoFechamento()
        {
            return TipoPDV.Tipo == ETipoPDV.POS_INTEGRADO_NTK
                || TipoPDV.Tipo == ETipoPDV.POS_INTEGRADO_STONE
                || TipoPDV.Tipo == ETipoPDV.AUTOATENDIMENTO
                || TipoPDV.Tipo == ETipoPDV.IFOOD;
        }
    }
}
