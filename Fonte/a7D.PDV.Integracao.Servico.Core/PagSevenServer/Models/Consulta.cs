using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.PagSevenServer.Models
{
    public class Consulta
    {
        public Int32 IDConsulta { get; set; }
        public string TelefoneCliente { get; set; }
        public Int32? IDEstabelecimento { get; set; }
        public Int32? IDPedido { get; set; }
        public string RespostaEstabelecimento { get; set; }
        public DateTime? DataRespostaEstabelecimento { get; set; }
        public DateTime DataCriacao { get; set; }

    }
}
