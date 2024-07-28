/*
* This class is used to create a many-to-many relationship between the Product and Category classes.
*/
namespace shopapp.entity
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property
        public int CategoryId { get; set; }
        public Category Category { get; set; } // Navigation property
    }
}