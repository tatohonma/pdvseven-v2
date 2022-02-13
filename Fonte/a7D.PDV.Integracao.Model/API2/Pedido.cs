
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using a7D.PDV.Model;

namespace a7D.PDV.Integracao.Model.API2
{

  
    public partial class Pedido 
    {
       
        public Pedido(int? IDPedido = default(int?),
            string NumeroComanda = "", string NumeroMesa = "",
            Cliente Cliente = default(Cliente)
            , int? IDTipoPedido = default(int?)
            , int? StatusPedido = default(int?)
            , TipoEntrada TipoEntrada = default(TipoEntrada)
            , string GUIDIdentificacao = default(string)
            , DateTime? DtPedido = default(DateTime?)
            , DateTime? DtPedidoFechamento = default(DateTime?)
            , DateTime? DtEnvio = default(DateTime?)
            , DateTime? DtEntrega = default(DateTime?)
            , decimal? ValorServico = default(decimal?)
            , decimal? ValorDesconto = default(decimal?)
            , decimal? ValorEntrega = default(decimal?)
            , decimal? ValorTotal = default(decimal?)
            , int? NumeroPessoas = default(int?)
            , List<Item> Itens = default(List<Item>)
            , List<Item> ItensCancelados = default(List<Item>)
            , List<Pagamento> Pagamentos = default(List<Pagamento>)
            ,Estabelecimento estabelecimento = default(Estabelecimento)
            )
        {
            this.IDPedido = IDPedido;
            this.NumeroComanda = NumeroComanda;
            this.NumeroMesa = NumeroMesa;
            this.Cliente = Cliente;
            this.IDTipoPedido = IDTipoPedido;
            this.StatusPedido = StatusPedido;
            this.TipoEntrada = TipoEntrada;
            this.GUIDIdentificacao = GUIDIdentificacao;
            this.DtPedido = DtPedido;
            this.DtPedidoFechamento = DtPedidoFechamento;
            this.DtEnvio = DtEnvio;
            this.DtEntrega = DtEntrega;
            this.ValorServico = ValorServico;
            this.ValorDesconto = ValorDesconto;
            this.ValorEntrega = ValorEntrega;
            this.ValorTotal = ValorTotal;
            this.NumeroPessoas = NumeroPessoas;
            this.Itens = Itens;
            this.ItensCancelados = ItensCancelados;
            this.Pagamentos = Pagamentos;
            this.Estabelecimento = estabelecimento;
        }

        public static Pedido Converter(PedidoInformation pedido)
        {
            return new Pedido(pedido?.IDPedido,
                pedido.NumeroComanda,
                pedido.NumeroMesa,
                Cliente.Converter(pedido?.Cliente), 
                pedido?.TipoPedido?.IDTipoPedido,
                pedido?.StatusPedido?.IDStatusPedido, 
                TipoEntrada.Converter(pedido?.TipoEntrada), 
                pedido?.GUIDIdentificacao, pedido?.DtPedido,
                pedido?.DtPedidoFechamento, pedido?.DtEnvio,
                pedido?.DtEntrega, pedido?.ValorServico, pedido?.ValorDesconto,
                pedido?.ValorEntrega, pedido?.ValorTotal, pedido?.NumeroPessoas);
        }

        [DataMember(Name = "NumeroComanda", EmitDefaultValue = true)]
        public string NumeroComanda { get; set; }
        [DataMember(Name = "NumeroMesa", EmitDefaultValue = true)]
        public string NumeroMesa { get; set; }

