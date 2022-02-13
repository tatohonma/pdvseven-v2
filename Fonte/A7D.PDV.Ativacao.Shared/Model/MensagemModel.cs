using a7D.PDV.Ativacao.Shared.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Ativacao.Shared.Model
{
    public abstract class MensagemModel : ICloneable
    {
        [Key]
        public int IDMensagem { get; set; }

        [Required]
        public DateTime DataCriada { get; set; }    // Data de Criação da mensagem

        public DateTime? DataRecebida { get; set; } // Data do recebimento (obtenção pelo programa via WS, ou quando a mensagem chegou do outro lado)

        [Required]
        public int IDTipo { get; set; }             // Tipo da Mensagem: Atualização, Cobrança, Resposta, SIM/NÃO, OK, Dica, Configuração

        [Required]
        public int IDOrigem { get; set; }           // Ativador, Guardião, IA

        [MaxLength(50)]
        public string KeyOrigem { get; set; }       // Chave para identificar o destino: ID, CPF, Numero Celular, Chave qualquer, Tipo: Gerente

        [Required]
        public int IDDestino { get; set; }          // Ativador, Guardião, IA

        [MaxLength(50)]
        public string KeyDestino { get; set; }      // Chave para identificar a origem: ID, CPF, Numero Celular, Chave qualquer, Tipo: Admin

        [Required]
        [Column(TypeName = "text")]
        public string Texto { get; set; }           // Texto a ser exibido

        [Column(TypeName = "text")]
        public string Parametros { get; set; }      // Dados da mensagem, JSON, Link, XML, Cor de background, Foreground

        public DateTime? DataVisualizada { get; set; }  // Data de Visualização (Notificação que foi vista por alguem, ou que algo foi concluido, por exemplo uma atualização)

        [Required]
        public int IDMensagemOrigem { get; set; }   // ID da mensagem que deu origem a atual

        [NotMapped]
        public EOrigemDestinoMensagem Origem { get => (EOrigemDestinoMensagem)IDOrigem; set => IDOrigem = (int)value; }

        [NotMapped]
        public EOrigemDestinoMensagem Destino { get => (EOrigemDestinoMensagem)IDDestino; set => IDDestino = (int)value; }

        [NotMapped]
        public ETipoMensagem Tipo { get => (ETipoMensagem)IDTipo; set => IDTipo = (int)value; }

        public override string ToString()
        {
            return $"{IDMensagem}: {Tipo} ({Origem} => {Destino}) {Texto} - {Parametros}";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public MensagemModel()
        {
            DataCriada = DateTime.Now;
        }

        public MensagemModel(MensagemModel nova)
        {
            // O 'IDMensagem' é unico em cada sistema, mas é referenciado no 'IDMensagemOrigem'
            // O conceito da mensagem é parecido muito com os emails, então mensagens nunca pode ser alteradas
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