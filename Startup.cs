﻿using DesktopApplication.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.EntityFrameworkCore;
using DesktopApplication.Service;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DesktopApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

         
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<DBConnections>();
            services.AddScoped<BusinessService>();

            // ✅ Enable Session
            services.AddDistributedMemoryCache();
            services.AddSession();
           
            // ✅ Register IHttpContextAccessor
            services.AddHttpContextAccessor();
            // ✅ Configure Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login";  // Redirect to Home Controller's Login action
                    options.AccessDeniedPath = "/Home/AccessDenied"; // Redirect unauthorized users
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);// Keep user logged in for 7 days
                });
       
            // ✅ Configure Authorization Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            // ✅ Use Session Middleware
            app.UseSession();
            // ✅ Enable Authentication & Authorization Middleware
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            InitializeDatabase(serviceProvider);

            if (HybridSupport.IsElectronActive)
            {
                ElectronBootstrap();
            }
        }

        private void InitializeDatabase(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
        }

        public async void ElectronBootstrap()
        {
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1152,
                Height = 940,
                Show = false
            });

            await browserWindow.WebContents.Session.ClearCacheAsync();
            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle(Configuration["DemoTitleInSettings"]);
        }
    }
}
