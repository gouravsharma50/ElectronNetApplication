using DesktopApplication.Database;
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
                options.UseSqlite("Data Source=ElectronPoC.sqlite"));

            services.AddSingleton<DBConnections>();
            services.AddScoped<BusinessService>();

            // ✅ Enable Session
            services.AddDistributedMemoryCache();
            services.AddSession();
           
            // ✅ Register IHttpContextAccessor
            services.AddHttpContextAccessor();
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
