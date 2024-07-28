// Purpose: Model for the category. It contains the properties of the category and validation rules for the category name and url.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        // Validation rules for the category name
        [Required(ErrorMessage = "Please enter a category name")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters")]
        public string Name { get; set; }

        // Validation rules for the category url
        [Required(ErrorMessage = "Please enter a category url")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Url must be between 5 and 100 characters")]
        [RegularExpression(@"^[^\s/$.?#].[^\s]*$", ErrorMessage = "Invalid URL.")]
        public string Url { get; set; }
        
        public List<Product> Products { get; set; }
    }
}