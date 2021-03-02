using Library.Data;
using Library.Models;
using Library.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddControllersWithViews();
            //connection string
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

            //Identity
            //1-inhirite from identitydbcontext
            //2-installing Microsoft.AspNetCore.Identity.UI Package
            //3-add service
            //4-scafolled idenity 
            //5-add endpoint
            //To Add Role To Services We Change  AddDefaultIdentity to AddIdentity & Add IdentityRole 
            //With AddDefaultTokenProviders & AddDefaultUI
            //services.AddIdentity<IdentityUser,IdentityRole>()
            services.AddIdentity<AppUser,IdentityRole>()
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDBContext>();

            services.AddTransient<IEmailSender, EmailSender>();
            //Configure Identity
            #region Identity Configurations
            services.Configure<IdentityOptions>(options =>
              {
                  // Password settings.
                  options.Password.RequireDigit = false;
                  options.Password.RequireLowercase = false;
                  options.Password.RequireNonAlphanumeric = false;
                  options.Password.RequireUppercase = false;
                  options.Password.RequiredLength = 6;
                  options.Password.RequiredUniqueChars = 0;

                  // Lockout settings.
                  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                  options.Lockout.MaxFailedAccessAttempts = 5;
                  options.Lockout.AllowedForNewUsers = true;

                  // User settings.
                  options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                  options.User.RequireUniqueEmail = false;
              });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            #endregion
            //session
            services.AddHttpContextAccessor();
            services.AddSession(q =>
            {
                q.IdleTimeout = TimeSpan.FromMinutes(10);
                q.Cookie.HttpOnly = true;
                q.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            //Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();
            //session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                //identity
                //map identity razor pages
                endpoints.MapRazorPages();
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
