using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otus.Teaching.PromoCodeFactory.Core;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Serilog;
using Serilog.Events;

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {






            #region Logger

            string LogFileName = "Applog.txt";

            if (File.Exists(LogFileName)) File.Delete(LogFileName);



            Log.Logger = new LoggerConfiguration()
                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                                   .Enrich.FromLogContext()
                                   .WriteTo.File(LogFileName)
                                   .CreateLogger();

            Log.Logger.Information("Point 1");


            #endregion


            services.AddControllersWithViews();

            #region repositories

            IAsyncRepositoryT<Employee> employeesRepo = null;
            IAsyncRepositoryT<EmployeeRole> rolesRepo = null;
            IAsyncRepositoryT<ConsoleToApiMessage> messagesRepo = null;

            Log.Logger.Information($"Initializing SQLITE repos");

            EFSqliteDbContext sqLitedbContext = new EFSqliteDbContext();

            try
            {

                employeesRepo = new EfAsyncRepository<Employee>(sqLitedbContext);
                rolesRepo = new EfAsyncRepository<EmployeeRole>(sqLitedbContext);
                messagesRepo = new EfAsyncRepository<ConsoleToApiMessage>(sqLitedbContext);
                employeesRepo.InitAsync(true);

                Log.Logger.Information($"SQLITE repos  initialized ok");

                services.AddTransient(typeof(IAsyncRepositoryT<Employee>), (x) => new EfAsyncRepository<Employee>(new EFSqliteDbContext()));
                services.AddTransient(typeof(IAsyncRepositoryT<EmployeeRole>), (x) => new EfAsyncRepository<EmployeeRole>(new EFSqliteDbContext()));

                services.AddScoped(typeof(IAsyncRepositoryT<Employee>), (x) => new EfAsyncRepository<Employee>(new EFSqliteDbContext()));
                services.AddScoped(typeof(IAsyncRepositoryT<EmployeeRole>), (x) => new EfAsyncRepository<EmployeeRole>(new EFSqliteDbContext()));
                services.AddScoped(typeof(IAsyncRepositoryT<ConsoleToApiMessage>), (x) => new EfAsyncRepository<ConsoleToApiMessage>(new EFSqliteDbContext()));


            }
            catch (Exception ex)
            {
                Log.Logger.Error($"{ex.Message}");
            }
            
            #endregion

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
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
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}