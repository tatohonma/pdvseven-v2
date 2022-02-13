using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Linq;

namespace a7D.PDV.Integracao.API2.Model
{
    public static class DTO
    {
        public static Categoria Converter(tbCategoriaProduto categoriaProduto)
        {
            return new Categoria(
                categoriaProduto.IDCategoriaProduto,
                categoriaProduto.Nome,
                categoriaProduto.Disponibilidade != false,
                categoriaProduto.DtAlteracaoDisponibilidade,
                categoriaProduto.DtUltimaAlteracao);
        }


        public static Categoria ConverterBasico(tbCategoriaProduto categoriaProduto)
        {
            return new Categoria(
                categoriaProduto.IDCategoriaProduto,
                categoriaProduto.Nome,
                categoriaProduto.Disponibilidade != false);
        }

        public static AreaDeImpressao Converter(tbAreaImpressao areaImpressao)
        {
            return new AreaDeImpressao(
                areaImpressao.IDAreaImpressao,
                areaImpressao.Nome,
                areaImpressao.IDTipoAreaImpressao);
        }

        public static Item Converter(PedidoProdutoInformation pedidoProduto)
        {
            var modificacoes = pedidoProduto?.ListaModificacao?.Select(pp => new Item
            {
                IDProduto = pp.Produto.IDProduto.Value,
                Qtd = pp.Quantidade,
                Preco = pp.ValorUnitario,
                Notas = pp.Notas,
            })?.ToList();

            return new Item(pedidoProduto?.IDPedidoProduto, pedidoProduto?.Produto?.IDProduto, pedidoProduto?.Produto?.Nome, pedidoProduto?.Quantidade, modificacoes, pedidoProduto?.Notas, pedidoProduto?.ValorUnitario);
        }

        public static Pagamento Converter(PedidoPagamentoInformation pedidoPagamento)
        {

            var contaRecebivel = "";
            var bandeira = "";

            if (pedidoPagamento.ContaRecebivel != null && pedidoPagamento.ContaRecebivel.IDContaRecebivel > 0)
                contaRecebivel = BLL.ContaRecebivel.Carregar(pedidoPagamento.ContaRecebivel.IDContaRecebivel).Nome;
            if (pedidoPagamento.Bandeira != null)
                bandeira = BLL.Bandeira.Carregar(pedidoPagamento.Bandeira.IDBandeira).Nome;
            if (pedidoPagamento.MeioPagamentoSAT == null || pedidoPagamento.MeioPagamentoSAT.IDMeioPagamentoSAT == null)
                throw new Exception("Erro no construtor de API Pagamento. Meio de Pagamento inválido.");


            return new Pagamento(pedidoPagamento?.IDPedidoPagamento,
                new TipoPagamento
                {
                    IDTipoPagamento = pedidoPagamento.TipoPagamento.IDTipoPagamento,
                    CodigoERP = pedidoPagamento.TipoPagamento.CodigoERP,
                    Nome = pedidoPagamento.TipoPagamento.Nome,
                },

                valor: pedidoPagamento?.Valor,
                autorizacao: pedidoPagamento.Autorizacao,
                contaRecebivel: contaRecebivel,
                bandeira: bandeira,
                metodo: pedidoPagamento.MeioPagamentoSAT.IDMeioPagamentoSAT
                );
        }

        public static Cliente Converter(ClienteInformation cliente)
        {
            var obj = new Cliente(cliente?.IDCliente, cliente?.NomeCompleto, cliente?.Telefone1DDD, cliente?.Telefone1Numero, cliente?.Telefone2DDD, cliente?.Telefone2Numero, cliente?.Documento1, cliente?.RG, cliente?.Email, DtNascimento: cliente?.DataNascimento);

            if (cliente != null)
            {
                obj.Sexo = cliente.Sexo?.ToLower() == "m" ? "M" : "F";
                obj.Endereco = new ClienteEndereco(Convert.ToString(cliente.CEP), cliente.Endereco, cliente.EnderecoNumero, cliente.EnderecoReferencia, cliente.Bairro, cliente.Cidade, cliente.EnderecoReferencia, cliente?.Estado?.Sigla);
            }

            return obj;
        }

        public static Cliente Converter(EF.Models.tbCliente cliente)
        {
            var obj = new Cliente(cliente?.IDCliente, cliente?.NomeCompleto, cliente?.Telefone1DDD, cliente?.Telefone1Numero, cliente?.Telefone2DDD, cliente?.Telefone2Numero, cliente?.Documento1, cliente?.RG, cliente?.Email, DtNascimento: cliente?.DataNascimento);

            if (cliente != null)
            {
                // TODO: cliente?.IDEstado.Estado?.Sigla;
                string uf = "";
                obj.Sexo = cliente.Sexo?.ToLower() == "m" ? "M" : "F";
                obj.Endereco = new ClienteEndereco(Convert.ToString(cliente.CEP), cliente.Endereco, cliente.EnderecoNumero, cliente.EnderecoReferencia, cliente.Bairro, cliente.Cidade, cliente.EnderecoReferencia, uf);
            }

            return obj;
        }

        public static ContaRecebivel Converter(EF.Models.tbContaRecebivel contaRecebivelInformation)
        {
            var conta = new ContaRecebivel();
            conta.CodigoIntegracao = contaRecebivelInformation.CodigoIntegracao;
            conta.Nome = contaRecebivelInformation.Nome;
            conta.IDContaRecebivel = contaRecebivelInformation.IDContaRecebivel;
            return conta;
        }

        public static Pedido Converter(PedidoInformation pedido)
        {
            return new Pedido(pedido?.IDPedido,
                pedido.NumeroComanda,
                pedido.NumeroMesa,
                DTO.Converter(pedido?.Cliente),
                pedido?.TipoPedido?.IDTipoPedido,
                pedido?.StatusPedido?.IDStatusPedido,
                DTO.Converter(pedido?.TipoEntrada),
                pedido?.GUIDIdentificacao, pedido?.DtPedido,
                pedido?.DtPedidoFechamento, pedido?.DtEnvio,
                pedido?.DtEntrega, pedido?.ValorServico, pedido?.ValorDesconto,
                pedido?.ValorEntrega, pedido?.ValorTotal, pedido?.NumeroPessoas);
        }

        public static TipoPagamento Converter(TipoPagamentoInformation tipoPagamento)
        {
            var obj = new TipoPagamento(tipoPagamento?.IDTipoPagamento, tipoPagamento?.Nome);

            if (tipoPagamento.MeioPagamentoSAT != null)
            {
                obj.CodigoPagamentoSat = tipoPagamento.MeioPagamentoSAT.Codigo;
                obj.MeioPagamento = new MeioPagamento
                {
                    IDMeioPagamento = tipoPagamento.MeioPagamentoSAT.IDMeioPagamentoSAT.Value,
                    Descricao = tipoPagamento.MeioPagamentoSAT.Descricao
                };
            }

            if (tipoPagamento.ContaRecebivel != null)
                obj.ContaRecebivel = new ContaRecebivel { IDContaRecebivel = tipoPagamento.ContaRecebivel.IDContaRecebivel };

            return obj;
        }

        public static TipoEntrada Converter(TipoEntradaInformation tipoEntrada)
        {
            return new TipoEntrada(tipoEntrada?.IDTipoEntrada, tipoEntrada?.Nome, tipoEntrada?.ValorEntrada, tipoEntrada?.ValorConsumacaoMinima);
        }
    }
}
