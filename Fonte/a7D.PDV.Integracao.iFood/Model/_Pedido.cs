using System;

namespace a7D.PDV.Integracao.iFood
{
    // https://developer.ifood.com.br/v1.0/reference#ordersreference
    public class PedidoIFood
    {
        public string id;                   // "ff80808161091d9601610adffc211dbf",   //Id de referencia interno
        public string reference;            // "5101428012317050",    // Id de referencia do pedido
        public string shortReference;       // "3409",   //Extranet Id
        public DateTime createdAt;          // "2018-01-18T20:05:06.177Z",    //Timestamp do pedido
        public string type;                 // "DELIVERY", //Tipo do pedido("DELIVERY" ou "TOGO")
        public MerchantIFood merchant;      // Dados do restaurante
        public DateTime deliveryDateTime;   // "2018-01-18T20:35:06.177Z"  //Timestamp do pedido
        public PagamentoIFood[] payments;
        public ClienteIFood customer;
        public ItemIFood[] items;
        public decimal subTotal;            // Total do pedido(Sem taxa de entrega)
        public decimal totalPrice;          // Total do pedido(Com taxa de entrega)
        public decimal deliveryFee;         // Taxa de entrega
        public EnderecoIFood deliveryAddress;

        public override string ToString()
        {
            string cliente = customer?.ToString() ?? "SEM CLIENTE";
            string endereco = deliveryAddress?.ToString() ?? "SEM ENDEREÇO";
            string pagamento = "";
            if (payments != null)
            {
                foreach (var pay in payments)
                    pagamento += "   " + pay.ToString() + "\r\n";
            }
            string produtos = "";
            foreach (var item in items)
                produtos += item.ToString(1) + "\r\n";

            produtos += $"Total: {subTotal} + {deliveryFee} = {totalPrice}";
            return $"iFood Pedido #{shortReference} em {createdAt.ToString("dd/MM/yy HH:mm")}\r\nCliente: {cliente}\r\nEntrega: {endereco}\r\nPagamentos:\r\n{pagamento}Produtos:\r\n{produtos}";
        }
    }

    public class MerchantIFood
    {
        public int id;                      // "10487",  //Identificador unico do restaurante⁎
        public string name;                 // "Modelo Area", Nome do restaurante
        public string[] phones;             // Telefone do restaurante "(11) 12345679" ],
        public EnderecoIFood address;      // Endereço do estabelecimento
    }

    public class EnderecoIFood
    {
        public string formattedAddress;     // "R Teste",  //Endereço formatado
        public string country;              // "BR",    //Pais
        public string state;                // "AC",  //Estado
        public string city;                 // "BUJARI",   //Cidade
        public string neighborhood;         // "OUTROS",   //Bairro
        public string streetName;           // "R Teste",    //Endereço (Tipo logradouro + Logradouro)
        public string streetNumber;         //  Numero
        public string complement;           // Complemento
        public string postalCode;           // "12345678"    //CEP
        public string reference;            // Referencia

        public override string ToString()
        => $"{formattedAddress} {complement} - {neighborhood} - {city} CEP: {postalCode}";
    }

    public class PagamentoIFood
    {
        public string name;             // "VISA", //Nome da forma de pagamento
        public string code;             // "VIS",  //Codigo da forma de pagamento⁎⁎⁎
        public decimal value;           // 28,    //Valor pago na forma
        public bool prepaid;            // true,    //Pedido pago ("true" ou "false")
        public string externalCode;     // "40",   //Codigo da forma de pagamento⁎⁎⁎⁎
        public string issuer;           // "VISA"    //Bandeira
        public decimal? changeFor;       // Troco para

        public override string ToString()
        => $"{name} {code} {(changeFor > 0 ? ("(" + changeFor.Value.ToString("N2") + ")") : "")} {value.ToString("N2")} {(prepaid ? "PRÉ-PAGO" : "A PAGAR")} {externalCode} {issuer}";

    }

    public class ClienteIFood
    {
        public string id;               // "1751813",    //Id do cliente
        public string name;             // "Teste ifood",  //Nome do cliente
        public string taxPayerIdentificationNumber; // "01234567890",  //CPF/CNPJ do cliente 
        public string phone;            // "19 - 966363963",  //Telefone do cliente
        public string email;            // "TESTECOMERCIAL@IFOOD.COM.BR"  //Email do cliente
        public int? ordersCountOnRestaurant;
        public override string ToString()
        => $"{id} {name} {(ordersCountOnRestaurant > 0 ? ("(Pedidos: " + ordersCountOnRestaurant + ")") : "(NOVO)")} {phone} {email} {taxPayerIdentificationNumber}";
    }

    public class ItemIFood
    {
        public string name;             // "X-salada", //Nome do item
        public decimal quantity;        // 1,  //Quantidade
        public decimal price;           // 8, //Preço
        public decimal? subItemsPrice;   // 3, //Preço dos subitens
        public decimal totalPrice;      // 11,   //Preço total
        public decimal discount;        // 0,  //Desconto
        public decimal addition;        // 0,  //Adição
        public string externalCode;     // "8",    //Código do e-PDV
        public string observations;     // "BEM CAPRICHADO!"   //Observação do item
        public ItemIFood[] subItems;

        public string ToString(int nivel)
        {
            string espacos = new string(' ', nivel * 3);
            string produto = $"{espacos}{quantity.ToString("0")} x {name} R$ {price.ToString("N2")}";
            if (subItems != null && subItems.Length > 0)
            {
                foreach (var si in subItems)
                    produto += "\r\n" + si.ToString(nivel + 1);
            }
            return produto;
        }

        public override string ToString()
            => ToString(1);

    }
}
