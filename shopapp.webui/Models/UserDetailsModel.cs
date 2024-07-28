// Purpose: Model for user details.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shopapp.webui.Models
{
    public class UserDetailsModel
    {
        public string UserId { get; set; }
        
        [Required]
        [Display(Name = "Username")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 100 characters")]
        public string UserName { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}