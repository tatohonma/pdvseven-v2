using a7D.PDV.Gateway.UIWeb.Filters;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace a7D.PDV.Gateway.UIWeb.Controllers
{
    [JwtAuthorize]
    public class SyncController : ApiController
    {
        private readonly JobKey ObterContasReceberJobKey = JobKey.Create("ObterContasReceber");

        public class Tempos
        {
            public DateTime? last { get; set; }
            public DateTime? next { get; set; }
        }

        [HttpPost]
        [ResponseType(typeof(Tempos))]
        public IHttpActionResult Sync()
        {
            ExecutarAgora(ObterContasReceberJobKey);

            Task.Delay(1000).Wait();
            var tempos = ObterTempos(ObterContasReceberJobKey);

            return Ok(tempos);
        }

        [HttpGet]
        [ResponseType(typeof(Tempos))]
        public IHttpActionResult Get()
        {
            var tempos = ObterTempos(ObterContasReceberJobKey);

            return Ok(tempos);
        }

        private void ExecutarAgora(JobKey jobKey)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.TriggerJob(jobKey);

        }

        private Tempos ObterTempos(JobKey jobKey)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            var triggers = (List<ITrigger>)scheduler.GetTriggersOfJob(jobKey);

            var next = triggers.Where(t => t.GetNextFireTimeUtc() != null).Min(t => t.GetNextFireTimeUtc());
            var last = triggers.Where(t => t.GetPreviousFireTimeUtc() != null).Max(t => t.GetPreviousFireTimeUtc());

            var tempos = new Tempos();

            if (next.HasValue)
                tempos.next = next.Value.LocalDateTime.ToUniversalTime();

            if (last.HasValue)
                tempos.last = last.Value.LocalDateTime.ToUniversalTime();

            return tempos;
        }
    }
}
