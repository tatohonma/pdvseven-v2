using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoTributacaoMap : EntityTypeConfiguration<tbTipoTributacao>
    {
        public tbTipoTributacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoTributacao);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(500);

            this.Property(t => t.CFOP)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ICMS00_Orig)
                .HasMaxLength(50);

            this.Property(t => t.ICMS00_CST)
                .HasMaxLength(50);

            this.Property(t => t.ICMS00_pICMS)
                .HasMaxLength(50);

            this.Property(t => t.ICMS40_Orig)
                .HasMaxLength(50);

            this.Property(t => t.ICMS40_CST)
                .HasMaxLength(50);

            this.Property(t => t.ICMSSN102_Orig)
                .HasMaxLength(50);

            this.Property(t => t.ICMSSN102_CSOSN)
                .HasMaxLength(50);

            this.Property(t => t.ICMSSN900_Orig)
                .HasMaxLength(50);

            this.Property(t => t.ICMSSN900_CSOSN)
                .HasMaxLength(50);

            this.Property(t => t.ICMSSN900_pICMS)
                .HasMaxLength(50);

            this.Property(t => t.PISAliq_CST)
                .HasMaxLength(50);

            this.Property(t => t.PISAliq_pPIS)
                .HasMaxLength(50);

            this.Property(t => t.PISQtde_CST)
                .HasMaxLength(50);

            this.Property(t => t.PISQtde_vAliqProd)
                .HasMaxLength(50);

            this.Property(t => t.PISNT_CST)
                .HasMaxLength(50);

            this.Property(t => t.PISSN_CST)
                .HasMaxLength(50);

            this.Property(t => t.PISOutr_CST)
                .HasMaxLength(50);

            this.Property(t => t.PISOutr_pPIS)
                .HasMaxLength(50);

            this.Property(t => t.PISOutr_vAliqProd)
                .HasMaxLength(50);

            this.Property(t => t.PISST_pPIS)
                .HasMaxLength(50);

            this.Property(t => t.PISST_vAliqProd)
                .HasMaxLength(50);

            this.Property(t => t.COFINSAliq_CST)
                .HasMaxLength(50);

            this.Property(t => t.COFINSAliq_pCOFINS)
                .HasMaxLength(50);

            this.Property(t => t.COFINSQtde_CST)
                .HasMaxLength(50);

            this.Property(t => t.COFINSQtde_vAliqProd)
                .HasMaxLength(50);

            this.Property(t => t.COFINSNT_CST)
                .HasMaxLength(50);

            this.Property(t => t.COFINSSN_CST)
                .HasMaxLength(50);

            this.Property(t => t.COFINSOutr_CST)
                .HasMaxLength(50);

            this.Property(t => t.COFINSOutr_pCOFINS)
                .HasMaxLength(50);

            this.Property(t => t.COFINSOutr_vAliqProd)
                .HasMaxLength(50);

            this.Property(t => t.COFINSST_pCOFINS)
                .HasMaxLength(50);

            this.Property(t => t.COFINSST_vAliqProd)
                .HasMaxLength(50);

            this.Property(t => t.ISSQN_vDeducISSQN)
                .HasMaxLength(50);

            this.Property(t => t.ISSQN_vAliq)
                .HasMaxLength(50);

            this.Property(t => t.ISSQN_cListServ)
                .HasMaxLength(50);

            this.Property(t => t.ISSQN_cServTribMun)
                .HasMaxLength(50);

            this.Property(t => t.ISSQN_cNatOp)
                .HasMaxLength(50);

            this.Property(t => t.ISSQN_indIncFisc)
                .HasMaxLength(50);

            this.Property(t => t.vItem12741)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbTipoTributacao");
            this.Property(t => t.IDTipoTributacao).HasColumnName("IDTipoTributacao");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.CFOP).HasColumnName("CFOP");
            this.Property(t => t.ICMS00_Orig).HasColumnName("ICMS00_Orig");
            this.Property(t => t.ICMS00_CST).HasColumnName("ICMS00_CST");
            this.Property(t => t.ICMS00_pICMS).HasColumnName("ICMS00_pICMS");
            this.Property(t => t.ICMS40_Orig).HasColumnName("ICMS40_Orig");
            this.Property(t => t.ICMS40_CST).HasColumnName("ICMS40_CST");
            this.Property(t => t.ICMSSN102_Orig).HasColumnName("ICMSSN102_Orig");
            this.Property(t => t.ICMSSN102_CSOSN).HasColumnName("ICMSSN102_CSOSN");
            this.Property(t => t.ICMSSN900_Orig).HasColumnName("ICMSSN900_Orig");
            this.Property(t => t.ICMSSN900_CSOSN).HasColumnName("ICMSSN900_CSOSN");
            this.Property(t => t.ICMSSN900_pICMS).HasColumnName("ICMSSN900_pICMS");
            this.Property(t => t.PISAliq_CST).HasColumnName("PISAliq_CST");
            this.Property(t => t.PISAliq_pPIS).HasColumnName("PISAliq_pPIS");
            this.Property(t => t.PISQtde_CST).HasColumnName("PISQtde_CST");
            this.Property(t => t.PISQtde_vAliqProd).HasColumnName("PISQtde_vAliqProd");
            this.Property(t => t.PISNT_CST).HasColumnName("PISNT_CST");
            this.Property(t => t.PISSN_CST).HasColumnName("PISSN_CST");
            this.Property(t => t.PISOutr_CST).HasColumnName("PISOutr_CST");
            this.Property(t => t.PISOutr_pPIS).HasColumnName("PISOutr_pPIS");
            this.Property(t => t.PISOutr_vAliqProd).HasColumnName("PISOutr_vAliqProd");
            this.Property(t => t.PISST_pPIS).HasColumnName("PISST_pPIS");
            this.Property(t => t.PISST_vAliqProd).HasColumnName("PISST_vAliqProd");
            this.Property(t => t.COFINSAliq_CST).HasColumnName("COFINSAliq_CST");
            this.Property(t => t.COFINSAliq_pCOFINS).HasColumnName("COFINSAliq_pCOFINS");
            this.Property(t => t.COFINSQtde_CST).HasColumnName("COFINSQtde_CST");
            this.Property(t => t.COFINSQtde_vAliqProd).HasColumnName("COFINSQtde_vAliqProd");
            this.Property(t => t.COFINSNT_CST).HasColumnName("COFINSNT_CST");
            this.Property(t => t.COFINSSN_CST).HasColumnName("COFINSSN_CST");
            this.Property(t => t.COFINSOutr_CST).HasColumnName("COFINSOutr_CST");
            this.Property(t => t.COFINSOutr_pCOFINS).HasColumnName("COFINSOutr_pCOFINS");
            this.Property(t => t.COFINSOutr_vAliqProd).HasColumnName("COFINSOutr_vAliqProd");
            this.Property(t => t.COFINSST_pCOFINS).HasColumnName("COFINSST_pCOFINS");
            this.Property(t => t.COFINSST_vAliqProd).HasColumnName("COFINSST_vAliqProd");
            this.Property(t => t.ISSQN_vDeducISSQN).HasColumnName("ISSQN_vDeducISSQN");
            this.Property(t => t.ISSQN_vAliq).HasColumnName("ISSQN_vAliq");
            this.Property(t => t.ISSQN_cListServ).HasColumnName("ISSQN_cListServ");
            this.Property(t => t.ISSQN_cServTribMun).HasColumnName("ISSQN_cServTribMun");
            this.Property(t => t.ISSQN_cNatOp).HasColumnName("ISSQN_cNatOp");
            this.Property(t => t.ISSQN_indIncFisc).HasColumnName("ISSQN_indIncFisc");
            this.Property(t => t.vItem12741).HasColumnName("vItem12741");
        }
    }
}
