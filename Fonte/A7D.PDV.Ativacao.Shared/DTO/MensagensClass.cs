using a7D.PDV.Ativacao.Shared.Model;
using a7D.PDV.Ativacao.Shared.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace a7D.PDV.Ativacao.Shared.DTO
{
    //public class MensagemBase
    //{
    //    [Required]
    //    [TypeConverter(typeof(EnumTypeConverter<ETipoMensagem>))]
    //    public ETipoMensagem Tipo { get; set; }

    //    [Required]
    //    [TypeConverter(typeof(EnumTypeConverter<EOrigemDestinoMensagem>))]
    //    public EOrigemDestinoMensagem Origem { get; set; }
    //    public string OrigemKey { get; set; }

    //    [Required]
    //    [TypeConverter(typeof(EnumTypeConverter<EOrigemDestinoMensagem>))]
    //    public EOrigemDestinoMensagem Destino { get; set; }
    //    public string DestinoKey { get; set; }

    //    [Required(AllowEmptyStrings = false)]
    //    public string Texto { get; set; }
    //    public string Parametros { get; set; }
    //}

    //public class MensagemRecebida : MensagemBase
    //{
    //    public int IdMensagem { get; set; }

    //    public DateTime DataCriada { get; set; }
    //}

    //public class MensagemLida
    //{
    //    [Required(ErrorMessage = "Informe a chave de ativação", AllowEmptyStrings = false)]
    //    [RegularExpression(@"\d{3}-\d{5}-\d{2}", ErrorMessage = "Informe uma chave de ativação válida")]
    //    public string Chave { get; set; }

    //    [Required(ErrorMessage = "Informe o ID da mensagem")]
    //    public int IdMensagem { get; set; }
    //}

    public class MensagemRecebida : MensagemModel
    {
        public MensagemRecebida() : base()
        {
        }

        public MensagemRecebida(MensagemModel mensagem) : base(mensagem)
        {
        }
    }

    public class MensagemNova
    {
        [Required(ErrorMessage = "Informe a chave de ativação", AllowEmptyStrings = false)]
        [RegularExpression(@"\d{3}-\d{5}-\d{2}", ErrorMessage = "Chave de ativação em formato inválido")]
        public string Chave { get; set; }

        public int? IdMensagemOrigem { get; set; }

        [Required]
        [TypeConverter(typeof(EnumTypeConverter<ETipoMensagem>))]
        public ETipoMensagem Tipo { get; set; }

        [Required]
        [TypeConverter(typeof(EnumTypeConverter<EOrigemDestinoMensagem>))]
        public EOrigemDestinoMensagem Origem { get; set; }
        public string OrigemKey { get; set; }

        [Required]
        [TypeConverter(typeof(EnumTypeConverter<EOrigemDestinoMensagem>))]
        public EOrigemDestinoMensagem Destino { get; set; }
        public string DestinoKey { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Texto { get; set; }
        public string Parametros { get; set; }
    }
}