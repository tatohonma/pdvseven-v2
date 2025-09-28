using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.Ativacao.Shared.Model;

namespace a7D.PDV.Ativacao.API.Model
{
    [Table("mensage")]
    public class Mensagem : MensagemModel
    {
        [Required]
        public int IDAtivacao { get; set; } 

        [ForeignKey("IDAtivacao")]
        public virtual Activation Ativacao { get; set; }

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

        // public MensagemModel Lida()
        // {
        //     if (DataRecebida == null)
        //     {
        //         var agora = DateTime.Now;
        //
        //         var sql = "UPDATE tbMensagem " +
        //                   "SET DataRecebida = @DataRecebida " +
        //                   "WHERE IDMensagem = @IDMensagem";
        //
        //         UpdateDB.RequestChanges(
        //             sql,
        //             new SqlParameter("@DataRecebida", agora),
        //             new SqlParameter("@IDMensagem", IDMensagem)
        //         );
        //
        //         // mantém o estado do objeto consistente
        //         DataRecebida = agora;
        //     }
        //
        //     return new MensagemRecebida(this);
        // }
    }


}