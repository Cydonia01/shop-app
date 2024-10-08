/*
*   This class is used to define the Order entity.
*   It contains the properties of the Order entity.
*   It also contains the EnumPaymentType and EnumOrderState enums.
*/
using System;
using System.Collections.Generic;

namespace shopapp.entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public int BillingAddressId { get; set; }
        public int ShippingAddressId { get; set; }
        public int CardId { get; set; }
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public BillingAddress BillingAddress { get; set; } // Navigation property
        public ShippingAddress ShippingAddress { get; set; } // Navigation property
        public Card Card { get; set; } // Navigation property
        public EnumOrderState OrderState { get; set; }
        public EnumPaymentType PaymentType { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public enum EnumPaymentType
    {
        CreditCard = 0,
        Eft = 1
    }

    public enum EnumOrderState {
        waiting = 0,
        unpaid = 1,
        completed = 2
    }
}