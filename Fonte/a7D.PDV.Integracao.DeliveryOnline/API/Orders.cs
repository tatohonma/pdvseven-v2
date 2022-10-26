using a7D.PDV.Integracao.Servico.Core;

namespace a7D.PDV.Integracao.DeliveryOnline.API
{
    public class Orders : APIJson
    {
        public Orders(string token) : base("http://delivery.pdvseven.com.br", token)
        {
        }

        public Model.Order.OrdersInformation GetOrders() => Get<Model.Order.OrdersInformation>($"/api/orders");
    }
}
