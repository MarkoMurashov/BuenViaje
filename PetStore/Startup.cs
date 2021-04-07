using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PetStore.Models.MongoDb;
using PetStore.Filters;
using BuenViaje.Models.Interfaces;
using BuenViaje.Models;
using BonVoyage.Models.Entity;
using BuenViaje.Models.Entity;

namespace PetStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:BuenViajeProducts:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:BuenViajeIdentity:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IStockRepository, EFStockRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddTransient<IFilterConditionsProducts, FilterConditionsProducts>();
            services.AddTransient<IEFContext<TransportOwner>, EFTransportOwner>();
            services.AddTransient<IEFContext<Transport>, EFTransport>();
            services.AddTransient<IVoucher, EFVoucher>();
            services.AddTransient<IEFContext<Location>, EFLocation>();
            services.AddTransient<IEFContext<LocationVoucher>, EFLocationVoucher>();
            services.AddTransient<IEFContext<Categories>, EFCategories>();
            services.AddTransient<ImagesDbContext>();
            services.AddScoped(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                );

               
                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Product", action = "List", productPage = 1 });

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
            //SeedData.EnsurePopulated(app);
            //IdentitySeedData.EnsurePopulated(app);
        }
    }
}
