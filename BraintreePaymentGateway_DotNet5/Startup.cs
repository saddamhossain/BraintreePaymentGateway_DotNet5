using BraintreePaymentGateway_DotNet5.Context;
using BraintreePaymentGateway_DotNet5.Interface;
using BraintreePaymentGateway_DotNet5.SeedData;
using BraintreePaymentGateway_DotNet5.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BraintreePaymentGateway_DotNet5
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connection = Configuration.GetConnectionString("DatabaseConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

            services.AddTransient<IUser, UserService>();

            services.AddTransient<IPayment, PaymentService>();

            services.AddTransient<IBook, BookService>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddScoped<IBraintree, BraintreeService>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            dbInitializer.Initialize();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Book}/{action=Index}/{id?}");
            });
        }
    }
}
