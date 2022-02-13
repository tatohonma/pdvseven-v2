using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbProcessamentoSAT
    {
        public int IDProcessamentoSAT { get; set; }
        public Nullable<int> IDStatusProcessamentoSAT { get; set; }
        public Nullable<int> IDTipoSolicitacaoSAT { get; set; }
        public Nullable<int> IDRetornoSAT { get; set; }
        public string XMLEnvio { get; set; }
        public string GUID { get; set; }
        public Nullable<int> NumeroSessao { get; set; }
        public System.DateTime DataSolicitacao { get; set; }
        public virtual tbRetornoSAT tbRetornoSAT { get; set; }
        public virtual tbStatusProcessamentoSAT tbStatusProcessamentoSAT { get; set; }
        public virtual tbTipoSolicitacaoSAT tbTipoSolicitacaoSAT { get; set; }
    }
}
