using a7D.PDV.Model.Dashboard.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BackOffice.UI.CEFAsyncBoundObjects
{
    public class BackofficeAsyncObject
    {
        public string ObterRelatorioDetalhado(double data, double? dataFim)
        {
            var date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(data)).ToLocalTime().DateTime;
            DateTime? dateFim = null;
            if (dataFim.HasValue)
                dateFim = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(dataFim.Value)).ToLocalTime().DateTime;

            var dashboard = BLL.Dashboard.ObterDashboard(date, dateFim, true);

            var result = new DashboardDetalhes
            {
                Categoria = dashboard.FaturamentoPorCategoria.Data,
                DiaDaSemana = dashboard.FaturamentoPorDiaDaSemana.Data,
                MotivosCancelamento = dashboard.MotivosCancelamento.Data,
                TipoPagamento = dashboard.FaturamentoPorTipoDePagamento.Data,
                TipoPedido = dashboard.FaturamentoPorTipoPedido.Data,
                VolumeGarcom = dashboard.VolumePorGarcom.Data
            };

            return JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public string ObterFechamento(int idFechamento)
        {
            var result = BLL.Dashboard.ObterFechamento(idFechamento);
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public string MelhoresClientes(double data, double? dataFim)
        {
            var date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(data)).ToLocalTime().DateTime;

            DateTime? dateFim = null;
            if (dataFim.HasValue)
                dateFim = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(dataFim.Value)).ToLocalTime().DateTime;

            var result = BLL.Dashboard.ObterMelhoresClientes(date, dateFim);
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public string ProdutosVendidos(string tipo, double data, double? dataFim)
        {
            var date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(data)).ToLocalTime().DateTime;

            DateTime? dateFim = null;
            if (dataFim.HasValue)
                dateFim = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(dataFim.Value)).ToLocalTime().DateTime;

            var result = BLL.Dashboard.ObterRankingProdutos(tipo, date, dateFim);
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public string ObterResumo(double data, double? dataFim)
        {
            var date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(data)).ToLocalTime().DateTime;

            DateTime? dateFim = null;
            if (dataFim.HasValue)
                dateFim = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(dataFim.Value)).ToLocalTime().DateTime;

            var result = BLL.Dashboard.ObterResumo(date, dateFim);
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
