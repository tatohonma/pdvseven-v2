using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.iFood
{
    public class IntegracaoEstabelecimento
    {
        public static Boolean LojaAbertaSistema()
        {
			String sql = @"
				SELECT	
					cbd.Valor as 'ID PDV iFood',
					CASE
						WHEN c.DtAbertura is null THEN  0
						WHEN c.DtAbertura is not null THEN  1
					END as 'Status Caixa iFood'
				FROM
					tbConfiguracaoBD cbd
					INNER JOIN tbCaixa c ON c.IDPDV = cbd.Valor
				WHERE
					cbd.Chave = 'CaixaPDV'
					AND
					c.DtAbertura is not null
					AND
					c.DtFechamento is null
				";

			return true;
        }
    }
}
