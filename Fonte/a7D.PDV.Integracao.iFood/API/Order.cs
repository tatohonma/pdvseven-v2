using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace a7D.PDV.Integracao.iFood.API
{
    public class Order : APIJson
    {
        public Order(string token) : base("https://merchant-api.ifood.com.br", token)
        {
        }

        public Model.Order.Event[] EventsPolling() => Get<Model.Order.Event[]>("/order/v1.0/events%3Apolling");

        public Model.Order.OrderDetails OrderDetails(string id) => Get<Model.Order.OrderDetails>($"/order/v1.0/orders/{id}");

        public Model.Order.CancellationReasons[] CancellationReasons(string id) => Get<Model.Order.CancellationReasons[]>($"/order/v1.0/orders/{id}/cancellationReasons");

        public string Acknowledgment(Model.Order.Event[] eventos) => Send<string>("order/v1.0/events/acknowledgment", eventos);

        
        public string Confirm(string id) => Send<string>($"/order/v1.0/orders/{id}/confirm");

        public string RequestCancellation(string id, string cancellationCode) => Send<string>($"/order/v1.0/orders/{id}/requestCancellation", new { reason = "string", cancellationCode = cancellationCode });

        public string Dispatch(string id) => Send<string>($"/order/v1.0/orders/{id}/dispatch");
        public string ReadyToPickup(string id) => Send<string>($"/order/v1.0/orders/{id}/readyToPickup");


    }
}
