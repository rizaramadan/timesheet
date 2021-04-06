using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.Domains.Data;
using Timesheet.Models;
using Timesheet.Services;

namespace Timesheet
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<AppUser, AppRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
                config.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddDefaultUI()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddRazorPages();
            services.AddControllersWithViews();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = Configuration["Auth:Google:ClientId"];
                    options.ClientSecret = Configuration["Auth:Google:ClientSecret"];
                    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                });

            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddTransient<IEmailSender, MailKitEmailSender>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.HostAddress = Configuration["MailKit:SMTP:Address"];
                options.HostPort = Convert.ToInt32(Configuration["MailKit:SMTP:Port"]);
                options.HostUsername = Configuration["MailKit:SMTP:Account"];
                options.HostPassword = Configuration["MailKit:SMTP:Password"];
                options.SenderEmail = Configuration["MailKit:SMTP:SenderEmail"];
                options.SenderName = Configuration["MailKit:SMTP:SenderName"];
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                //options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add($"/Domains/Shared/{{0}}{RazorViewEngine.ViewExtension}");
                options.ViewLocationFormats.Add($"/Domains/{{1}}/{{0}}{RazorViewEngine.ViewExtension}");
                options.PageViewLocationFormats.Add($"/Domains/Shared/{{0}}{RazorViewEngine.ViewExtension}");
                options.AreaPageViewLocationFormats.Add($"/Domains/Shared/{{0}}{RazorViewEngine.ViewExtension}");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
