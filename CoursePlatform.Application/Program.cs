using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace CoursePlatform.Application
{
    public class Program
    {



        public static void Main(string[] args)
        {

            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                webBuilder.UseStartup<Startup>();


                //webBuilder.ConfigureAppConfiguration((wb, cb) =>{
                //    cb.
                //});
                    
                    })
            .ConfigureLogging((hostcontext,logging) =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
                string cfgFile = hostcontext.HostingEnvironment.IsDevelopment() ? "nlog.Development.config" : "nlog.config";
                logging.AddNLog(cfgFile);

            })
            .UseNLog();
    }
}
