using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using EjarTech.Services.LanguageServies;
using EjarTech.Services.LanguageServies.Helpers;
using EjarTech.Models.ConfigurationModel.Database;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using EjarTech.Services.DatabaseServices.Connection;
using EjarTech.Services.AuthServices;
using EjarTech.Services.AuthServices.Helpers;
using EjarTech.Models.ConfigurationModel;
using EjarTech.Services.MessageService.Helpers;
using EjarTech.Services.MessageService;

namespace EjarTech
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        //constructor
        public Startup(IHostingEnvironment environment)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            this.Configuration = config.Build();
        }
        //Configuration DI
        public void ConfigureServices(IServiceCollection services)
        {
            //Set Company And Car Types
            services.Configure<CarsCatalog>(Configuration.GetSection("CarCatalog"));
            //Set Mobile auth service
            services.Configure<SmsOption>(Configuration.GetSection("MobileAuth"));
            //Set Mail Service
            services.Configure<MailServices>(Configuration.GetSection("MailSender"));
            //Supported Cities
            services.Configure<SupportedCities>(Configuration.GetSection("SupportedCities"));
            //Supported Language
            services.Configure<SupportedLanguage>(Configuration.GetSection("SupportedLanguage"));
            //Database Options
            services.Configure<DatabaseOptions>(Configuration.GetSection("Database"));
            //add sms service
            services.AddTransient<ISmsService, SmsService>();
            //add http context accessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //set database
            services.AddScoped<IDatabaseConnection, DatabaseConnection>();
            //User Provider
            services.AddScoped<IUserProvider, UserProvider>();
            //Translate manager
            services.AddScoped<ITranslationProvider, TranslationManager>();
            //add mvc
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
