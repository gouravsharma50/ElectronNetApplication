using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using DesktopApplication.Database;

namespace DesktopApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { logging.AddConsole(); })
                .UseElectron(args)
                .UseStartup<Startup>();
        }
    }
   
}


