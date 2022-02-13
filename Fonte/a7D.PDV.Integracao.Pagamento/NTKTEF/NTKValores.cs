using System;
using System.Collections.Generic;
using System.Text;

namespace a7D.PDV.Integracao.Pagamento.NTKTEF
{
    public class NTKValores
    {
        internal SortedDictionary<NTKCampos, string> Lista { get; private set; }

        public NTKValores(NTKEtapa etapa, long identificacao)
        {
            Etapa = etapa;
            Lista = new SortedDictionary<NTKCampos, string>
            {
                [NTKCampos.Identificacao] = identificacao.ToString()
            };
        }

        public string this[NTKCampos campo]
        {
            get
            {
                if (Lista.ContainsKey(campo))
                    return Lista[campo];
                else
                    return null;
            }
            internal set
            {
                Lista[campo] = value;
            }
        }

        /// <summary>
        /// Etapa Atual
        /// </summary>
        public NTKEtapa Etapa { get; internal set; }

        /// <summary>
        /// Arquivo de origem
        /// </summary>
        public string Arquivo { get; private set; }

        /// <summary>
        /// Valor original lido e processado
        /// Quando há ? é porque uma variável não foi reconhecida
        /// </summary>
        public string Resultado => resposta.ToString();

        /// <summary>
        /// Identificação da coleção de valores
        /// </summary>
        public long Identificacao => Lista.ContainsKey(NTKCampos.Identificacao) ? long.Parse(Lista[NTKCampos.Identificacao]) : 0;

        public object Current => throw new NotImplementedException();

        private StringBuilder resposta;

        internal void LeLinhas(IEnumerable<string> linhas, string arquivo)
        {
            Arquivo = arquivo;
            resposta = new StringBuilder();
            var primeiro = new List<NTKCampos>();

            foreach (var linha in linhas)
            {
                // 0123456789ABCDEF
                // 000-000 = abcde... 
                if (linha.Length < 10)
                    continue;

                string codigo = linha.Substring(0, 3);
                string valor = linha.Substring(10);

                if (Enum.TryParse(codigo, out NTKCampos campo))
                {
                    var v = Enum.GetName(typeof(NTKCampos), campo);
                    if (v == null)
                        continue;

                    if (primeiro.Contains(campo))
                    {
                        this[campo] += "\r\n" + valor;
                        resposta.AppendLine(valor);
                    }
                    else
                    {
                        primeiro.Add(campo);
                        this[campo] = valor;
                        resposta.AppendLine($"{campo} {valor}");
                    }
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in Lista)
                sb.AppendLine($"{item.Key} = {item.Value}");

            return sb.ToString();
        }
    }
}