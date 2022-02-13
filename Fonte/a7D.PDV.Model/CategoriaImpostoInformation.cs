using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCategoriaImposto")]
    [Serializable]
    public class CategoriaImpostoInformation
    {
        [CRUDParameterDAL(true, "IDCategoriaImposto")]
        public Int32? IDCategoriaImposto { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "ICMS00_Orig")]
        public String ICMS00_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMS00_CST")]
        public String ICMS00_CST { get; set; }

        [CRUDParameterDAL(false, "ICMS00_pICMS")]
        public String ICMS00_pICMS { get; set; }

        [CRUDParameterDAL(false, "ICMS40_Orig")]
        public String ICMS40_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMS40_CST")]
        public String ICMS40_CST { get; set; }

        [CRUDParameterDAL(false, "ICMSSN102_Orig")]
        public String ICMSSN102_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMSSN102_CSOSN")]
        public String ICMSSN102_CSOSN { get; set; }

        [CRUDParameterDAL(false, "ICMSSN900_Orig")]
        public String ICMSSN900_Orig { get; set; }

        [CRUDParameterDAL(false, "ICMSSN900_CSOSN")]
        public String ICMSSN900_CSOSN { get; set; }

        [CRUDParameterDAL(false, "ICMSSN900_pICMS")]
        public String ICMSSN900_pICMS { get; set; }

        [CRUDParameterDAL(false, "PISAliq_CST")]
        public String PISAliq_CST { get; set; }

        [CRUDParameterDAL(false, "PISAliq_vBC")]
        public String PISAliq_vBC { get; set; }

        [CRUDParameterDAL(false, "PISAliq_pPIS")]
        public String PISAliq_pPIS { get; set; }

        [CRUDParameterDAL(false, "PISQtde_CST")]
        public String PISQtde_CST { get; set; }

        [CRUDParameterDAL(false, "PISQtde_vAliqProd")]
        public String PISQtde_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "PISNT_CST")]
        public String PISNT_CST { get; set; }

        [CRUDParameterDAL(false, "PISSN_CST")]
        public String PISSN_CST { get; set; }

        [CRUDParameterDAL(false, "PISOutr_CST")]
        public String PISOutr_CST { get; set; }

        [CRUDParameterDAL(false, "PISOutr_vBC")]
        public String PISOutr_vBC { get; set; }

        [CRUDParameterDAL(false, "PISOutr_pPIS")]
        public String PISOutr_pPIS { get; set; }

        [CRUDParameterDAL(false, "PISOutr_vAliqProd")]
        public String PISOutr_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "PISST_vBC")]
        public String PISST_vBC { get; set; }

        [CRUDParameterDAL(false, "PISST_pPIS")]
        public String PISST_pPIS { get; set; }

        [CRUDParameterDAL(false, "PISST_vAliqProd")]
        public String PISST_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "COFINSAliq_CST")]
        public String COFINSAliq_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSAliq_vBC")]
        public String COFINSAliq_vBC { get; set; }

        [CRUDParameterDAL(false, "COFINSAliq_pCOFINS")]
        public String COFINSAliq_pCOFINS { get; set; }

        [CRUDParameterDAL(false, "COFINSQtde_CST")]
        public String COFINSQtde_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSQtde_vAliqProd")]
        public String COFINSQtde_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "COFINSNT_CST")]
        public String COFINSNT_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSSN_CST")]
        public String COFINSSN_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSOutr_CST")]
        public String COFINSOutr_CST { get; set; }

        [CRUDParameterDAL(false, "COFINSOutr_vBC")]
        public String COFINSOutr_vBC { get; set; }

        [CRUDParameterDAL(false, "COFINSOutr_pCOFINS")]
        public String COFINSOutr_pCOFINS { get; set; }

        [CRUDParameterDAL(false, "COFINSOutr_vAliqProd")]
        public String COFINSOutr_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "COFINSST_vBC")]
        public String COFINSST_vBC { get; set; }

        [CRUDParameterDAL(false, "COFINSST_pCOFINS")]
        public String COFINSST_pCOFINS { get; set; }

        [CRUDParameterDAL(false, "COFINSST_vAliqProd")]
        public String COFINSST_vAliqProd { get; set; }

        [CRUDParameterDAL(false, "ISSQN_vDeducISSQN")]
        public String ISSQN_vDeducISSQN { get; set; }

        [CRUDParameterDAL(false, "ISSQN_vAliq")]
        public String ISSQN_vAliq { get; set; }

        [CRUDParameterDAL(false, "ISSQN_cListServ")]
        public String ISSQN_cListServ { get; set; }

        [CRUDParameterDAL(false, "ISSQN_cServTribMun")]
        public String ISSQN_cServTribMun { get; set; }

        [CRUDParameterDAL(false, "ISSQN_cNatOp")]
        public String ISSQN_cNatOp { get; set; }

        [CRUDParameterDAL(false, "ISSQN_indIncFisc")]
        public String ISSQN_indIncFisc { get; set; }

        [CRUDParameterDAL(false, "indRegra")]
        public String indRegra { get; set; }

        [CRUDParameterDAL(false, "vItem12741")]
        public String vItem12741 { get; set; }

        public static CategoriaImpostoInformation ConverterObjeto(Object obj)
        {
            return (CategoriaImpostoInformation)obj;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
