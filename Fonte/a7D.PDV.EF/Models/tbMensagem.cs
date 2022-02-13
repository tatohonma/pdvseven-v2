using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.EF.Models
{
    [Table("tbMensagem")]
    public class tbMensagem : Ativacao.Shared.Model.MensagemModel
    {
        public DateTime? DataProcessamento { get; set; }

        [Column(TypeName = "text")]
        public string ResultadoProcessamento { get; set; }

        public int IDRespostaProcesamento { get; set; }

        public tbMensagem() : base()
        {
        }

        public tbMensagem(Ativacao.Shared.Model.MensagemModel nova)
        {
            DataCriada = nova.DataCriada;
            DataRecebida = DateTime.Now;
            Tipo = nova.Tipo;

            KeyOrigem = nova.KeyOrigem;
            Origem = nova.Origem;

            KeyDestino = nova.KeyDestino;
            Destino = nova.Destino;

            Texto = nova.Texto;
            Parametros = nova.Parametros;
            IDMensagemOrigem = nova.IDMensagem;
        }
    }
}