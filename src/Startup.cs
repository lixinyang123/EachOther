using System;
using EachOther.Data;
using EachOther.Models;
using EachOther.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace EachOther
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
            services.Configure<CookieOptions>(config =>
            {
                config.SameSite = SameSiteMode.Lax;
            });

            services.AddControllersWithViews();

            services.AddDbContext<ArticleDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("EachOther"), new MySqlServerVersion(new Version(8, 0, 18)));
            });

            services.AddSingleton(new SecurityService("Key.txt", new Models.Secret()
            {
                IV = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16),
                Key = Guid.NewGuid().ToString().Replace("-", "")
            }));
            services.AddSingleton<NotifyService>();
            services.AddSingleton(new OssService(Configuration.GetSection("OssConfig").Get<OssConfig>()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ArticleDbContext articleDbContext)
        {
            if(!articleDbContext.Database.CanConnect())
                articleDbContext.Database.EnsureCreated();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
