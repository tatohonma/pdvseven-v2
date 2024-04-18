using System;

namespace a7D.PDV.Integracao.DeliveryOnline.Model.Orders
{
    public class OrdersInformation
    {
        public DataInformation[] data;
        public IncludedInformation[] included;
    }

    public class DataInformation
    {
        public string type;
        public string id;
        public AttributesOrderInformation attributes;
    }

    public class IncludedInformation
    {
        public string type;
        public string id;
        public AttributesAddessInformation attributes;
    }

    public class AttributesAddessInformation
    {
        public int? address_id;
        public int? customer_id;
        public string address_1;
        public string address_2;
        public string city;
        public string state;
        public string postcode;
        public int country_id;
    }

    public class AttributesOrderInformation
    {
        public int? customer_id;
        public string first_name;
        public string last_name;
        public string email;
        public string telephone;
        public int location_id;
        public int? address_id;
        public string comment;
        public string payment;
        public string order_type;
        //...
        public StatusInformation status;
        public OrderTotalsInformation[] order_totals;
        public OrderMenusInformation[] order_menus;
    }

    public class StatusInformation
    {
        public Int32 status_id;
        public String status_name;
    }

    public class OrderTotalsInformation
    {
        public string code;
        public string title;
        public string value;
    }

    public class OrderMenusInformation
    {
        public Int32 menu_id;
        public string name;
        public Decimal quantity;
        public string price;
        public string subtotal;
        public string comment;
        public MenuOptions[] menu_options;
    }

    public class MenuOptions
    {
        public Int32 menu_id;
        public string order_option_name;
        public string order_option_price;
        public Decimal quantity;
        public string order_option_category;
    }
}
