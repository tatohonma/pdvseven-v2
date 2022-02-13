using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.ERPCake.Model;
using a7D.PDV.Integracao.ERPCake.Sync;
using a7D.PDV.Integracao.Servico.Core;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace a7D.PDV.Integracao.ERPCake
{
    public class IntegraERPCake : IntegracaoTask, ICakeBase
    {
        private ConfiguracoesERP config;
        public DateTime DataInicial { get; set; }
        public DateTime UltimoSync { get; set; }
        public APIERPCake api { get; private set; }
        public int DefaultCustomer { get; private set; }
        public string HardwareSerial { get; private set; }
        public override string Nome => "Cake ERP";

        public override void Executar()
        {
            Configurado = false;
            Disponivel = BLL.PDV.PossuiERP();
            if (!Disponivel)
            {
                AddLog("Sem Licenças para ERP");
                return;
            }

            config = new ConfiguracoesERP();
            if (!config.IntegracaoERP)
            {
                AddLog("Integração Cake ERP desligada");
                return;
            }

            if (string.IsNullOrEmpty(config.Token))
            {
                AddLog("ERP Desativado");
                return;
            }

            HardwareSerial = ValidacaoSistema.RetornarSerialHD();

            if (string.IsNullOrEmpty(config.PedidoDataInicio))
            {
                DataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                AddLog("Iniciando sincronismo de pedidos a partir do mês atual: " + DataInicial.ToString("dd/MM/yyyy"));
            }
            else if (DateTime.TryParse(config.PedidoDataInicio, out DateTime dtStart))
                DataInicial = dtStart;
            else
            {
                AddLog("Defina uma data inicial válida para sincronização dos pedidos");
                return;
            }

            if (DateTime.TryParse(config._ERPUltimoSincronismo, out DateTime dtUltimo))
                UltimoSync = dtUltimo;
            else
            {
                UltimoSync = DateTime.MinValue;
                AddLog("Último sincronismo: " + DataInicial.ToString("dd/MM/yyyy"));
            }

            AddLog("Sincronizando ERP HW: " + HardwareSerial);

            try
            {
                api = new APIERPCake(config.Token);
                this.CacheClear();

                var clientePadrao = api.GetFirst<Customer>("code=PDV7");
                if (clientePadrao == null)
                {
                    clientePadrao = new Customer()
                    {
                        name = "(SEM CLIENTE)",
                        code = "PDV7",
                        address_street = HardwareSerial,
                        observation = "Este cliente será vinculado a todos os pedidos quando não for informado um cliente"
                    };
                    clientePadrao = api.Save(clientePadrao);
                    if (clientePadrao == null)
                    {
                        AddLog("Erro ao cadastrar o cliente padrão, para pedidos sem cliente");
                        return;
                    }
                    GenericSync.Insert(clientePadrao);
                    AddLog($"Inserido cliente padrão '{clientePadrao.id}: {clientePadrao.name}'");
                }
                else if (clientePadrao.address_street != HardwareSerial)
                {
                    AddLog($"Cadastro '{clientePadrao.id}: {clientePadrao.name}' precisa ter o endereço a chave de hardware '{HardwareSerial}'");
                    return;
                }
                else
                    AddLog($"Cliente Padrão '{clientePadrao.id}: {clientePadrao.name}'");

                DefaultCustomer = clientePadrao.id.Value;

                var sql = @"select CodigoERP, count(1) QTD, min(Nome) Nome from tbProduto 
where not CodigoERP is null
group by CodigoERP
having count(1)>1";

                /*
-- Apaga registros duplicados
delete from tbProduto where IDProduto in(
	select max(IDProduto) Nome from tbProduto 
	where not CodigoERP is null
	group by CodigoERP
	having count(1)>1)
    */

                var results = EF.Repositorio.Query<CodigoERP_QTD_Nome>(sql);
                if (results.Length > 0)
                {
                    var duplicados = new StringBuilder($"Registros duplicados com o mesmo código de ERP em produtos" + Environment.NewLine);
                    foreach (var item in results)
                        duplicados.AppendLine($" ! {item.CodigoERP} ({item.QTD}) {item.Nome}");

                    AddLog(duplicados.ToString());
                    var exDuplicado = new ExceptionPDV(CodigoErro.EE04);
                    exDuplicado.Data.Add("duplicados", duplicados.ToString());
                    Logs.Erro(exDuplicado);
                    return;
                }

            }
            catch (Exception ex)
            {
                AddLog(ex);
                return;
            }

            Configurado = true;
            Iniciar(() => Loop());
        }

        public void UpdateSync(DateTime dt)
        {
            ConfiguracaoBD.DefinirValorPadraoTipo(EF.Enum.EConfig._ERPUltimoSincronismo, EF.Enum.ETipoPDV.ERP, dt.ToString("dd/MM/yyyy HH:mm:ss"));
            UltimoSync = dt;
        }

        public static State[] states { get; set; }

        private void Loop()
        {
            try
            {
                bool lOK = true;
                while (lOK)
                {
                    var orders = api.All<Sales_Order>(where: "order_type=1"); // 59930
                    var fiado = EF.Repositorio.Query<int>(@"SELECT DISTINCT p.IDPedido FROM tbPedido p
INNER JOIN tbPedidoPagamento pp ON p.IDPedido = pp.IDPedido
WHERE pp.IDGateway = " + ((int)EGateway.ContaCliente) + " AND pp.IDSaldoBaixa IS NULL");
                    lOK = false;
                    int waitFiado = 0;
                    foreach (var order in orders)
                    {
                        if (fiado.Contains(order.order_number))
                        {
                            // Só fatura o que está quitado
                            waitFiado++;
                            continue;
                        }

                        if (order.invoice_model == null)
                        {
                            order.invoice_model = 59;
                            api.Save(order);
                        }
                        try
                        {
                            api.BillOrder(order.id.Value);
                            AddLog("Faturado: " + order);
                            lOK = true;
                        }
                        catch (Exception ex)
                        {
                            AddLog("Erro ao faturado: " + order + " - " + ex.Message);
                        }
                    }

                    if (waitFiado > 0)
                    {
                        AddLog($"Aguardando a quitação de {waitFiado} pedidos para faturar");
                        break;
                    }
                }

                states = api.All<State>();

                this.Sincronizar<tbUsuario, Seller>(true);
                this.Sincronizar<tbTipoPagamento, Payment_Form>(true);
                //this.Sincronizar<tbUnidade, Measure_Unit>(true); // Falta EF: CodigoERP
                this.Sincronizar<tbCategoriaProduto, Product_Category>(true);
                this.Sincronizar<tbProduto, Product>(true);

                // update tbCliente set codigoerp='' where codigoerp IN (select codigoerp from tbCliente group by codigoerp having count(*)>1)
                this.Sincronizar<tbCliente, Customer>(false);

                this.SincronizarCreditos();

                UpdateSync(DateTime.Now);

                int loop = 0;
                while (Executando && loop < 120) // 1 dia
                {
                    loop++;

                    if (this.SincronizarPedidos())
                        Sleep(5 * 60);
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EE01, ex);
            }
        }
    }
}