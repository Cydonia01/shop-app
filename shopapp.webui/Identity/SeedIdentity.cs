// Purpose: Contains the SeedIdentity class which is used to seed the database with users and roles.
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using shopapp.business.Abstract;

namespace shopapp.webui.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ICartService cartService)
        {
            // Get the roles from the configuration file
            var roles = configuration.GetSection("Data:Roles").GetChildren().Select(x => x.Value).ToArray();
            // Create the roles if they do not exist
            foreach (var role in roles) {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Get the users from the configuration file
            var users = configuration.GetSection("Data:Users");
            foreach (var user in users.GetChildren())
            {
                var username = user.GetValue<string>("username");
                var password = user.GetValue<string>("password");
                var email = user.GetValue<string>("email");
                var role = user.GetValue<string>("role");
                var firstName = user.GetValue<string>("firstName");
                var lastName = user.GetValue<string>("lastName");
                var City = user.GetValue<string>("City");
                var Country = user.GetValue<string>("Country");
                var ZipCode = user.GetValue<string>("ZipCode");

                // Create the user if it does not exist
                if (await userManager.FindByNameAsync(username) == null)
                {
                    var newUser = new User()
                    {
                        UserName = username,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        City = City,
                        Country = Country,
                        ZipCode = ZipCode,
                        EmailConfirmed = true
                    };
                    // Create the user
                    var result = await userManager.CreateAsync(newUser, password);
                    // Add the user to the role
                    if(result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, role);
                        cartService.InitializeCart(newUser.Id);
                    }
                }
            }
        }
    }
}