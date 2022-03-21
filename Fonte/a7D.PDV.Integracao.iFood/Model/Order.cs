using System;
using a7D.PDV.Integracao.iFood.Model;

namespace a7D.PDV.Integracao.iFood.Model.Order
{
    public class AdditionalFee
    {
        public string description;
        public string type;
        public decimal value;
    }
    public class Value
    {
        public string currency;
        public decimal value;
    }
    public class Benefit
    {
        public string type;
        public Value value;
        public SponsorshipValue sponmsorshipValue;
        public string target;
    }
    public class SponsorshipValue
    {
        public Value own;
        public Value partner;
    }
    public class CancellationRequest
    {
        public string description;
        public string cancellationCode;
    }

    public class CashInformation
    {
        public string description;
        public decimal changeFor;
    }

    public class CommonOrderDetails
    {
        public Benefit benefit;
        public string orderType;
        public Payments payments;
        public Merchant merchant;
        public string salesChannel;
        public Picking picking;
        public string orderTiming;
        public string createdAt;
        public Total total;
        public string preparationStartDateTime;
        public string id;
        public string displayId;
        public Item[] items;
        public Customer customer;
        public string extraInfo;
        public AdditionalFee additionalFees;
    }

    public class Coordinates
    {
        public float latitude;
        public float longitude;
    }

    public class CreditCardInformation
    {
        public string brand;
    }

    public class Customer
    {
        public Phone phone;
        public string documentNumber;
        public string name;
        public int ordersCountOnMerchant;
        public string id;
    }

    public class DeliveryAddress
    {
        public string reference;
        public string country;
        public string streetName;
        public string formattedAddress;
        public string streetNumber;
        public string city;
        public string postalCode;
        public Coordinates coordinates;
        public string neighborhood;
        public string state;
        public string complement;
    }

    public class DeliveryInformation
    {
        public string mode;
        public string deliveredBy;
        public DeliveryAddress deliveryAddress;
        public string deliveryDateTime;
    }

    public class DigitalWalletInformation
    {
        public string name;
    }

    public class Error
    {
        public string code;
        public string field;
        public string[] details;
        public string message;
    }

    public class ErrorDTO
    {
        public string code;
        public string field;
        public string[] details;
        public string message;
        public string[] unauthorizedMerchants;
    }

    public class ErrorView
    {
        public ErrorDTO error;
    }

    public class Event
    {
        public string createdAt;
        public string fullCode;
        public Object metadata;
        public string code;
        public string orderId;
        public string id;
    }

    public class IndoorInformation
    {
        public string mode;
        public string deliveryDateTime;
        public string table;
    }

    public class Item
    {
        public Decimal unitPrice;
        public int quantity;
        public string externalCode;
        public Decimal totalPrice;
        public int index;
        public string unit;
        public string ean;
        public Decimal price;
        public ItemScalePrices sclaePrices;
        public string observations;
        public string imageUrl;
        public string name;
        public ItemOption[] options;
        public string id;
        public Decimal optionPrice;
    }

    public class ItemScalePrices
    {
        public Decimal defaultPrice;
        public ScalePrice[] scales;
    }

    public class ScalePrice
    {
        public int minQuantity;
        public Decimal price;
    }

    public class ItemOption
    {
        public Decimal unitPrice;
        public string unit;
        public string ean;
        public int quantity;
        public string externalCode;
        public Decimal price;
        public string name;
        public int index;
        public string id;
        public Decimal addition;
    }

    public class Merchant
    {
        public string name;
        public string id;
    }

    public class OrderDetails
    {
        public string orderType;
        public Benefit[] benefits;
        public Payments payments;
        public Merchant merchant;
        public string salesChannel;
        public Picking picking;
        public string orderTiming;
        public string createdAt;
        public Total total;
        public string preparationStartDateTime;
        public string id;
        public string displayId;
        public Item[] items;
        public Customer customer;
        public string extraInfo;
        public AdditionalFee[] additionalFees;
        public DeliveryInformation delivery;
        public ScheduleInformation schedule;
        public IndoorInformation indoor;
        public TakeoutInformation takeout;
    }

    public class PaymentMethod
    {
        public DigitalWalletInformation wallet;
        public string method;
        public bool prepaid;
        public string currency;
        public string type;
        public decimal value;
        public CashInformation cash;
        public CreditCardInformation card;
    }

    public class Payments
    {
        public decimal pending;
        public decimal prepaid;
        public PaymentMethod[] methods;
    }

    public class Phone
    {
        public string number;
        public string localizer;
        public string localizerExpiration;
    }

    public class Picking
    {
        public string picker;
        public string reclacementOptions;
    }

    public class ScheduleInformation
    {
        public string deliveryDateTimeStart;
        public string deliveryDateTimeEnd;
    }
    //public class Sponsorship
    //{
    //    public SponsorshipValue own;
    //    public SponsorshipValue partner;
    //}
    //public class SponsorshipValue
    //{
    //    public string currency;
    //    public decimal value;
    //}
    public class TakeoutInformation
    {
        public string mode;
        public string takeoutDateTime;
    }

    public class Total
    {
        public Decimal benefits;
        public Decimal deliveryFee;
        public Decimal orderAmount;
        public Decimal subTotal;
        public Decimal additionalFees;
    }

    public class Tracking
    {
        public float deliveryEtaEnd;
        public string expectedDelivery;
        public float latitude;
        public float longitude;
        public float pickupEtaStart;
        public float trackDate;
    }
}
