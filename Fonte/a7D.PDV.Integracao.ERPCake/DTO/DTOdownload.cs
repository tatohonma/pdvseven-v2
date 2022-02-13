using System;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.ERPCake.Model;

namespace a7D.PDV.Integracao.ERPCake.DTO
{
    public static class DTOdownload
    {
        internal static Tpdv UpdateOrCreate<Tpdv, Tcake>(Tcake itemERP, Tpdv itemPDV, out string log)
            where Tpdv : class, IERP, new()
            where Tcake : ModelCake
        {
            if (itemERP is Product produto) return (Tpdv)FillProduto(produto, itemPDV, out log);
            else if (itemERP is Customer cliente) return (Tpdv)FillCliente(cliente, itemPDV, out log);
            else if (itemERP is Seller usuario) return (Tpdv)FillVendedor(usuario, itemPDV, out log);
            else if (itemERP is Payment_Form tipoPagamento) return (Tpdv)FillTipoPagamento(tipoPagamento, itemPDV, out log);
            else if (itemERP is Measure_Unit unidade) return (Tpdv)FillTipoUnidade(unidade, itemPDV, out log);
            else if (itemERP is Product_Category category) return (Tpdv)FillCategoria(category, itemPDV, out log);
            else throw new Exception($"Conversor para '{itemPDV.GetType().Name}' não implementado");
        }

        private static object FillProduto(Product produtoERP, object objPDV, out string log)
        {
            if (!(objPDV is tbProduto produtoPDV))
            {
                log = "(new PDV";
                produtoPDV = new tbProduto()
                {
                    CodigoERP = produtoERP.id.ToString(),
                    IDTipoProduto = (int)ETipoProduto.Item,
                    DtAlteracaoDisponibilidade = DateTime.Now,
                };
            }
            else
                log = "(update PDV";

            produtoPDV.Nome = produtoERP.name;
            produtoPDV.ValorUnitario = produtoERP.price_sell ?? 0;
            produtoPDV.ControlarEstoque = produtoERP.down_stock == true;
            produtoPDV.Ativo = produtoERP.active == true;
            produtoPDV.DtUltimaAlteracao = DateTime.Now;

            if (!produtoPDV.Ativo)
                log += " INATIVO";

            if (produtoPDV.Excluido)
                log += " EXCLUIDO";

            log += ")";

            return produtoPDV;
        }

        private static object FillCliente(Customer clienteERP, object objPDV, out string log)
        {
            if (!(objPDV is tbCliente clientePDV))
            {
                log = "(new PDV)";
                clientePDV = new tbCliente()
                {
                    CodigoERP = clienteERP.id.ToString(),
                    DtInclusao = DateTime.Now
                };
            }
            else
                log = "(update PDV)";

            clientePDV.NomeCompleto = clienteERP.name;
            clientePDV.Documento1 = clienteERP.doc_cpf;
            clientePDV.Bloqueado = clienteERP.active == false;
            clientePDV.Endereco = clienteERP.address_street;
            clientePDV.EnderecoNumero = clienteERP.address_number;
            clientePDV.Complemento = clienteERP.address_complement;
            //clientePDV.CEP = clienteERP.address_zip_code;
            //clienteERP.contact_phone = $"({clientePDV.Telefone1DDD}) {clientePDV.Telefone1Numero}";
            //clienteERP.contact_cellphone = $"({clientePDV.Telefone2DDD}) {clientePDV.Telefone2Numero}";
            clientePDV.DataNascimento = clienteERP.BirthdayConvert;
            clientePDV.Sexo = clienteERP.gender;
            clientePDV.Email = clienteERP.contact_email;
            clientePDV.Observacao = clienteERP.observation;

            return clientePDV;
        }

        private static object FillVendedor(Seller vendedorERP, object objPDV, out string log)
        {
            if (!(objPDV is tbUsuario usuarioPDV))
            {
                log = "(new PDV)";
                usuarioPDV = new tbUsuario()
                {
                    CodigoERP = vendedorERP.id.ToString(),
                    DtUltimaAlteracao = DateTime.Now,
                    Excluido = false
                };
            }
            else
                log = "(update PDV)";

            usuarioPDV.Nome = vendedorERP.name;
            usuarioPDV.Ativo = vendedorERP.active == true;

            return usuarioPDV;
        }

        private static object FillTipoPagamento(Payment_Form tipoPagamentoERP, object objPDV, out string log)
        {
            if (!(objPDV is tbTipoPagamento formaPagamentoPDV))
            {
                log = "(new PDV)";
                formaPagamentoPDV = new tbTipoPagamento()
                {
                    CodigoERP = tipoPagamentoERP.id.ToString(),
                    Ativo = false // É necessário ativar outros campos no BackOffice
                };
            }
            else
                log = "(update PDV)";

            formaPagamentoPDV.Nome = tipoPagamentoERP.name;

            return formaPagamentoPDV;
        }

        private static object FillTipoUnidade(Measure_Unit unidadeERP, object objPDV, out string log)
        {
            if (!(objPDV is tbUnidade unidadePDV))
            {
                log = "(new PDV)";
                unidadePDV = new tbUnidade();
            }
            else
                log = "(update PDV)";

            unidadePDV.Nome= unidadeERP.name;
             unidadePDV.Simbolo= unidadeERP.symbol;

            return unidadePDV;
        }

        private static object FillCategoria(Product_Category categoria, object objPDV, out string log)
        {
            if (!(objPDV is tbCategoriaProduto categoriaPDV))
            {
                log = "(new PDV)";
                categoriaPDV = new tbCategoriaProduto();
            }
            else
                log = "(update PDV)";

            categoriaPDV.Nome = categoria.name;
            categoriaPDV.CodigoERP = categoria.id.ToString();

            return categoriaPDV;
        }
    }
}
