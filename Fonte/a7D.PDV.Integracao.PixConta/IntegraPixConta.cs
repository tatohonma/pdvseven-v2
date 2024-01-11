//using a7D.Fmk.CRUD.DAL;
//using a7D.PDV.BLL;
//using a7D.PDV.EF.Enum;
//using a7D.PDV.Integracao.Servico.Core;
//using a7D.PDV.Model;
//using Newtonsoft.Json;
using a7D.PDV.Integracao.Servico.Core;
using System;
using a7D.PDV.BLL;
using a7D.PDV.Model;
using System.Linq;
using a7D.PDV.EF.Enum;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Integracao.PixConta
{
    public partial class IntegraPixConta : IntegracaoTask
    {
        public override string Nome => "Pix-Conta";
        
        ConfiguracoesPixConta ConfigPixConta;
        UsuarioInformation UsuarioPixConta;
        PDVInformation PDVPixConta;
        TipoPagamentoInformation TipoPagamentoPixConta;

        API.Invoice APIInvoice;

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

            ConfigPixConta = new ConfiguracoesPixConta();

            if(!ConfigPixConta.ContaCliente)
            {
                AddLog("Integração Pix-Conta Conta-Cliente desligada no Configurador");
                return false;
            }

            if (string.IsNullOrEmpty(ConfigPixConta.Token_IUGU))
            {
                AddLog("Falta configurar o Token da API do IUGU no Configurador");
                return false;
            }

            var listaUsuario = Usuario.Listar();
            UsuarioPixConta = listaUsuario.FirstOrDefault(u => u.Nome == "PixConta");
            if(UsuarioPixConta == null)
            {
                UsuarioPixConta = new UsuarioInformation();

                UsuarioPixConta.Nome = "PixConta";
                UsuarioPixConta.Ativo = true;
                UsuarioPixConta.DtUltimaAlteracao = DateTime.Now;

                UsuarioPixConta.PermissaoAdm = false;
                UsuarioPixConta.PermissaoCaixa = false;
                UsuarioPixConta.PermissaoGarcom = false;
                UsuarioPixConta.PermissaoGerente = false;
                UsuarioPixConta.Excluido = false;

                CRUD.Adicionar(UsuarioPixConta);
                AddLog("Usuario 'PixConta' cadastrado!");
            }

            var listaPagamentos = TipoPagamento.Listar().OrderByDescending(p => p.Ativo);
            TipoPagamentoPixConta = listaPagamentos.FirstOrDefault(p => p.IDGateway == (int)EGateway.PixConta);
            if (TipoPagamentoPixConta == null)
            {
                TipoPagamentoPixConta = new TipoPagamentoInformation();
                TipoPagamentoPixConta.MeioPagamentoSAT = new MeioPagamentoSATInformation { IDMeioPagamentoSAT = 10 };

                TipoPagamentoPixConta.Nome = "PixConta";
                TipoPagamentoPixConta.CodigoImpressoraFiscal = "PixConta";
                TipoPagamentoPixConta.Ativo = false;
                TipoPagamentoPixConta.RegistrarValores = false;
                TipoPagamentoPixConta.IDGateway = (int)EGateway.PixConta;

                CRUD.Adicionar(TipoPagamentoPixConta);
                AddLog("Tipo Pagamento com Gateway 'PixConta' cadastrado!");
            }

            var pdvs = BLL.PDV.Listar();
            PDVPixConta = pdvs.FirstOrDefault(p => p.IDPDV == ConfigPixConta.IDPDV && p.TipoPDV.Tipo == ETipoPDV.CAIXA);
            if (PDVPixConta == null)
            {
                AddLog($"ID PDV do Caixa: {ConfigPixConta.IDPDV} inválido!");
                configurado = false;
            }

            return configurado;
        }

        private void Loop()
        {
            try
            {
                AddLog("Integração Pix-Conta: Ativada");

                while (Executando)
                {
                    if (APIInvoice == null)
                        APIInvoice = new API.Invoice(ConfigPixConta.Token_IUGU);

                    AddLog("Verificar Pagamentos");
                    VerificarPagamentosPendentes();

                    CancelarFaturas();

                    Sleep(30);
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro na integração Pix-Conta: " + ex.Message);
                AddLog("Reinicie o Integrador para restabelecer essa integração...\r\nCaso não resolva, entre em contato com o suporte!!!");

                AddLog("Detalhes: \n" + ex.ToString());
            }
        }
    }
}