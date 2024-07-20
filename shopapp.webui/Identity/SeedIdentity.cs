using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using shopapp.business.Abstract;
using shopapp.data.Concrete;

namespace shopapp.webui.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ICartService cartService)
        {

            var roles = configuration.GetSection("Data:Roles").GetChildren().Select(x => x.Value).ToArray();
            foreach (var role in roles) {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var users = configuration.GetSection("Data:Users");
            foreach (var user in users.GetChildren())
            {
                var username = user.GetValue<string>("username");
                var password = user.GetValue<string>("password");
                var email = user.GetValue<string>("email");
                var role = user.GetValue<string>("role");
                var firstName = user.GetValue<string>("firstName");
                var lastName = user.GetValue<string>("lastName");

                if (await userManager.FindByNameAsync(username) == null)
                {
                    var newUser = new User()
                    {
                        UserName = username,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newUser, password);
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