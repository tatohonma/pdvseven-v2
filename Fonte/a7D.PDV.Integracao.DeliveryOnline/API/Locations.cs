using a7D.PDV.Integracao.Servico.Core;

namespace a7D.PDV.Integracao.DeliveryOnline.API
{
    public class Locations : APIJson
    {
        public Locations(string token) : base("http://delivery.pdvseven.com.br", token)
        {
        }

        public Model.Locations.LocationsInformation GetLocations(int id) => Get<Model.Locations.LocationsInformation>($"/api/locations/{id}");
    }
}
