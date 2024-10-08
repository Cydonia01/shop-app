/*
* This class is used to store the shipping address of the user.
*/
namespace shopapp.entity
{
    public class ShippingAddress
    {
        public int ShippingAddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}