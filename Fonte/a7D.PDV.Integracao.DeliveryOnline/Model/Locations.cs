using System;

namespace a7D.PDV.Integracao.DeliveryOnline.Model.Locations
{
    public class LocationsInformation
    {
        public DataInformation data;
    }

    public class DataInformation
    {
        public string type;
        public string id;
        public AttributesInformation attributes;
    }

    public class AttributesInformation
    {
        public string location_address_1;
        public string location_address_2;
        public string location_city;
        public string location_state;
        public string location_postcode;
    }
}
