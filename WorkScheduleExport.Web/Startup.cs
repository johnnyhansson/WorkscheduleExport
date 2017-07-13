using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkScheduleExport.Web.Infrastructure;
using WorkScheduleExport.Web.Infrastructure.Delivery;
using WorkScheduleExport.Web.Infrastructure.Export;

namespace WorkScheduleExport.Web
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder();

            if (env.IsDevelopment())
            {
                configurationBuilder.AddUserSecrets<EmailConfiguration>();
            }
            else if (env.IsProduction())
            {
                configurationBuilder.AddJsonFile("emailconfiguration.json");
            }

            configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IWorkScheduleExporter, ICalendarExporter>();
            services.AddScoped<IWorkScheduleReaderFactory, HtmlWorkScheduleReaderFactory>();
            services.AddScoped<IWorkScheduleDeliveryService, EmailDelivery>();
            services.AddSingleton(configuration.Get<EmailConfiguration>());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
