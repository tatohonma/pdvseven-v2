using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.Servico.Core;
using a7D.PDV.Model;
using System;
using System.Linq;

namespace a7D.PDV.Integracao.iFood
{
    enum StatusLoja
    {
        none, aberta, fechada
    }

    public partial class IntegraIFood : IntegracaoTask
    {
        APIiFood ifoodAPI = null;
        PDVInformation ifoodPDV;
        PDVInformation ifoodCaixaPDV;
        CaixaInformation ifoodCaixa;
        UsuarioInformation ifoodUsuario;
        TipoPagamentoInformation ifoodPagamento;
        TipoPagamentoInformation ifoodDinheiro;
        TipoPagamentoInformation ifoodCredito;
        TipoPagamentoInformation ifoodDebito;
        TipoPagamentoInformation ifoodRefeicao;
        TipoPagamentoInformation ifoodOutros;
        TaxaEntregaInformation ifoodTaxa;
        ConfiguracoesIFood config;
        DateTime UltimaVerificacao = DateTime.MinValue;
        DateTime UltimaAlteracao = DateTime.Now.AddYears(-1);
        StatusLoja status;

        public override string Nome => "Delivery IFood";

        public override void Executar()
        {
            Configurado = false;
            Disponivel = BLL.PDV.PossuiIFOOD();
            if (!Disponivel)
            {
                AddLog("Sem Licenças para iFood");
                return;
            }

            config = new ConfiguracoesIFood();
            if (!config.IntegracaoIFood)
            {
                AddLog("Integração iFood desligada");
                return;
            }

            if (string.IsNullOrEmpty(config.merchant_id)
             || config.CaixaPDV == 0
             || string.IsNullOrEmpty(config.username)
             || string.IsNullOrEmpty(config.password)
             || string.IsNullOrEmpty(config.ChaveUsuario))
            {
                AddLog("Falta configurar o iFood no Configurador");
                return;
            }

            var pdvs = BLL.PDV.Listar();
            ifoodPDV = pdvs.FirstOrDefault(p => p.TipoPDV.Tipo == EF.Enum.ETipoPDV.IFOOD);
            if (ifoodPDV == null)
            {
                Disponivel = false;
                AddLog("Licenças para iFood removida");
                return;
            }

            bool valido = true;

            ifoodCaixaPDV = pdvs.FirstOrDefault(p => p.IDPDV == config.CaixaPDV && p.TipoPDV.Tipo == ETipoPDV.CAIXA);
            if (ifoodCaixaPDV == null)
            {
                AddLog($"Caixa ID PDV: {config.CaixaPDV} inválido!");
                valido = false;
            }

            // Todos os pagamentos priorizando os pagamentos ativos!
            var listaPagamentos = TipoPagamento.Listar().OrderByDescending(p => p.Ativo);
            ifoodCaixa = null;
            if ((ifoodPagamento = listaPagamentos.FirstOrDefault(
                p => p.IDGateway == (int)EGateway.iFood)) == null)
            {
                AddLog("Não há um meio de pagamento com Gateway 'iFood' cadastrado no Backoffice");
                valido = false;
            }

            if ((ifoodDinheiro = listaPagamentos.FirstOrDefault(
                p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Dinheiro))) == null)
            {
                AddLog("Não há um meio de pagamento 'Dinheiro' disponível cadastrado no Backoffice");
                valido = false;
            }

            if ((ifoodDebito = listaPagamentos.FirstOrDefault(
                p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Debito))) == null)
            {
                AddLog("Não há um meio de pagamento 'Cartão de Débito' disponível cadastrado no Backoffice");
                valido = false;
            }

