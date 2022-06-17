using System;
using BugTracker.Controllers;
using BugTracker.Data;
using BugTracker.Data.Repositories;
using BugTracker.Extensions;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BugTracker
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            }, ServiceLifetime.Singleton);

            services.AddControllersWithViews();

            services.AddScoped<EfCoreRepository<Issue, ApplicationDbContext>, EfCoreIssueRepository>();
            services.AddScoped<EfCoreRepository<User, ApplicationDbContext>, EfCoreUserRepository>();
            services.AddScoped<EfCoreRepository<Status, ApplicationDbContext>, EfCoreStatusRepository>();
            services.AddScoped<EfCoreRepository<Priority, ApplicationDbContext>, EfCorePriorityRepository>();

            services.AddHttpClient<IVacancyService, VacancyService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["VacanciesApiUrl"]);
            }).AddRetryPolicy();

            services.AddLogging(loggingBuilder =>
            {
                // Output SQL queries
                loggingBuilder.AddConsole().AddFilter(DbLoggerCategory.Database.Name, LogLevel.Information);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Issue/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Issue}/{action=Index}/{id?}");
            });
            
            logger.LogInformation("1111111111");
        }
    }
}