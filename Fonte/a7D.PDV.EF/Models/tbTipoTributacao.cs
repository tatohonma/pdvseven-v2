using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoTributacao
    {
        public tbTipoTributacao()
        {
            this.tbClassificacaoFiscals = new List<tbClassificacaoFiscal>();
        }

        public int IDTipoTributacao { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string CFOP { get; set; }
        public string ICMS00_Orig { get; set; }
        public string ICMS00_CST { get; set; }
        public string ICMS00_pICMS { get; set; }
        public string ICMS40_Orig { get; set; }
        public string ICMS40_CST { get; set; }
        public string ICMSSN102_Orig { get; set; }
        public string ICMSSN102_CSOSN { get; set; }
        public string ICMSSN900_Orig { get; set; }
        public string ICMSSN900_CSOSN { get; set; }
        public string ICMSSN900_pICMS { get; set; }
        public string PISAliq_CST { get; set; }
        public string PISAliq_pPIS { get; set; }
        public string PISQtde_CST { get; set; }
        public string PISQtde_vAliqProd { get; set; }
        public string PISNT_CST { get; set; }
        public string PISSN_CST { get; set; }
        public string PISOutr_CST { get; set; }
        public string PISOutr_pPIS { get; set; }
        public string PISOutr_vAliqProd { get; set; }
        public string PISST_pPIS { get; set; }
        public string PISST_vAliqProd { get; set; }
        public string COFINSAliq_CST { get; set; }
        public string COFINSAliq_pCOFINS { get; set; }
        public string COFINSQtde_CST { get; set; }
        public string COFINSQtde_vAliqProd { get; set; }
        public string COFINSNT_CST { get; set; }
        public string COFINSSN_CST { get; set; }
        public string COFINSOutr_CST { get; set; }
        public string COFINSOutr_pCOFINS { get; set; }
        public string COFINSOutr_vAliqProd { get; set; }
        public string COFINSST_pCOFINS { get; set; }
        public string COFINSST_vAliqProd { get; set; }
        public string ISSQN_vDeducISSQN { get; set; }
        public string ISSQN_vAliq { get; set; }
        public string ISSQN_cListServ { get; set; }
        public string ISSQN_cServTribMun { get; set; }
        public string ISSQN_cNatOp { get; set; }
        public string ISSQN_indIncFisc { get; set; }
        public string vItem12741 { get; set; }
        public virtual ICollection<tbClassificacaoFiscal> tbClassificacaoFiscals { get; set; }
    }
}
