using System;

namespace a7D.PDV.Integracao.iFood.Model.Merchant
{
    public class Error
    {
        string code;
        string message;
    }

    public class MerchantSummary
    {
        string id;
        string name;
        string corporateName;
    }

    public class Address
    {
        string country;
        string state;
        string city;
        string postalCode;
        string district;
        string street;
        string number;
        string latitude;
        string longitude;
    }

    public class Merchant
    {
        string id;
        string name;
        string corporateName;
        string description;
        int averageTicket;
        bool exclusive;
        string type;
        string status;
        string createdAt;
        Address address;
        Operation operation;

    }

    public class Interruption
    {
        string id;
        string description;
        string start;
        string end;
    }

    public class Status
    {
        string operation;
        string salesChannel;
        bool available;
        string state;
        Reopenable reopenable;
        StatusValidation validations;
        StatusMessage message;
    }

    public class Reopenable
    {
        string identifier;
        string type;
        bool reopenable;
    }

    public class StatusValidation
    {
        string id;
        string code;
        string state;
        StatusMessage message;
    }

    public class StatusMessage
    {
        string title;
        string subtitle;
        string description;
    }

    public class Operation
    {
        string name;
        SalesChannel salesChannel;
    }

    public class SalesChannel
    {
        string name;
        string enabled;
    }
}
