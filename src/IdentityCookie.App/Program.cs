using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityCookie.App.IocConfig;
using IdentityCookie.Services.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityCookie.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            webHost.Services.InitializeDb();
            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((hostingContext, logging) => 
                    {
                        logging.ClearProviders();
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddDebug();

                        if(hostingContext.HostingEnvironment.IsDevelopment()) 
                            logging.AddConsole(); // افزایش سرعت برنامه در ارائه نهایی
                        
                        logging.AddDbLogger(); // TODO: چک کن این چجوری کار میکنه
                    }).UseStartup<Startup>();
                });
    }
}
