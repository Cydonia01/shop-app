// Purpose: Contains the extension method to migrate the database on startup.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shopapp.data.Concrete.EfCore;
using shopapp.webui.Identity;

namespace shopapp.webui.Extensions
{
    public static class MigrationManager
    {
        // Extend the IHost interface to include a method to migrate the database
        public static IHost MigrateDatabase(this IHost host) {
            using (var scope = host.Services.CreateScope()) {
                using (var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>()) {
                    try {
                        applicationContext.Database.Migrate();
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var shopContext = scope.ServiceProvider.GetRequiredService<ShopContext>()) {
                    try {
                        shopContext.Database.Migrate();
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return host;
        }
    }
}