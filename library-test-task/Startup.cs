using System.IO;
using library_test_task.Data;
using library_test_task.Data.Models;
using library_test_task.Services;
using library_test_task.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace library_test_task
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
            services.AddControllersWithViews();
         
            // TODO: https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1
            // https://docs.microsoft.com/ru-ru/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1
            services.AddLogging();
            ConfigureDatabase(services);
            ConfigureDependencies(services);
            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            
            SeedData.EnsurePopulated(app);
        }

        /// <summary>
        /// Сконфигурировать конекст базы данных
        /// </summary>
        /// <param name="serviceCollection">Объект для управления сервисами приложения</param>
        void ConfigureDatabase(IServiceCollection serviceCollection)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            serviceCollection.AddDbContext<DbContext, ApplicationContext>(
                builder => builder.UseSqlite(connection).UseLazyLoadingProxies());
        }

        /// <summary>
        /// Сконфигурировать зависимости приложения
        /// </summary>
        /// <param name="serviceCollection">Объект для управления сервисами приложения</param>
        void ConfigureDependencies(IServiceCollection serviceCollection)
        {
            // DAL
            serviceCollection.AddScoped<IRepository<Customer>, RelationalRepository<Customer>>();
            serviceCollection.AddScoped<IRepository<Book>, RelationalRepository<Book>>();
            serviceCollection.AddScoped<IRepository<BookRent>, RelationalRepository<BookRent>>();
            serviceCollection.AddScoped<IRepository<BookStorage>, RelationalRepository<BookStorage>>();
            // Services
            serviceCollection.AddScoped<BookRentService>();
            serviceCollection.AddScoped<BookRentValidator>();
        }
    }
}