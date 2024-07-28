// Purpose: Contains the OrderModel class which is used to represent the order information in the web application. Also contains the ShipModel, BillModel, and CardModel classes which are used to represent the shipping, billing, and card information in the order.

namespace shopapp.webui.Models
{
    public class OrderModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public ShipModel ShipModel { get; set; }
        public BillModel BillModel { get; set; }
        public CardModel CardModel { get; set; }
        public CartModel CartModel { get; set; }
    }

    public class ShipModel {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

    public class BillModel {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

    public class CardModel {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvc { get; set; }
    }
}