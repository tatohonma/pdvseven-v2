using System;

namespace a7D.PDV.Integracao.DeliveryOnline.Model.Order
{
    public class OrdersInformation
    {
        public DataInformation[] data;
    }

    public class DataInformation
    {
        public string type;
        public string id;
        public AttributesInformation attributes;
    }

    public class AttributesInformation
    {
        public Int32 customer_id;
        public string first_name;
    }
}
