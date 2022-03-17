using System;

namespace a7D.PDV.Integracao.iFood.Model.Merchant
{
    public class Error
    {
        public string code;
        public string message;
    }

    public class MerchantSummary
    {
        public string id;
        public string name;
        public string corporateName;
    }

    public class Address
    {
        public string country;
        public string state;
        public string city;
        public string postalCode;
        public string district;
        public string street;
        public string number;
        public string latitude;
        public string longitude;
    }

    public class Merchant
    {
        public string id;
        public string name;
        public string corporateName;
        public string description;
        public int averageTicket;
        public bool exclusive;
        public string type;
        public string status;
        public string createdAt;
        public Address address;
        public Operation operation;

    }

    public class Interruption
    {
        public string id;
        public string description;
        public string start;
        public string end;
    }

    public class Status
    {
        public string operation;
        public string salesChannel;
        public bool available;
        public string state;
        public Reopenable reopenable;
        public StatusValidation[] validations;
        public StatusMessage message;
    }

    public class Reopenable
    {
        public string identifier;
        public string type;
        public bool reopenable;
    }

    public class StatusValidation
    {
        public string id;
        public string code;
        public string state;
        public StatusMessage message;
    }

    public class StatusMessage
    {
        public string title;
        public string subtitle;
        public string description;
    }

    public class Operation
    {
        public string name;
        public SalesChannel salesChannel;
    }

    public class SalesChannel
    {
        public string name;
        public string enabled;
    }
}
