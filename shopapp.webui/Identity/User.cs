// Purpose: Contains the User class which extends IdentityUser class to add additional properties to the user class.
using Microsoft.AspNetCore.Identity;

namespace shopapp.webui.Identity
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}