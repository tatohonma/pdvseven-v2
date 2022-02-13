using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoSolicitacaoSAT
    {
        public tbTipoSolicitacaoSAT()
        {
            this.tbProcessamentoSATs = new List<tbProcessamentoSAT>();
            this.tbRetornoSATs = new List<tbRetornoSAT>();
        }

        public int IDTipoSolicitacaoSAT { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbProcessamentoSAT> tbProcessamentoSATs { get; set; }
        public virtual ICollection<tbRetornoSAT> tbRetornoSATs { get; set; }
    }
}
