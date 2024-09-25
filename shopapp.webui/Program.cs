// Purpose: Main entry point for the application. This class is responsible for creating the host builder and running the application.
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using shopapp.webui.Extensions;

namespace shopapp.webui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDatabase().Run(); // MigrateDatabase() is an extension method
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Startup class is responsible for configuring the application
                });
    }
}