            if ((ifoodCredito = listaPagamentos.FirstOrDefault(
                p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Credito))) == null)
            {
                AddLog("Não há um meio de pagamento 'Cartão de Credito' disponível cadastrado no Backoffice");
                valido = false;
            }

            if ((ifoodRefeicao = listaPagamentos.FirstOrDefault(
                p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Refeicao))) == null)
            {
                AddLog("Não há um meio de pagamento 'Vale Refeição' disponível cadastrado no Backoffice");
                valido = false;
            }

            if ((ifoodOutros = listaPagamentos.FirstOrDefault(
                p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Outros))) == null)
            {
                AddLog("Não há um meio de pagamento 'Outros' disponível cadastrado no Backoffice");
                valido = false;
            }

            if ((ifoodTaxa = TaxaEntrega.CarregarPorNome("iFood")) == null)
            {
                AddLog("Não há uma taxa de entrega com o nome 'iFood' cadastrada no Backoffice");
                valido = false;
            }

            if (!valido)
                return;

            try
            {
                ifoodUsuario = Usuario.Autenticar(config.ChaveUsuario);
            }
            catch (ExceptionPDV ex)
            {
                AddLog(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar o usuário, pela chave informada", ex);
            }

            Configurado = true;
            Iniciar(() => Loop());
        }

        private void Loop()
        {
            try
            {
                AddLog("Integração iFood: Ativada");
                status = StatusLoja.none;

                while (Executando)
                {
                    if (ifoodAPI == null)
                    {
                        ifoodAPI = new APIiFood();

                        var auth = ifoodAPI.Autenticar(config.merchant_id, config.username, config.password);
                        if (Executando = (auth == "OK"))
                            AddLog($"IFOOD PDV {ifoodPDV.IDPDV}:{ifoodPDV.Nome} - CAIXA PDV: {ifoodCaixaPDV.IDPDV}:{ifoodCaixaPDV.Nome} - Usuário {ifoodUsuario.IDUsuario}:{ifoodUsuario.Nome} => API Token: {ifoodAPI.Token?.Length} Bytes Valido até {ifoodAPI.TokenValidade.ToString("HH:mm:ss")}");
                        else
                            AddLog("Erro ao autenticar na API do iFood: " + auth);

                    }
                    else if (DateTime.Now > ifoodAPI.TokenValidade.AddMinutes(-5))
                    {
                        // Antes do token expirar, destroi a instancia para refazer um novo token
                        AddLog("Recriando Token");
                        ifoodAPI = null;
                        continue;
                    }

                    StatusLoja novoStatus = status;
                    string statusConfig = ConfiguracaoBD.ValorOuPadrao(EConfig.HabilitarIFood, ifoodPDV);

                    if (statusConfig == "0")
                    {
                        // Antes de tudo o que vale é a configuração, se está aberta ou fechada
                        if (novoStatus == StatusLoja.aberta)
                        {
                            novoStatus = StatusLoja.fechada;
                            AddLog("Loja desativada pela configuração");
                        }
                    }
                    else if (ifoodCaixa == null)
                    {
                        ifoodCaixa = Caixa.ListarAbertos().FirstOrDefault(c => c.PDV.IDPDV == ifoodCaixaPDV.IDPDV);
                        if (ifoodCaixa == null)
                        {
                            // Sem caixa aberto, fecha a loja (notifica só uma vez)
                            if (novoStatus != StatusLoja.fechada)
                            {
                                novoStatus = StatusLoja.fechada;
                                AddLog($"Aguardando a abertura do caixa {ifoodCaixaPDV.IDPDV}:{ifoodCaixaPDV.Nome}");
                            }
                        }
                        else
                        {
                            novoStatus = StatusLoja.aberta;
                            AddLog($"Caixa {ifoodCaixaPDV.IDPDV}:{ifoodCaixaPDV.Nome} aberto em {ifoodCaixa.DtAbertura}");
                        }
                    }
                    else
                    {
                        // Sempre revalida o caixa, pois pode ter sido fechado
                        ifoodCaixa = Caixa.Carregar(ifoodCaixa.IDCaixa.Value);
                        if (ifoodCaixa.DtFechamento.HasValue)
                        {
                            AddLog($"Caixa {ifoodCaixaPDV.IDPDV}:{ifoodCaixaPDV.Nome} fechado em {ifoodCaixa.DtFechamento}");
                            ifoodCaixa = null;
                            novoStatus = StatusLoja.fechada;
                        }
                        else if (novoStatus == StatusLoja.fechada)
                        {
                            novoStatus = StatusLoja.aberta;
                            AddLog("Loja ativada pela configuração");
                        }
                    }

                    // Controle unificado quando há alteração de estado
                    if (status != novoStatus)
                    {
                        status = novoStatus;
                        if (status == StatusLoja.aberta)
                        {
                            ifoodAPI.StatusLoja(true, "aberto");
                            AddLog("LOJA IFOOD ABERTA! Aguardando pedidos...");
                        }
                        else
                        {
                            ifoodAPI.StatusLoja(false, "Vendas interrompida temporariamente");
                            AddLog("LOJA IFOOD FECHADA!");
                        }
                    }

                    if (status == StatusLoja.aberta)
                        Sleep(30);
                    else
                    {
                        Sleep(60);
                        continue;
                    }

                    if (DateTime.Now.Subtract(UltimaVerificacao).TotalMinutes > 5)
                        EnviarPrecosDisponibilidade();

                    EnviaConfirmacoes();

                    var novos = ifoodAPI.EventoPendentes();
                    if (novos == null)
                        continue;

                    foreach (var evento in novos)
                    {
                        try
                        {
                            AddLog($"{evento.correlationId} => {evento.code}");

                            if (evento.code == "PLACED")
                                InserePedido(evento);
                            else if (evento.code == "CANCELLED")
                                CancelaPedido(evento);

                            ifoodAPI.EventoLido(evento.id);
                        }
                        catch (Exception ex)
                        {
                            AddLog(new ExceptionPDV(CodigoErro.EE17, ex, evento.code), true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("token expired") && !ex.Message.Contains("Invalid access"))
                    throw new ExceptionPDV(CodigoErro.EE11, ex);
            }
        }
    }
}
