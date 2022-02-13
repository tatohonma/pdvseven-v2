using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    public class ValidarResponse : BaseResponse
    {
        private int _idTipoPedido;

        public ValidarResponse(TipoPedido tipo)
        {
            _idTipoPedido = (int)tipo;
        }

        public string guidIdentificacao { get; set; }
        public int idPedido { get; set; }
        public int idTipoPedido { get { return _idTipoPedido; } }
        public string referenciaLocalizacao { get; set; }
        public string cliente { get; set; }
        public decimal valorProdutos { get; set; }
        public decimal valorServico { get; set; }
        public decimal valorEntrada { get; set; }
        public decimal valorConsumacaoMinima { get; set; }
        public decimal valorTotal { get; set; }
        public decimal percentagemServico { get; set; }
    }

    public enum TipoPedido
    {
        MESA = 10,
        COMANDA = 20
    }
}