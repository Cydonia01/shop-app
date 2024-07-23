using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Display(Name="Name")]
        [Required(ErrorMessage="Please enter a product name.")]
        [StringLength(60, MinimumLength=5, ErrorMessage="Name must be between 5 and 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage="Please enter a url.")]
        [StringLength(100, MinimumLength=5, ErrorMessage="Url must be between 5 and 100 characters.")]
        [RegularExpression(@"^[^\s/$.?#].[^\s]*$", ErrorMessage = "Invalid URL.")]
        public string Url { get; set; }

        [Required(ErrorMessage="Please enter a price.")]
        [Range(1,1000000,ErrorMessage = "Price must be between 1 and 1000000.")]
        public double? Price { get; set; }

        [Required(ErrorMessage="Please enter a description.")]
        public string Description { get; set; }

        [Required(ErrorMessage="Please enter a image url.")]
        public string ImageUrl { get; set; }

        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}