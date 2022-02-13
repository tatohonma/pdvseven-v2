using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoTributacao")]
    [Serializable]
    public class TipoTributacaoInformation
    {
        [CRUDParameterDAL(true, "IDTipoTributacao")]
        public int? IDTipoTributacao { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }

        [CRUDParameterDAL(false, "CFOP")]
        public string CFOP { get; set; }

        [CRUDParameterDAL(false, "ICMS00_Orig")]
        public string ICMS00_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMS00_CST")]
        public string ICMS00_CST { get; set; }

        [CRUDParameterDAL(false, "ICMS00_pICMS")]
        public string ICMS00_pICMS { get; set; }

        [CRUDParameterDAL(false, "ICMS40_Orig")]
        public string ICMS40_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMS40_CST")]
        public string ICMS40_CST { get; set; }

        [CRUDParameterDAL(false, "ICMSSN102_Orig")]
        public string ICMSSN102_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMSSN102_CSOSN")]
        public string ICMSSN102_CSOSN { get; set; }

        [CRUDParameterDAL(false, "ICMSSN900_Orig")]
        public string ICMSSN900_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMSSN900_CSOSN")]
        public string ICMSSN900_CSOSN { get; set; }

        [CRUDParameterDAL(false, "ICMSSN900_pICMS")]
        public string ICMSSN900_pICMS { get; set; }

        [CRUDParameterDAL(false, "PISAliq_CST")]
        public string PISAliq_CST { get; set; }

        [CRUDParameterDAL(false, "PISAliq_pPIS")]
        public string PISAliq_pPIS { get; set; }

        [CRUDParameterDAL(false, "PISQtde_CST")]
        public string PISQtde_CST { get; set; }

        [CRUDParameterDAL(false, "PISQtde_vAliqProd")]
        public string PISQtde_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "PISNT_CST")]
        public string PISNT_CST { get; set; }

        [CRUDParameterDAL(false, "PISSN_CST")]
        public string PISSN_CST { get; set; }

        [CRUDParameterDAL(false, "PISOutr_CST")]
        public string PISOutr_CST { get; set; }

        [CRUDParameterDAL(false, "PISOutr_pPIS")]
        public string PISOutr_pPIS { get; set; }

        [CRUDParameterDAL(false, "PISOutr_vAliqProd")]
        public string PISOutr_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "PISST_pPIS")]
        public string PISST_pPIS { get; set; }

        [CRUDParameterDAL(false, "PISST_vAliqProd")]
        public string PISST_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "COFINSAliq_CST")]
        public string COFINSAliq_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSAliq_pCOFINS")]
        public string COFINSAliq_pCOFINS { get; set; }

        [CRUDParameterDAL(false, "COFINSQtde_CST")]
        public string COFINSQtde_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSQtde_vAliqProd")]
        public string COFINSQtde_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "COFINSNT_CST")]
        public string COFINSNT_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSSN_CST")]
        public string COFINSSN_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSOutr_CST")]
        public string COFINSOutr_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSOutr_pCOFINS")]
        public string COFINSOutr_pCOFINS { get; set; }

        [CRUDParameterDAL(false, "COFINSOutr_vAliqProd")]
        public string COFINSOutr_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "COFINSST_pCOFINS")]
        public string COFINSST_pCOFINS { get; set; }

        [CRUDParameterDAL(false, "COFINSST_vAliqProd")]
        public string COFINSST_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "ISSQN_vDeducISSQN")]
        public string ISSQN_vDeducISSQN { get; set; }

        [CRUDParameterDAL(false, "ISSQN_vAliq")]
        public string ISSQN_vAliq { get; set; }

        [CRUDParameterDAL(false, "ISSQN_cListServ")]
        public string ISSQN_cListServ { get; set; }

        [CRUDParameterDAL(false, "ISSQN_cServTribMun")]
        public string ISSQN_cServTribMun { get; set; }

        [CRUDParameterDAL(false, "ISSQN_cNatOp")]
        public string ISSQN_cNatOp { get; set; }

        [CRUDParameterDAL(false, "ISSQN_indIncFisc")]
        public string ISSQN_indIncFisc { get; set; }

        [CRUDParameterDAL(false, "vItem12741")]
        public string vItem12741 { get; set; }

        public static TipoTributacaoInformation ConverterObjeto(object obj)
        {
            return (TipoTributacaoInformation)obj;
        }
    }
}