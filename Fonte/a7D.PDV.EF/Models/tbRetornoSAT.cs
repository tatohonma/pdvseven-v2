using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbRetornoSAT
    {
        public tbRetornoSAT()
        {
            this.tbPedidoes = new List<tbPedido>();
            this.tbPedidoes1 = new List<tbPedido>();
            this.tbProcessamentoSATs = new List<tbProcessamentoSAT>();
            this.tbRetornoSAT1 = new List<tbRetornoSAT>();
        }

        public int IDRetornoSAT { get; set; }
        public int IDTipoSolicitacaoSAT { get; set; }
        public string numeroSessao { get; set; }
        public string EEEEE { get; set; }
        public string CCCC { get; set; }
        public string mensagem { get; set; }
        public string cod { get; set; }
        public string mensagemSEFAZ { get; set; }
        public string arquivoCFeSAT { get; set; }
        public string timeStamp { get; set; }
        public string chaveConsulta { get; set; }
        public string valorTotalCFe { get; set; }
        public string CPFCNPJValue { get; set; }
        public string assinaturaQRCODE { get; set; }
        public Nullable<int> IDRetornoSAT_cancelamento { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes1 { get; set; }
        public virtual ICollection<tbProcessamentoSAT> tbProcessamentoSATs { get; set; }
        public virtual ICollection<tbRetornoSAT> tbRetornoSAT1 { get; set; }
        public virtual tbRetornoSAT tbRetornoSAT2 { get; set; }
        public virtual tbTipoSolicitacaoSAT tbTipoSolicitacaoSAT { get; set; }
    }
}
