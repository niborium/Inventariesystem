using GammaltGlimmer.Data;
using GammaltGlimmer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GammaltGlimmer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                // for in memory database
                config.UseInMemoryDatabase("MemoryBaseDataBase");
            });

            // AddIdentity :-  Registers the services
            services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                // User defined password policy settings.
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Cookie settings
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "GammaltGlimmerCookie";
                config.LoginPath = "/Home/Login"; // User defined login path
                config.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            services.AddControllersWithViews();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.TryAddScoped<SignInManager<AppUser>>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios,
                // see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            CreateRoles(serviceProvider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private static void CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            Task<IdentityResult> roleResult;
            const string email = "admin@gammaltglimmer.se";

            //Check that there is an Auctioneer role and create if not
            Task<bool> hasAuctioneerRole = roleManager.RoleExistsAsync("Auctioneer");
            hasAuctioneerRole.Wait();

            if (!hasAuctioneerRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Auctioneer"));
                roleResult.Wait();
            }

            //Check if the auctioneer user exists and create it if not
            //Add to the auctioneer role

            Task<AppUser> testUser = userManager.FindByEmailAsync(email);
            testUser.Wait();

            if (testUser.Result == null)
            {
                AppUser auctioneer = new();
                auctioneer.Name = "Admin";
                auctioneer.Email = email;
                auctioneer.UserName = "Admin";

                Task<IdentityResult> newUser = userManager.CreateAsync(auctioneer, "Sommar123!");
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(auctioneer, "Auctioneer");
                    newUserRole.Wait();
                }
            }
        }
    }
}