using Library.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
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
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDBContext>();
            //session
            services.AddHttpContextAccessor();
            services.AddSession(q=> {
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
