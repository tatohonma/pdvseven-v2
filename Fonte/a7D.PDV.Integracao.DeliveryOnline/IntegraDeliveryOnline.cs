using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.Servico.Core;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace a7D.PDV.Integracao.DeliveryOnline
{
    enum eStatusLoja
    {
        none, aberta, fechada
    }

    public partial class IntegraDeliveryOnline : IntegracaoTask
    {
        public override string Nome => "Delivery Online";

        ConfiguracoesDeliveryOnline ConfigDO;
        TaxaEntregaInformation TaxaEntregaDO;
        UsuarioInformation UsuarioDO;
        PDVInformation PDVDO;
        TipoDescontoInformation TipoDescontoDO;
        ProdutoInformation TaxaAdicional;

        TipoPagamentoInformation PagamentoPixConta;
        TipoPagamentoInformation PagamentoDinheiro;
        TipoPagamentoInformation PagamentoCredito;
        TipoPagamentoInformation PagamentoDebito;
        TipoPagamentoInformation PagamentoRefeicao;
        TipoPagamentoInformation PagamentoOutros;

        API.Orders APIOrders;
        API.Locations APILocations;

        public override void Executar()
        {
            //if (!ValidarLicenca())'
            //    return;

            if (!ValidarConfiguracoes())
                return;

            Iniciar(() => Loop());
        }

        private bool ValidarConfiguracoes()
        {
            Boolean configurado = true;

            ConfigDO = new ConfiguracoesDeliveryOnline();

            if (!ConfigDO.IntegracaoDeliveryOnline)
            {
                AddLog("Integração Delivery Online desligada no Configurador");
                return false;
            }

            if (string.IsNullOrEmpty(ConfigDO.Location_id)
             || string.IsNullOrEmpty(ConfigDO.Username)
             || string.IsNullOrEmpty(ConfigDO.Password)
             || string.IsNullOrEmpty(ConfigDO.DeviceName))
            {
                AddLog("Falta configurar o acesso a API do Delivery Online no Configurador (location_id, username, password e device-name)");
                configurado = false;
            }

            var pdvs = BLL.PDV.Listar();
            PDVDO = pdvs.FirstOrDefault(p => p.IDPDV == ConfigDO.CaixaPDV && p.TipoPDV.Tipo == ETipoPDV.CAIXA);
            if (PDVDO == null)
            {
                AddLog($"Caixa ID PDV: {ConfigDO.CaixaPDV} inválido!");
                configurado = false;
            }

            TaxaEntregaDO = TaxaEntrega.CarregarPorNome("Delivery Online");
            if (TaxaEntregaDO.IDTaxaEntrega == null)
            {
                TaxaEntregaDO = new TaxaEntregaInformation();
                TaxaEntregaDO.Nome = "Delivery Online";
                TaxaEntregaDO.Ativo = true;
                TaxaEntregaDO.Excluido = false;
                TaxaEntregaDO.Valor = 0;

                CRUD.Adicionar(TaxaEntregaDO);

                AddLog("Taxa Entrega 'Delivery Online' adicionada!");
            }

            TipoDescontoDO = (TipoDescontoInformation)CRUD.Carregar(new TipoDescontoInformation { Nome = "Delivery Online" });
            if (TipoDescontoDO.IDTipoDesconto == null)
            {
                TipoDescontoDO = new TipoDescontoInformation();
                TipoDescontoDO.Nome = "Delivery Online";
                TipoDescontoDO.Ativo = true;
                TipoDescontoDO.Excluido = false;

                CRUD.Adicionar(TipoDescontoDO);

                AddLog("Tipo Desconto 'Delivery Online' adicionado!");
            }

            TaxaAdicional = (ProdutoInformation)CRUD.Carregar(new ProdutoInformation { Nome = "Serviço Delivery Online" });
            if (TaxaAdicional.IDProduto == null)
            {
                TaxaAdicional = new ProdutoInformation();
                TaxaAdicional.Nome = "Serviço Delivery Online";
                TaxaAdicional.TipoProduto = new TipoProdutoInformation { IDTipoProduto = 10 };
                TaxaAdicional.ValorUnitario = 0;
                TaxaAdicional.Ativo = true;
                TaxaAdicional.Disponibilidade = true;
                TaxaAdicional.Excluido = false;
                TaxaAdicional.DtAlteracaoDisponibilidade = DateTime.Now;
                TaxaAdicional.DtUltimaAlteracao = DateTime.Now;
                TaxaAdicional.ClassificacaoFiscal = new ClassificacaoFiscalInformation { IDClassificacaoFiscal = 2 };
                TaxaAdicional.Unidade = new UnidadeInformation { IDUnidade = 1 };
                TaxaAdicional.ControlarEstoque = false;
                TaxaAdicional.UtilizarBalanca = false;
                TaxaAdicional.AssistenteModificacoes = false;
                TaxaAdicional.GUIDIdentificacao = Guid.NewGuid().ToString();

                CRUD.Adicionar(TaxaAdicional);
                AddLog("Produto com o nome 'Serviço Delivery Online' cadastrado!");
            }

            var listaPagamentos = TipoPagamento.Listar().OrderByDescending(p => p.Ativo);

            PagamentoRefeicao = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Refeicao));
            if (PagamentoRefeicao == null)
            {
                PagamentoRefeicao = new TipoPagamentoInformation();
                PagamentoRefeicao.Nome = "Vale Refeição";
                PagamentoRefeicao.CodigoImpressoraFiscal = "0";
                PagamentoRefeicao.RegistrarValores = true;
                PagamentoRefeicao.Ativo = true;
                PagamentoRefeicao.MeioPagamentoSAT = new MeioPagamentoSATInformation { IDMeioPagamentoSAT = 7 };

                CRUD.Adicionar(PagamentoRefeicao);
                AddLog("Pagamento 'Vale Refeição' cadastrado!");
            }

            PagamentoPixConta = listaPagamentos.FirstOrDefault(p => p.IDGateway == (int)EGateway.PixConta);
            if (PagamentoPixConta == null)
            {
                PagamentoPixConta = new TipoPagamentoInformation();
                PagamentoPixConta.MeioPagamentoSAT = new MeioPagamentoSATInformation { IDMeioPagamentoSAT = 10 };

                PagamentoPixConta.Nome = "PixConta";
                PagamentoPixConta.CodigoImpressoraFiscal = "PixConta";
                PagamentoPixConta.Ativo = true;
                PagamentoPixConta.RegistrarValores = false;
                PagamentoPixConta.IDGateway = (int)EGateway.PixConta;

                CRUD.Adicionar(PagamentoPixConta);
                AddLog("Pagamento com Gateway 'PixConta' cadastrado!");
            }

            PagamentoDinheiro = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Dinheiro));
            if (PagamentoDinheiro == null)
            {
                AddLog("Não há um meio de pagamento 'Dinheiro' disponível cadastrado no Backoffice");
                configurado = false;
            }

            PagamentoDebito = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Debito));
            if (PagamentoDebito == null)
            {
                AddLog("Não há um meio de pagamento 'Cartão de Débito' disponível cadastrado no Backoffice");
                configurado = false;
            }

            PagamentoCredito = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Credito));
            if (PagamentoCredito == null)
            {
                AddLog("Não há um meio de pagamento 'Cartão de Credito' disponível cadastrado no Backoffice");
                configurado = false;
            }

            PagamentoOutros = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Outros));
            if (PagamentoOutros == null)
            {
                AddLog("Não há um meio de pagamento 'Outros' disponível cadastrado no Backoffice");
                configurado = false;
            }

            try
            {
                UsuarioDO = Usuario.Autenticar(ConfigDO.ChaveUsuario);
            }
            catch (ExceptionPDV ex)
            {
                configurado = false;
                AddLog(ex.Message);
            }
            catch (Exception ex)
            {
                configurado = false;
                AddLog("Erro ao carregar o usuário pela chave informada ( " + ConfigDO.ChaveUsuario + ")");
                AddLog(ex.Message);
            }

            return configurado;
        }

        private void Loop()
        {
            try
            {
                //EventosRecebidos = new List<string>();
                AddLog("Integração Delivery Online: Ativada");

                while (Executando)
                {
                    if (String.IsNullOrEmpty(ConfigDO.Token))
                    {
                        AddLog("Autenticando...");
                        if (!Autenticar())
                        {
                            Sleep(60);
                            continue;
                        }
                    }

                    if (APIOrders == null)
                        APIOrders = new API.Orders(ConfigDO.Token);

                    if (APILocations == null)
                        APILocations = new API.Locations(ConfigDO.Token);

                    AddLog("Lendo pedidos...");
                    LerPedidos();

                    AddLog("Atualizando status no APP...");
                    AtualizarStatus();

                    Sleep(60);
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro na integração Delivery Online: " + ex.Message);
                AddLog("Reinicie o Integrador para restabelecer essa integração...\r\nCaso não resolva, entre em contato com o suporte!!!");
                //if (!ex.Message.Contains("token expired") && !ex.Message.Contains("Invalid access"))
                //    throw new ExceptionPDV(CodigoErro.EE11, ex);

                AddLog(ex.ToString());
            }
        }

        private void AtualizarStatus()
        {
            var pedidos = ListarPedidosNaoSincronizados();

            foreach (var pedido in pedidos)
            {
                Int32 idPedido = Convert.ToInt32(((DataRow)pedido)["IDPedido"]);
                Int32 idStatusPedido = Convert.ToInt32(((DataRow)pedido)["IDStatusPedido"]);
                string guidIdentificacao = ((DataRow)pedido)["guidIdentificacao"].ToString();
                String order_id = ((DataRow)pedido)["order_id"].ToString();

                switch (idStatusPedido)
                {
                    case 10:
                        APIOrders.UpdateStatus(order_id, 3);
                        Tag.Alterar(guidIdentificacao, "DeliveryOnline-status_id", "3");
                        AddLog($"Status do Pedido {idPedido} (order-id {order_id}) alterado no app para 'Preparação'");
                        break;
                    case 20:
                        APIOrders.UpdateStatus(order_id, 4);
                        Tag.Alterar(guidIdentificacao, "DeliveryOnline-status_id", "4");
                        AddLog($"Status do Pedido {idPedido} (order-id {order_id}) alterado no app para 'Enviado'");
                        break;
                    case 40:
                        APIOrders.UpdateStatus(order_id, 5);
                        Tag.Alterar(guidIdentificacao, "DeliveryOnline-status_id", "5");
                        AddLog($"Status do Pedido {idPedido} (order-id {order_id}) alterado no app para 'Finalizado'");
                        break;
                    case 50:
                        APIOrders.UpdateStatus(order_id, 9);
                        Tag.Alterar(guidIdentificacao, "DeliveryOnline-status_id", "9");
                        AddLog($"Status do Pedido {idPedido} (order-id {order_id}) alterado no app para 'Cancelado'");
                        break;
                }
            }
        }

        private void LerPedidos()
        {
            var listaLocationId = ConfigDO.Location_id.Split(';');
            Int32 qtdPedidos = 0;

            foreach (var locationId in listaLocationId)
            {
                var pedidos = APIOrders.GetOrders(Convert.ToInt32(locationId.Trim()), 2);

                if (pedidos != null && pedidos.data.Length > 0)
                {
                    foreach (var pedido in pedidos.data)
                    {
                        if (pedido.attributes.order_type == "delivery")
                        {
                            var endereco = pedidos.included.FirstOrDefault(include => include.type == "addresses" && include.id == pedido.attributes.address_id.ToString());
                            AdicionarPedido(pedido, endereco.attributes);
                        }
                        else
                        {
                            AdicionarPedido(pedido, null);
                        }
                        qtdPedidos++;
                    }
                }
            }

            if (qtdPedidos == 0)
                AddLog(" -- Sem pedidos!");
        }

        private Boolean Autenticar()
        {
            API.Auth apiAuth = new API.Auth();

            try
            {
                Model.Token.Token token = apiAuth.Token(ConfigDO.Username, ConfigDO.Password, ConfigDO.DeviceName);

                if (!String.IsNullOrEmpty(token.token))
                {
                    ConfigDO.Token = token.token;
                    AtualizarToken(token.token);

                    AddLog("Autorizado e Token gerado!");
                    AddLog(ConfigDO.Token);
                    return true;
                }
                else
                {
                    AddLog("Falha na autentição!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AddLog($"Erro na geração do Token: " + ex.Message);

                ConfigDO.Token = "";
                AtualizarToken("");

                return false;
            }
        }
        private void AtualizarToken(string token)
        {
            ConfiguracaoBDInformation config = ConfiguracaoBD.BuscarConfiguracao("Token", 250);
            config.Valor = token;
            CRUD.Alterar(config);
        }
    }
}