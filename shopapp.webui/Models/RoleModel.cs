using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using shopapp.webui.Identity;

namespace shopapp.webui.Models
{
    public class RoleModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }
    }

    public class RoleDetails {
        public IdentityRole Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }

    public class RoleEditModel {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}