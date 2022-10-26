using a7D.PDV.Integracao.Servico.Core;

namespace a7D.PDV.Integracao.DeliveryOnline.API
{
    public class Orders : APIJson
    {
        public Orders(string token) : base("http://delivery.pdvseven.com.br", token)
        {
        }

        public Model.Orders.OrdersInformation GetOrders() => Get<Model.Orders.OrdersInformation>($"/api/orders");

        public string UpdateStatus(string id, int status_id) => Send<string>($"api/orders/{id}", new { status_id = status_id });
    }
}
