using System;
using System.Linq;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.ERPCake.Model;
using a7D.PDV.Integracao.ERPCake.Sync;

namespace a7D.PDV.Integracao.ERPCake.DTO
{
    public static class DTOupload
    {
        internal static Tcake UpdateOrCreate<Tpdv, Tcake>(Tpdv itemPDV, Tcake itemERP, ref string log)
            where Tpdv : class, IERP, new()
            where Tcake : ModelCake
        {
            if (itemPDV is tbProduto produto) return (Tcake)FillProduto(produto, itemERP, ref log);
            else if (itemPDV is tbCliente cliente) return (Tcake)FillCliente(cliente, itemERP);
            else if (itemPDV is tbUsuario usuario) return (Tcake)FillVendedor(usuario, itemERP, ref log);
            else if (itemPDV is tbTipoPagamento tipoPagamento) return (Tcake)FillTipoPagamento(tipoPagamento, itemERP);
            else if (itemPDV is tbUnidade unidade) return (Tcake)FillUnidade(unidade, itemERP);
            else if (itemPDV is tbCategoriaProduto categoria) return (Tcake)FillCategoria(categoria, itemERP);
            else throw new Exception($"Conversor para '{itemPDV.GetType().Name}' não implementado");
        }

        private static object FillProduto(tbProduto produtoPDV, object objERP, ref string log)
        {
            if (!(objERP is Product produtoERP))
            {
                log = "(new ERP ";
                produtoERP = new Product()
                {
                    code = produtoPDV.IDProduto.ToString(),
                };
            }
            else
                log = "(update ERP ";

            if (!produtoPDV.Ativo)
                log += " INATIVO ";

            if (produtoPDV.Excluido)
                log += " EXCLUIDO ";

            // TODO: Pensar em um cache
            var categoriaProduto = EF.Repositorio.Carregar<tbProdutoCategoriaProduto>(c => c.IDProduto == produtoPDV.IDProduto);
            if (categoriaProduto != null)
            {
                var categoria = EF.Repositorio.Carregar<tbCategoriaProduto>(c => c.IDCategoriaProduto == categoriaProduto.IDCategoriaProduto);
                if (categoria != null && int.TryParse(categoria.CodigoERP, out int codERP))
                {
                    log += categoria.Nome;
                    produtoERP.category = codERP;
                }
            }

            produtoERP.name = produtoPDV.Nome.Replace("'", "\'");
            produtoERP.price_sell = produtoPDV.ValorUnitario;
            produtoERP.active = produtoPDV.Ativo && !produtoPDV.Excluido;

            if (produtoPDV.IDTipoProduto == (int)ETipoProduto.Ingrediente
             || produtoPDV.IDTipoProduto == (int)ETipoProduto.Modificacao)
            {
                produtoERP.product_type = 3; // Produto para produção
                produtoERP.production_type = "T";
            }
            else
            {
                var qtd = Repositorio.Contar<tbProdutoReceita>(r => r.IDProduto == produtoPDV.IDProduto);
                if (qtd == 0)
                {
                    produtoERP.production_type = "T";
                    produtoERP.product_type = 1; // Produto Pronto
                }
                else
                {
                    produtoERP.production_type = "P";
                    produtoERP.product_type = 2; // Produto Produzido
                }
            }

            if (produtoPDV.IDTipoProduto == (int)ETipoProduto.Servico || produtoPDV.IDTipoProduto == (int)ETipoProduto.Credito)
                produtoERP.down_stock = false;
            else
                produtoERP.down_stock = produtoPDV.ControlarEstoque;

            log += $" - {produtoERP.production_type})";

            return produtoERP;
        }

        private static object FillCliente(tbCliente clientePDV, object objERP)
        {
            if (!(objERP is Customer clienteERP))
            {
                clienteERP = new Customer()
                {
                    code = clientePDV.IDCliente.ToString(),
                };
            }

            clienteERP.name = clientePDV.NomeCompleto;

            // O CPF é usado como busca para evitar duplicidades
            clienteERP.doc_cpf = string.IsNullOrEmpty(clientePDV.Documento1) ? null : clientePDV.Documento1;

            clienteERP.active = !clientePDV.Bloqueado;
            clienteERP.address_street = clientePDV.Endereco;
            clienteERP.address_number = clientePDV.EnderecoNumero;
            clienteERP.address_complement = clientePDV.Complemento;
            clienteERP.address_zip_code = clientePDV.CEP?.ToString();
            clienteERP.BirthdayConvert = clientePDV.DataNascimento;
            clienteERP.gender = clientePDV.Sexo;
            clienteERP.contact_email = clientePDV.Email;
            clienteERP.observation = clientePDV.Observacao;

            if (clientePDV.Telefone1DDD.HasValue && clientePDV.Telefone1Numero.HasValue)
                clienteERP.contact_phone = $"({clientePDV.Telefone1DDD}) {clientePDV.Telefone1Numero}";

            if (clientePDV.Telefone2DDD.HasValue && clientePDV.Telefone2Numero.HasValue)
                clienteERP.contact_cellphone = $"({clientePDV.Telefone2DDD}) {clientePDV.Telefone2Numero}";

            var estado = Repositorio.Carregar<tbEstado>(e => e.IDEstado == clientePDV.IDEstado);
            if (estado != null)
            {
                var state = IntegraERPCake.states.FirstOrDefault(s => s.symbol == estado.Sigla);
                if (state != null)
                    clienteERP.address_state = state.id;
            }

            return clienteERP;
        }

        private static object FillVendedor(tbUsuario usuarioPDV, object objERP, ref string log)
        {
            if (!(objERP is Seller vendedorERP))
            {
                log = "(new ERP";
                vendedorERP = new Seller()
                {
                    code = usuarioPDV.IDUsuario.ToString(),
                };
            }
            else
                log = "(update ERP";

            vendedorERP.name = usuarioPDV.Nome;
            vendedorERP.active = usuarioPDV.Ativo && !usuarioPDV.Excluido;

            if (!usuarioPDV.Ativo)
                log += " INATIVO ";

            if (usuarioPDV.Excluido)
                log += " EXCLUIDO ";

            log += ")";

            return vendedorERP;
        }

        private static object FillTipoPagamento(tbTipoPagamento tipoPagamento, object objERP)
        {
            if (!(objERP is Payment_Form formaPagamento))
            {
                formaPagamento = new Payment_Form()
                {
                    description = tipoPagamento.IDTipoPagamento.ToString(),
                    minimum_parcel = 0,
                    visible = true,
                    allow_change = true,
                    change_only_first_parcel = true,
                    automatic_payment = true,
                    automatic_receipt = true,
                    payment_type = DTOtradutor.PaymentType(tipoPagamento.IDMeioPagamentoSAT)
                };
            }

            formaPagamento.name = tipoPagamento.Nome;
            formaPagamento.visible_pdv = tipoPagamento.Ativo;

            return formaPagamento;
        }

        private static object FillUnidade(tbUnidade unidadePDV, object objERP)
        {
            if (!(objERP is Measure_Unit unidadeERP))
                unidadeERP = new Measure_Unit();

            unidadeERP.name = unidadePDV.Nome;
            unidadeERP.symbol = unidadePDV.Simbolo;

            return unidadeERP;
        }

        private static object FillCategoria(tbCategoriaProduto categoriaPDV, object objERP)
        {
            if (!(objERP is Product_Category categoriaERP))
                categoriaERP = new Product_Category();

            categoriaERP.name = categoriaPDV.Nome;
            categoriaERP.external_code = categoriaPDV.IDCategoriaProduto.ToString();

            return categoriaERP;
        }
    }
}
