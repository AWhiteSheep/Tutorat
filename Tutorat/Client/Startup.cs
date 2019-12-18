using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Client.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client
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
            // ajout du context pour la base de données
            services.AddDbContext<TutoratCoreContext>(options =>
                options.UseSqlServer(
                    // connectionn string
                    Configuration.GetConnectionString("DefaultConnection")));
            // ajout de l'identity asp uases fait et ficelé
            services.AddIdentity<AspNetUsers, IdentityRole>(options => {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.SignIn.RequireConfirmedAccount = false;
                }).AddDefaultUI()
                .AddEntityFrameworkStores<TutoratCoreContext>();
            // ajout des controlleurs et des views comme service
            services.AddControllersWithViews();
            // ajout de la notation razor dans la page comme service
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // utiise la redirection et l'utilisation de fichier static comme le wwwroot
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // middleware de routage fait à chaque demande
            app.UseRouting();
            // ajout de l'authentification et des authorization comme middleware
            app.UseAuthentication();
            app.UseAuthorization();
            // ajout des endpoints default de l'application
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
