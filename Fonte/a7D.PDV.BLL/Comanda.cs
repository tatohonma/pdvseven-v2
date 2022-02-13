using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.BLL
{
    public static class Comanda
    {
        /// <summary>
        /// Número mínimo de dígitos para usar código em vez do número da comanda
        /// </summary>
        public const int DigitosCodigo = 6;

        public static List<ComandaInformation> Listar()
        {
            var objFiltro = new ComandaInformation();

            var listaObj = CRUD.Listar(objFiltro);
            return listaObj.Cast<ComandaInformation>().ToList();
        }

        public static ComandaInformation Carregar(int idComanda)
        {
            var obj = new ComandaInformation { IDComanda = idComanda };
            obj = (ComandaInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static long HexaOuDecimalOuZero(string numero, bool hex)
        {
            try
            {
                return HexaOuDecimal(numero, hex);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static long HexaOuDecimal(string numero, bool hex)
        {
            if (numero.Length < DigitosCodigo)
            {
                if (int.TryParse(numero, out int comanda))
                    return comanda;
                else
                    throw new Exception(numero + " de comanda inválido");
            }
            else if (hex)
            {
                try
                {
                    return Convert.ToInt64(numero, 16);
                }
                catch (Exception ex)
                {
                    throw new Exception(numero + " código HEX de comanda inválido ", ex);
                }
            }
            else // Decimal
            {
                if (long.TryParse(numero, out long codigo))
                    return codigo;
                else
                    throw new Exception(numero + " código de comanda inválido");
            }
        }

        public static ComandaInformation CarregarPorNumeroOuCodigoOuHex(string texto, bool hex)
        {
            return CarregarPorNumeroOuCodigo(HexaOuDecimalOuZero(texto, hex));
        }

        public static ComandaInformation CarregarPorNumeroOuCodigo(long codigoOuNumero)
        {

            var obj = new ComandaInformation();
            if (codigoOuNumero.ToString().Length >= DigitosCodigo)
                obj.Codigo = codigoOuNumero;
            else
                obj.Numero = (int)codigoOuNumero;

            obj = (ComandaInformation)CRUD.Carregar(obj);

            if (obj.StatusComanda != null)
                obj.StatusComanda = StatusComanda.Carregar(obj.ValorStatusComanda);

            return obj;
        }

        public static ComandaInformation CarregarPorGUID(String guidIdentificacao)
        {
            var obj = new ComandaInformation();
            if (guidIdentificacao != null)
            {
                obj = new ComandaInformation { GUIDIdentificacao = guidIdentificacao };
                obj = (ComandaInformation)CRUD.Carregar(obj);

                if (obj.StatusComanda != null)
                    obj.StatusComanda = StatusComanda.Carregar(obj.ValorStatusComanda);
            }

            return obj;
        }

        public static void Salvar(ComandaInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(int idComanda)
        {
            try
            {
                var obj = new ComandaInformation { IDComanda = idComanda };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static DataTable ListarAbertas(string texto, string codigo, bool hex)
        {
            return ComandaDAL.ListarAbertas(texto, HexaOuDecimalOuZero(codigo, hex));
        }

        public static DataTable ListarTodasOuCodigo(string codigo, bool hex)
        {
            return ComandaDAL.ListarTudoOuCodigo(HexaOuDecimalOuZero(codigo, hex));
        }

        public static ComandaInformation Validar(long numeroOuCodigo, bool pagamento = false)
        {
            return ValidarComRetorno(numeroOuCodigo, pagamento)?.Comanda;
        }

        public static RetornoComanda ValidarComRetorno(long numeroOuCodigo, bool pagamento = false)
        {
            var comanda = CarregarPorNumeroOuCodigo(numeroOuCodigo);

            if (comanda.IDComanda == null)
            {
                throw new ExceptionPDV(CodigoErro.AC0A, -1, $"COMANDA {numeroOuCodigo} NÃO CADASTRADA.\n Verifique o número da comanda informada!");
            }
            else if (comanda.ValorStatusComanda == EStatusComanda.Liberada)
            {
                if (ConfiguracoesSistema.Valores.ComandaComCheckin)
                    throw new ExceptionPDV(CodigoErro.B101, 0, $"COMANDA {comanda.Numero} COM CHECKIN FECHADA! \nFavor informar o gerente.");
                else if (ConfiguracoesSistema.Valores.ComandaComCredito)
                    throw new ExceptionPDV(CodigoErro.AC0B, 0, $"COMANDA {comanda.Numero} INVALIDO! \nConfiguração inválida, é necessário configurar checkin.");
                else
                {
                    Pedido.NovoPedidoComanda(comanda.GUIDIdentificacao, null, null);
                }
            }
            else if (comanda.ValorStatusComanda == EStatusComanda.Cancelada)
            {
                throw new ExceptionPDV(CodigoErro.AC0A, 2, $"COMANDA {comanda.Numero} CANCELADA! \nFavor informar o gerente.");
            }
            else if (comanda.ValorStatusComanda == EStatusComanda.Perdida)
            {
                throw new ExceptionPDV(CodigoErro.AC0A, 1, $"COMANDA {comanda.Numero} PERDIDA! \nFavor informar o gerente.");
            }
            else if (comanda.ValorStatusComanda == EStatusComanda.ContaSolicitada && !pagamento)
            {
                throw new ExceptionPDV(CodigoErro.AC0A, 3, $"COMANDA {comanda.Numero} aguardando a conta.\n Não é possível fazer pedido!");
            }

            return new RetornoComanda
            {
                Comanda = comanda,
                ComCheckin = ConfiguracoesSistema.Valores.ComandaComCheckin, // Obsoledo
                ComCredito = ConfiguracoesSistema.Valores.ComandaComCredito  // Obsoledo
            };
        }

        public static void AlterarStatus(string guidIdentificacao, EStatusComanda eStatusComanda)
        {
            var comanda = CarregarPorGUID(guidIdentificacao);
            comanda.StatusComanda = eStatusComanda.ToObjInfo();
            Salvar(comanda);
        }

        public static bool ClienteTemComandaAberta(int clienteID)
        {
            int qtd = Repositorio.Contar<tbPedido>(p => p.IDStatusPedido == 10 && p.IDTipoPedido == 20 && p.IDCliente == clienteID);
            return qtd > 0;
        }

        public class RetornoComanda
        {
            public ComandaInformation Comanda { get; set; }
            public bool ComCheckin { get; set; }
            public bool ComCredito { get; set; }
        }

        public static PedidoInformation AbrirComanda(ComandaInformation comanda, ClienteInformation cliente, TipoEntradaInformation tipoEntrada, int pdv, int usuarioAutenticado)
        {
            var pedido = Pedido.NovoPedidoComanda(comanda.GUIDIdentificacao, cliente, tipoEntrada);

            pedido.ListaProduto = new List<PedidoProdutoInformation>();
            var produtoEntrada = Produto.Carregar(ProdutoInformation.IDProdutoEntrada);
            PedidoProdutoInformation entrada = new PedidoProdutoInformation
            {
                RetornarAoEstoque = false,
                Pedido = pedido,
                Produto = produtoEntrada,
                Quantidade = 1,
                Cancelado = false,
                ValorUnitario = tipoEntrada.ValorEntrada,
                IDTipoEntrada = tipoEntrada.IDTipoEntrada,
                Notas = tipoEntrada.Nome,
                PDV = new PDVInformation { IDPDV = pdv },
                Usuario = new UsuarioInformation() { IDUsuario = usuarioAutenticado },
                DtInclusao = DateTime.Now,
                CodigoAliquota = produtoEntrada.CodigoAliquota
            };
            PedidoProduto.Adicionar(entrada);
            pedido.ListaProduto.Add(entrada);

            return pedido;
        }

        public static bool KeyPressValid(KeyPressEventArgs e)
        {
            return e.KeyChar != (char)8
                && e.KeyChar != (char)'\u0016' // CTRL+V
                && !char.IsNumber(e.KeyChar)
                && !(e.KeyChar >= 'a' && e.KeyChar <= 'f')
                && !(e.KeyChar >= 'A' && e.KeyChar <= 'F');
        }

        public static decimal ObtemTotalConsumacao(string guidIdentificacao, string guidAgrupamentoPedido)
        {
            return Repositorio.Somar<tbPedido>(
                p => p.ValorDesconto, // Os itens pagos por credito são descontos
                p => p.IDTipoPedido == (int)ETipoPedido.Comanda
                  && p.GUIDIdentificacao == guidIdentificacao
                  && p.GUIDAgrupamentoPedido == guidAgrupamentoPedido) ?? 0;
        }

        public static decimal ObtemValorConsumacaoMinima(string guidIdentificacao, string guidAgrupamentoPedido)
        {
            return Repositorio.Carregar<tbPedido>(
                p => p.GUIDIdentificacao == guidIdentificacao
                  && p.GUIDAgrupamentoPedido == guidAgrupamentoPedido
                  && p.ValorConsumacaoMinima > 0)?.ValorConsumacaoMinima ?? 0;
        }

        public static void RegistrarVendasComCreditoNoCaixa(int idpdv, int idusuario, ref int idCaixa)
        {
            string querySql = $"SELECT COUNT(*) FROM tbPedido WHERE IDTipoPedido=20 AND IDStatusPedido=40 AND idCaixa IS NULL";
            var result = Repositorio.ExecuteScalar<int>(querySql);
            if (result > 0)
            {
                Caixa.UsaOuAbreRefID(idpdv, idusuario, ref idCaixa);
                ComandaDAL.DefinirCaixaEmPedidosComandaFechados(idCaixa);
            }
        }

        public static void FecharComandasContaCliente(int idpdv, int idusuario, ref int idCaixa)
        {
            //Essa rotina é só para quando há comandas com conta de cliente!
            try
            {
                int qtdComanda = 0;
                int qtdPedidos = 0;
                int qtdInconsistencias = 0;
                var comandas = Repositorio.Listar<tbComanda>(c => c.IDStatusComanda == (int)EStatusComanda.EmUso);
                StringBuilder info = new StringBuilder();
                foreach (var comanda in comandas)
                {
                    qtdComanda++;
                    string resumo = $"Comanda: {comanda.Numero}";
                    var pedido = Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);
                    if (pedido == null || pedido.IDPedido == null)
                    {
                        resumo += " => Pedido não encontrado";
                    }
                    else
                    {
                        // Obtem todos pedidos sem consumação
                        qtdPedidos++;
                        resumo += $" => Pedido {pedido.IDPedido}";
                        var valorConsumacaoMinima = ObtemValorConsumacaoMinima(pedido.GUIDIdentificacao, pedido.GUIDAgrupamentoPedido);
                        if (valorConsumacaoMinima > 0)
                        {
                            pedido.ValorConsumacaoMinima = valorConsumacaoMinima;
                            var valorTotal = ObtemTotalConsumacao(pedido.GUIDIdentificacao, pedido.GUIDAgrupamentoPedido);
                            if (valorTotal < pedido.ValorConsumacaoMinima)
                            {
                                var produto = Produto.Carregar(ProdutoInformation.IDProdutoEntracaCM);
                                var entradaCM = new PedidoProdutoInformation
                                {
                                    Produto = produto,
                                    Quantidade = 1,
                                    CodigoAliquota = produto.CodigoAliquota,
                                    Cancelado = false,
                                    ValorUnitario = pedido.ValorConsumacaoMinima - valorTotal,
                                    PDV = new PDVInformation() { IDPDV = idpdv },
                                    Usuario = new UsuarioInformation() { IDUsuario = idusuario },
                                    Pedido = new PedidoInformation { IDPedido = pedido.IDPedido }
                                };

                                resumo += $" + adicionado consumação mínima: R$ {entradaCM.ValorUnitarioString}";
                                pedido.ListaProduto.Add(entradaCM);
                                PedidoProduto.Salvar(entradaCM);
                            }   // Consumação mínima adicionada
                        }

                        // Se tem produtos / Serviços
                        if (pedido.ListaProduto.Any())
                        {
                            // Tenta pagar como saldo a conta do cliente
                            if (Saldo.AdicionaPagamentoPorSaldo(pedido))
                            {
                                resumo += " -> Debito de Saldo: " + pedido.ListaPagamento.Sum(p => p.Valor ?? 0).ToString("C");
                            }
                            else
                            {
                                resumo += " -> Sem saldo para finalizar pedido";
                                info.AppendLine(resumo);
                                continue;
                            }
                        }
                        else
                        {
                            resumo += " -> Vazio";
                        }

                        // Sempre fechar venda por meio dessa rotina para que seja computado o estoque
                        Caixa.UsaOuAbreRefID(idpdv, idusuario, ref idCaixa);
                        Pedido.FecharVendaDB(pedido, new CaixaInformation() { IDCaixa = idCaixa }, idusuario);
                        resumo += " -> Pedido Finalizado";
                    }

                    comanda.IDStatusComanda = (int)EStatusComanda.Liberada;
                    Repositorio.Atualizar(comanda);
                    resumo += " -> Comanda Liberada";

                    info.AppendLine(resumo);
                }

                // Procura por inconsistencia antigas nos pedidos de COMANDAS !!!
                var pedidos = Repositorio.Listar<tbPedido>(
                    p => p.IDTipoPedido == (int)ETipoPedido.Comanda
                      && p.IDStatusPedido == (int)EStatusPedido.Aberto);

                if (pedidos.Count() > 0)
                {
                    info.AppendLine();
                    info.AppendLine("Inconsistencias em pedidos (Pedidos em aberto)");
                    foreach (var pedido in pedidos)
                    {
                        qtdInconsistencias++;
                        var comanda = Repositorio.Carregar<tbComanda>(c => c.GUIDIdentificacao == pedido.GUIDIdentificacao);
                        string resumo = $"Pedido: {pedido.IDPedido}";

                        if (comanda != null)
                            // A comanda por ter sido apagada, alterado....
                            resumo += $" Comanda: {comanda.Numero.ToString()} Status: {((EStatusComanda)comanda.IDStatusComanda).ToString()}";

                        var ped = Pedido.CarregarCompleto(pedido.IDPedido);

                        if (ped.Cliente?.IDCliente == null)
                            resumo += " Sem cliente!!!";

                        else if (ped.ListaProduto.Count == 0
                              || ped.ListaProduto.Sum(p => p.ValorUnitario) == 0)
                        {
                            // Comandas sem itens, ou sem valor podem ser cancelada
                            pedido.IDStatusPedido = (int)EStatusPedido.Cancelado;
                            pedido.DtPedidoFechamento = DateTime.Now;
                            Repositorio.Atualizar(pedido);
                            resumo += " -> Pedido Cancelado";
                            if (comanda != null)
                            {
                                if (comanda.IDStatusComanda != (int)EStatusComanda.Liberada)
                                {
                                    comanda.IDStatusComanda = (int)EStatusComanda.Liberada;
                                    Repositorio.Atualizar(comanda);
                                    resumo += " -> Comanda Liberada";
                                }
                                else
                                    resumo += " -> Comanda já estava liberada";
                            }
                        }
                        else
                            resumo += " -> Pedido com itens!!!";

                        info.AppendLine(resumo);
                    }
                }

                Logs.Info(CodigoInfo.I012, info.ToString(), $"Fechamento de comandas com créditos: {qtdComanda} Pedidos: {qtdPedidos} Inconsistências: {qtdInconsistencias}");
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.ECFD, ex);
            }
        }
    }
}