        /// <summary>
        /// Chave primária deste pedido
        /// </summary>
        /// <value>Chave primária deste pedido</value>
        [DataMember(Name = "IDPedido", EmitDefaultValue = false)]
        public int? IDPedido { get; set; }
        /// <summary>
        /// Gets or Sets Cliente
        /// </summary>
        [DataMember(Name = "Cliente", EmitDefaultValue = false)]
        public Cliente Cliente { get; set; }
        /// <summary>
        /// Tipo deste pedido 10: Mesa 20: Comanda 30: Delivery 40: Balcão 
        /// </summary>
        /// <value>Tipo deste pedido 10: Mesa 20: Comanda 30: Delivery 40: Balcão </value>
        [DataMember(Name = "IDTipoPedido", EmitDefaultValue = false)]
        public int? IDTipoPedido { get; set; }
        /// <summary>
        /// Status deste pedido 10: Aberto 20: Enviado 40: Finalizado 50: Cancelado 
        /// </summary>
        /// <value>Status deste pedido 10: Aberto 20: Enviado 40: Finalizado 50: Cancelado </value>
        [DataMember(Name = "StatusPedido", EmitDefaultValue = false)]
        public int? StatusPedido { get; set; }
        /// <summary>
        /// Gets or Sets TipoEntrada
        /// </summary>
        [DataMember(Name = "TipoEntrada", EmitDefaultValue = false)]
        public TipoEntrada TipoEntrada { get; set; }
        /// <summary>
        /// GUID de identificação deste pedido
        /// </summary>
        /// <value>GUID de identificação deste pedido</value>
        [DataMember(Name = "GUIDIdentificacao", EmitDefaultValue = false)]
        public string GUIDIdentificacao { get; set; }
        /// <summary>
        /// Data de abertura deste pedido
        /// </summary>
        /// <value>Data de abertura deste pedido</value>
        [DataMember(Name = "DtPedido", EmitDefaultValue = false)]
        public DateTime? DtPedido { get; set; }
        /// <summary>
        /// Data de fechamento deste pedido
        /// </summary>
        /// <value>Data de fechamento deste pedido</value>
        [DataMember(Name = "DtPedidoFechamento", EmitDefaultValue = false)]
        public DateTime? DtPedidoFechamento { get; set; }
        /// <summary>
        /// Data em que o pedido foi enviado
        /// </summary>
        /// <value>Data em que o pedido foi enviado</value>
        [DataMember(Name = "DtEnvio", EmitDefaultValue = false)]
        public DateTime? DtEnvio { get; set; }
        /// <summary>
        /// Data informada em que este pedido foi entregue
        /// </summary>
        /// <value>Data informada em que este pedido foi entregue</value>
        [DataMember(Name = "DtEntrega", EmitDefaultValue = false)]
        public DateTime? DtEntrega { get; set; }
        /// <summary>
        /// Valor  deste pedido
        /// </summary>
        /// <value>Valor  deste pedido</value>
        [DataMember(Name = "ValorServico", EmitDefaultValue = false)]
        public decimal? ValorServico { get; set; }
        /// <summary>
        /// Valor  deste pedido
        /// </summary>
        /// <value>Valor  deste pedido</value>
        [DataMember(Name = "ValorDesconto", EmitDefaultValue = false)]
        public decimal? ValorDesconto { get; set; }
        /// <summary>
        /// Valor  deste pedido
        /// </summary>
        /// <value>Valor  deste pedido</value>
        [DataMember(Name = "ValorEntrega", EmitDefaultValue = false)]
        public decimal? ValorEntrega { get; set; }
        /// <summary>
        /// Valor  deste pedido
        /// </summary>
        /// <value>Valor  deste pedido</value>
        [DataMember(Name = "ValorTotal", EmitDefaultValue = false)]
        public decimal? ValorTotal { get; set; }
        /// <summary>
        /// Numero de pessoas para este pedido
        /// </summary>
        /// <value>Numero de pessoas para este pedido</value>
        [DataMember(Name = "NumeroPessoas", EmitDefaultValue = false)]
        public int? NumeroPessoas { get; set; }
        /// <summary>
        /// Gets or Sets Itens
        /// </summary>
        [DataMember(Name = "Itens", EmitDefaultValue = false)]
        public List<Item> Itens { get; set; }
        [DataMember(Name = "ItensCancelados", EmitDefaultValue = false)]
        public List<Item> ItensCancelados { get; set; }
        /// <summary>
        /// Gets or Sets Pagamentos
        /// </summary>
        [DataMember(Name = "Pagamentos", EmitDefaultValue = false)]
        public List<Pagamento> Pagamentos { get; set; }

        [DataMember(Name = "Estabelecimento", EmitDefaultValue = false)]
        public Estabelecimento Estabelecimento { get; set; }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Pedido {\n");
            sb.Append("  IDPedido: ").Append(IDPedido).Append("\n");
            sb.Append("  Cliente: ").Append(Cliente).Append("\n");
            sb.Append("  IDTipoPedido: ").Append(IDTipoPedido).Append("\n");
            sb.Append("  StatusPedido: ").Append(StatusPedido).Append("\n");
            sb.Append("  TipoEntrada: ").Append(TipoEntrada).Append("\n");
            sb.Append("  GUIDIdentificacao: ").Append(GUIDIdentificacao).Append("\n");
            sb.Append("  DtPedido: ").Append(DtPedido).Append("\n");
            sb.Append("  DtPedidoFechamento: ").Append(DtPedidoFechamento).Append("\n");
            sb.Append("  DtEnvio: ").Append(DtEnvio).Append("\n");
            sb.Append("  DtEntrega: ").Append(DtEntrega).Append("\n");
            sb.Append("  ValorServico: ").Append(ValorServico).Append("\n");
            sb.Append("  ValorDesconto: ").Append(ValorDesconto).Append("\n");
            sb.Append("  ValorEntrega: ").Append(ValorEntrega).Append("\n");
            sb.Append("  ValorTotal: ").Append(ValorTotal).Append("\n");
            sb.Append("  NumeroPessoas: ").Append(NumeroPessoas).Append("\n");
            sb.Append("  Itens: ").Append(Itens).Append("\n");

            sb.Append("  ItensCancelados: ").Append(ItensCancelados).Append("\n");
            sb.Append("  Pagamentos: ").Append(Pagamentos).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

      

     

    }

}
