using a7D.PDV.Ativacao.API.Services;
using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.Ativacao.Shared.Model;
using a7D.PDV.Ativacao.Shared.Services;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;

namespace a7D.PDV.Ativacao.API.Entities
{
    [Table("tbMensagem")]
    public class Mensagem : MensagemModel
    {
        [Required]
        public int IDAtivacao { get; set; }         // ID da Chave de Ativação do estabelecimento 

        [ForeignKey("IDAtivacao")]
        public virtual Ativacao Ativacao { get; set; }

        public Mensagem() : base()
        {
        }

        public Mensagem(MensagemNova nova)
        {
            if (nova.Destino == EOrigemDestinoMensagem.Ativador)
                DataRecebida = DateTime.Now;

            Tipo = nova.Tipo;
            KeyOrigem = nova.OrigemKey;
            Origem = nova.Origem;

            KeyDestino = nova.DestinoKey;
            Destino = nova.Destino;

            Texto = nova.Texto;
            if (nova.Parametros?.Length > 10000)
                Parametros = nova.Parametros.Substring(0, 10000) + "...";
            else
                Parametros = nova.Parametros;

            IDMensagemOrigem = nova.IdMensagemOrigem ?? 0;
        }

        public MensagemModel Lida()
        {
            if (DataRecebida == null)
                UpdateDB.RequestChanges("UPDATE tbMensagem SET DataRecebida=@DataRecebida WHERE IDMensagem=" + IDMensagem, new SqlParameter("@DataRecebida", DateTime.Now));

            return new MensagemRecebida(this);
        }
    }


}