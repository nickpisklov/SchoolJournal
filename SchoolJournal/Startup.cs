using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal
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

          /*services.AddTransient<IAllClothing, ClothingRepository>();
            services.AddTransient<IClothitgsCategory, CategoryRepository>();
            services.AddTransient<IAllOrders, OrdersRepository>();
            services.AddDbContext<ShopDB>(options => options.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => ShopCart.GetCart(sp));*/
            
            services.AddRazorPages();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
             /* endpoints.MapControllerRoute("categoryFilter", "Clothing/{action}/{category?}", defaults: new { Controller = "Clothing", action = "List" });*/
            });

            /*
            using (var scope = app.ApplicationServices.CreateScope())
            {
                ShopDB content = scope.ServiceProvider.GetRequiredService<ShopDB>();
                DBObject.Initial(content);
            }
            */
        }
    }
}
