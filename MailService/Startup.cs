using MailService.Services.ConsumerNotificationService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace MailService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => new ConnectionFactory
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            });
            services.AddHostedService<NotificationConsumer>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync("The mail notification service has been launched...");
                    });
            });
        }
    }
}