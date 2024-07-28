/*
* This class is used to store the billing address of the user.
*/
namespace shopapp.entity
{
    public class BillingAddress
    {
        public int BillingAddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}