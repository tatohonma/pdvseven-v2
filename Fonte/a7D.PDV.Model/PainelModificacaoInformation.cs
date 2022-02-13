using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPainelModificacao")]
    [Serializable]
    public class PainelModificacaoInformation
    {
        [CRUDParameterDAL(true, "IDPainelModificacao")]
        public Int32? IDPainelModificacao { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Titulo")]
        public String Titulo { get; set; }

        [CRUDParameterDAL(false, "Maximo")]
        public Int32? Maximo { get; set; }

        [CRUDParameterDAL(false, "Minimo")]
        public Int32? Minimo { get; set; }

        [CRUDParameterDAL(false, "DtUltimaAlteracao")]
        public DateTime? DtUltimaAlteracao { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public Boolean? Excluido { get; set; }

        [CRUDParameterDAL(false, "IDPainelModificacaoOperacao", "IDPainelModificacaoOperacao")]
        public PainelModificacaoOperacaoInformation PainelModificacaoOperacao { get; set; }

        [CRUDParameterDAL(false, "IDValorUtilizado")]
        public Int32? IDValorUtilizado { get; set; }

        [CRUDParameterDAL(false, "IDTipoItem")]
        public Int32? IDTipoItem { get; set; }

        [CRUDParameterDAL(false, "IgnorarValorItem")]
        public Boolean? IgnorarValorItem { get; set; }

        public List<PainelModificacaoProdutoInformation> ListaProduto { get; set; }

        public List<PainelModificacaoCategoriaInformation> ListaCategoria { get; set; }
        public List<PainelModificacaoRelacionadoInformation> PaineisRelacionados { get; set; }

        public static PainelModificacaoInformation ConverterObjeto(Object obj)
        {
            return (PainelModificacaoInformation)obj;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ((PainelModificacaoInformation)obj).GetHashCode() == GetHashCode();
        }

        public static bool operator ==(PainelModificacaoInformation pn1, PainelModificacaoInformation pn2)
        {
            if (ReferenceEquals(pn1, null))
                return ReferenceEquals(pn2, null);
            return pn1.Equals(pn2);
        }

        public static bool operator !=(PainelModificacaoInformation pn1, PainelModificacaoInformation pn2)
        {
            return !(pn1 == pn2);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            if (IDPainelModificacao.HasValue)
                hash = hash * 23 + IDPainelModificacao.Value.GetHashCode();
            if (Nome != null)
                hash = hash * 23 + Nome.GetHashCode();
            if (DtUltimaAlteracao.HasValue)
                hash = hash * 23 + DtUltimaAlteracao.GetHashCode();
            if (Excluido.HasValue)
                hash = hash * 23 + Excluido.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"{IDPainelModificacao ?? 0 } {Titulo} ({Nome})";
        }
    }
}
