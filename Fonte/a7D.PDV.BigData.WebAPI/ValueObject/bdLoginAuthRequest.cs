using System.ComponentModel.DataAnnotations;

namespace a7D.PDV.BigData.WebAPI.ValueObject
{
    public class bdLoginAuthRequest
    {
        [Required]
        public string CNPJ { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string ChannelId { get; set; }

        [Required]
        public string FromId { get; set; }
    }
}
