using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbRetornoSAT")]
    [Serializable]
    public class RetornoSATInformation
    {
        [CRUDParameterDAL(true, "IDRetornoSAT")]
        public Int32? IDRetornoSAT { get; set; }

        [CRUDParameterDAL(false, "IDTipoSolicitacaoSAT", "IDTipoSolicitacaoSAT")]
        public TipoSolicitacaoSATInformation TipoSolicitacaoSAT { get; set; }

        [CRUDParameterDAL(false, "numeroSessao")]
        public String numeroSessao { get; set; }

        [CRUDParameterDAL(false, "EEEEE")]
        public String EEEEE { get; set; }

        [CRUDParameterDAL(false, "CCCC")]
        public String CCCC { get; set; }

        [CRUDParameterDAL(false, "mensagem")]
        public String mensagem { get; set; }

        [CRUDParameterDAL(false, "arquivoCFeSAT")]
        public String arquivoCFeSAT { get; set; }

        [CRUDParameterDAL(false, "cod")]
        public String cod { get; set; }

        [CRUDParameterDAL(false, "mensagemSEFAZ")]
        public String mensagemSEFAZ { get; set; }

        [CRUDParameterDAL(false, "timeStamp")]
        public String timeStamp { get; set; }

        [CRUDParameterDAL(false, "chaveConsulta")]
        public String chaveConsulta { get; set; }

        [CRUDParameterDAL(false, "valorTotalCFe")]
        public String valorTotalCFe { get; set; }

        [CRUDParameterDAL(false, "CPFCNPJValue")]
        public String CPFCNPJValue { get; set; }

        [CRUDParameterDAL(false, "assinaturaQRCODE")]
        public String assinaturaQRCODE { get; set; }

        [CRUDParameterDAL(false, "IDRetornoSAT_cancelamento", "IDRetornoSAT")]
        public RetornoSATInformation RetornoSATCancelamento { get; set; }

        public static RetornoSATInformation ConverterObjeto(Object obj)
        {
            return (RetornoSATInformation)obj;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{TipoSolicitacaoSAT.IDTipoSolicitacaoSAT}: {timeStamp} - {chaveConsulta} {valorTotalCFe} {mensagem}";
        }
    }
}
