using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class ListarPaineisModificacaoResponse: BaseResponse
    {
        public PainelModificacao[] paineisModificacao { get; set; }

        public class PainelModificacao
        {
            public int idPainelModificacao { get; set; }
            public string titulo { get; set; }
            public int? min { get; set; }
            public int? max { get; set; }
            public bool ativo { get; set; }
            public DateTime dtUltimaAlteracao { get; set; }
            public ProdutoModificacao[] produtos { get; set; }
        }

        public class ProdutoModificacao
        {
            public bool selecionado { get { return false; } private set { } }
            public string tratamentoValor { get { return "somar"; } private set { } }
            public int ordem { get; set; }
            public int idProduto { get; set; }
        }
    }
}