// Purpose: Contains the Startup class which is used to configure the application's request pipeline.
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using Microsoft.EntityFrameworkCore;
using shopapp.webui.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Http;
using shopapp.webui.EmailServices;
using Microsoft.Extensions.Configuration;

namespace shopapp.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Use tihs code for Sqlite
            // services.AddDbContext<ApplicationContext>(options => options.UseSqlite(_configuration.GetConnectionString("SqliteConnection")));
            // services.AddDbContext<ShopContext>(options => options.UseSqlite(_configuration.GetConnectionString("SqliteConnection")));
            
            services.AddDbContext<ApplicationContext>(options => options.UseMySql(_configuration.GetConnectionString("MySqlConnection")));
            services.AddDbContext<ShopContext>(options => options.UseMySql(_configuration.GetConnectionString("MySqlConnection")));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders(); // AddDefaultTokenProviders() is used to generate tokens for password reset and email confirmation

            // Password settings
            services.Configure<IdentityOptions>(options => {
                // Password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                
                
                // Lockout
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
                
                // options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            // Cookie settings
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });

            // This code is used to add the services to the container.
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // This code is used to add the SmtpEmailSender service to the container.
            services.AddScoped<IEmailSender, SmtpEmailSender>(
                service => new SmtpEmailSender(
                    _configuration["EmailSender:Host"],
                    _configuration.GetValue<int>("EmailSender:Port"),
                    _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    _configuration["EmailSender:Username"],
                    _configuration["EmailSender:Password"]
                )
            );

            services.AddControllersWithViews(); // This code is used to add the MVC services to the container.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        ICartService cartService)
        {
            app.UseStaticFiles();
            app.UseStaticFiles(
                new StaticFileOptions{
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                    RequestPath="/modules"
                }
            );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // This code is used to configure the request pipeline.
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            // This code is used to configure the endpoints.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "orders",
                    pattern: "orders",
                    defaults: new {controller="Cart", action="GetOrders"}
                );
                endpoints.MapControllerRoute(
                    name: "checkout",
                    pattern: "checkout",
                    defaults: new {controller="Cart", action="Checkout"}
                );
                endpoints.MapControllerRoute(
                    name: "cart",
                    pattern: "cart",
                    defaults: new {controller="Cart", action="Index"}
                );
                endpoints.MapControllerRoute(
                    name: "adminuseredit",
                    pattern: "admin/user/{id?}",
                    defaults: new {controller="Admin", action="UserEdit"}
                );
                endpoints.MapControllerRoute(
                    name: "adminusers",
                    pattern: "admin/user/list",
                    defaults: new {controller="Admin", action="UserList"}
                );
                endpoints.MapControllerRoute(
                    name: "adminroles",
                    pattern: "admin/role/list",
                    defaults: new {controller="Admin", action="RoleList"}
                );
                endpoints.MapControllerRoute(
                    name: "adminrolecreate",
                    pattern: "admin/role/create",
                    defaults: new {controller="Admin", action="RoleCreate"}
                );
                endpoints.MapControllerRoute(
                    name: "adminroleedit",
                    pattern: "admin/role/{id?}",
                    defaults: new {controller="Admin", action="RoleEdit"}
                );
                endpoints.MapControllerRoute(
                    name: "admincategories",
                    pattern: "admin/categories",
                    defaults: new {controller="Admin", action="CategoryList"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategorycreate",
                    pattern: "admin/categories/create",
                    defaults: new {controller="Admin", action="CategoryCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategoryedit",
                    pattern: "admin/categories/{id?}",
                    defaults: new {controller="Admin", action="CategoryEdit"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new {controller="Admin", action="ProductList"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductcreate",
                    pattern: "admin/products/create",
                    defaults: new {controller="Admin", action="ProductCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductedit",
                    pattern: "admin/products/{id?}",
                    defaults: new {controller="Admin", action="ProductEdit"}
                );
                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new {controller="Shop", action="search"}
                );
                endpoints.MapControllerRoute(
                    name: "productsdetails",
                    pattern: "{url}",
                    defaults: new {controller="Shop", action="Details"}
                );
                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new {controller="Shop", action="List"}
                );
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
            SeedIdentity.Seed(userManager, roleManager, configuration, cartService).Wait(); // This code is used to seed the database with some initial data.
        }
    }
}
