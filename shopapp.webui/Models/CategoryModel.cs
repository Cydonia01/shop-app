
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        // [Required(ErrorMessage = "Please enter a category name")]
        // [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters")]
        public string Name { get; set; }
        // [Required(ErrorMessage = "Please enter a category url")]
        // [StringLength(100, MinimumLength = 5, ErrorMessage = "Url must be between 5 and 100 characters")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}