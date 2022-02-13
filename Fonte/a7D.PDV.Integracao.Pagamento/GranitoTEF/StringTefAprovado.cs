using System;
using System.IO;
using System.Text;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public class StringTefAprovado
    {
        public StringTefAprovado(StringBuilder valor)
        {
            var value = valor.ToString();
            try
            {
                Tipo = int.Parse(value.Substring(0, 1));
                Identificador = int.Parse(value.Substring(1, 6));
                Titulo = value.Substring(7, 31);
                Reservado = value.Substring(39, 22);
                TamIDTransacao = int.Parse(value.Substring(66, 3));
                IDTransacao = int.Parse(value.Substring(69, TamIDTransacao));
                CodigoResposta = int.Parse(value.Substring(79, 2));
                Descricao = value.Substring(81, 29);
                Adiquirente = (PG_Rede)int.Parse(value.Substring(111, 3));
                Bandeira = (PG_Bandeiras)int.Parse(value.Substring(114, 3));
                Transacao = (PG_TipoTransacao)int.Parse(value.Substring(117, 1));
                Forma = (PG_TipoForma)int.Parse(value.Substring(118, 1));
                Modalidade = (PG_TipoModalidade)int.Parse(value.Substring(119, 1));
                Parcelas = int.Parse(value.Substring(120, 2));
                Valor = decimal.Parse(value.Substring(122, 12)) / 100;
                NSU = value.Substring(134, 6);
                Aturorizacao = value.Substring(140, 6);
                TamCuponEstabelecimento = int.Parse(value.Substring(146, 12));
                CuponEstabelecimento = value.Substring(149, TamCuponEstabelecimento).Replace("|", "\r\n");
                TamCuponCliente = int.Parse(value.Substring(149 + TamCuponEstabelecimento, 12));
                CuponCliente = value.Substring(152 + TamCuponEstabelecimento, TamCuponCliente).Replace("|", "\r\n");
            }
            catch (Exception) { }
            try
            {
                string file = string.Format(@"C:\PAGO_TEF_WEB\logs\{0:yyyyMMdd-HHmmss}-{1}.txt", DateTime.Now, Aturorizacao);
                File.WriteAllText(file, value);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 1 Tipo
        /// 1 Mensagem
        /// </summary>
        public int Tipo { get; private set; }

        /// <summary>
        /// 2 - 7 Identificador entre aplicações
        /// Eco do identificador entre pdv e TEF
        /// </summary>
        public int Identificador { get; private set; }

        /// <summary>
        /// 8 - 39 Título com espaços ao fim 
        /// “TRANSACAO APROVADA ”
        /// </summary>
        public string Titulo { get; private set; }

        /// <summary>
        /// 40 - 66 Espaços em branco
        /// Reservado para uso futuro
        /// </summary>
        private string Reservado { get; set; }

        /// <summary>
        /// 67 - 69 Tamanho do campo ID Transação 
        /// Por padrão “010”
        /// </summary>
        private int TamIDTransacao { get; set; }

        /// <summary>
        /// 70 - 79 ID Transação
        /// Identificador da transação(utilizado no produto “Recorrência”)
        /// </summary>
        public int IDTransacao { get; private set; }

        /// <summary>
        /// 80 - 81 Código de Resposta 
        /// “00”
        /// </summary>
        public int CodigoResposta { get; private set; }

        /// <summary>
        /// 82 - 111 Descrição do Código de Resposta
        /// “TRANSACAO APROVADA ”
        /// </summary>
        public string Descricao { get; private set; }

        /// <summary>
        /// 112 - 114 Rede 
        /// Código da rede adquirente
        /// </summary>
        public PG_Rede Adiquirente { get; private set; }

        /// <summary>
        /// 115 - 117 Bandeira
        /// Código do cartão
        /// </summary>
        public PG_Bandeiras Bandeira { get; private set; }

        /// <summary>
        /// 118 Tipo de Transação 
        /// 1. Crédito 
        /// 2. Débito
        /// </summary>
        public PG_TipoTransacao Transacao { get; private set; }

        /// <summary>
        /// 119 Forma de Pagamento
        /// 1. À vista
        /// 2. Parcelado
        /// </summary>
        public PG_TipoForma Forma { get; private set; }

        /// <summary>
        /// 120 Modalidade de Parcelamento 
        /// 1. Sem juros 
        /// 2. Com juros
        /// </summary>
        public PG_TipoModalidade Modalidade { get; private set; }

        /// <summary>
        /// 121 - 122 Quantidade de Parcelas
        /// 00 quando débito ou crédito à vista
        /// </summary>
        public int Parcelas { get; private set; }

        /// <summary>
        /// Valor (12 bytes sendo os 2 últimos os centavos)
        /// Valor da venda
        /// </summary>
        public decimal Valor { get; private set; }

        /// <summary>
        /// 135 - 140 Nsu(número sequencial único)
        /// Número de transação gerado pelo TEF
        /// </summary>
        public string NSU { get; private set; }

        /// <summary>
        /// 141 - 146 Código Transação
        /// Código de autorização da operação aprovada (venda e estorno)
        /// </summary>
        public string Aturorizacao { get; private set; }

        /// <summary>
        /// 147 - 149 Tamanho do cupom do estabelecimento
        /// Três caracteres com zeros a esquerda
        /// </summary>
        private int TamCuponEstabelecimento { get; set; }

        /// <summary>
        /// 150 - y Cupom do estabelecimento 
        /// Quebra de linha é representada por |
        /// </summary>
        public string CuponEstabelecimento { get; private set; }

        /// <summary>
        /// (y+1) -(y+3) Tamanho do cupom do cliente
        /// Três caracteres com zeros a esquerda
        /// </summary>
        private int TamCuponCliente { get; set; }

        /// <summary>
        /// (y+4) - n Cupom do cliente Quebra de linha é representada por |
        /// </summary>
        public string CuponCliente { get; private set; }
    }
}