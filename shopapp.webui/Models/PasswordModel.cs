// Purpose: Model for password change.

using System.ComponentModel.DataAnnotations;

namespace shopapp.webui.Models
{
    public class PasswordModel
    {
        [Required]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required]
        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password must match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}