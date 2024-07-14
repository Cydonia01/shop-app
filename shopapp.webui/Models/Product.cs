using System.ComponentModel.DataAnnotations;

namespace shopapp.webui.Models {
    public class Product {
        public int ProductId { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 5,ErrorMessage = "Product name must be between 5 and 60 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a price")]
        [Range(1,10000)]
        public double? Price { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}