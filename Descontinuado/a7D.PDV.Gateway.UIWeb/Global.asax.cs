using a7D.PDV.Gateway.UIWeb.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace a7D.PDV.Gateway.UIWeb
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            var jobKey = JobKey.Create("ObterContasReceber");

            IJobDetail job = JobBuilder.Create<ObterContasReceberJob>()
                .WithIdentity(jobKey).Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("3 hour trigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(3)
                    //.WithIntervalInSeconds(30)
                    .RepeatForever()
                )
            .Build();

            //scheduler.ScheduleJob(job, trigger);
        }
    }
}
