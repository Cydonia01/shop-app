/*
* This class is used to represent the CartItem entity in the database.
*/
namespace shopapp.entity
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property
        public int CartId { get; set; }
        public Cart Cart { get; set; } // Navigation property
        public int Quantity { get; set; }
    }
}