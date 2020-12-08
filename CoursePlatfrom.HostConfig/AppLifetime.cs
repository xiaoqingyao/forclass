using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace CoursePlatfrom.HostConfig
{
    public static class AppLifetime
    {
            
        public static void AppStarted(this IApplicationBuilder app, IHostApplicationLifetime appLifetime)
        {

            var db = app.ApplicationServices.GetRequiredService<IUnitOfWork<CPDbContext>>(); 

            //app.ServerFeatures.

        }
        



    }
}
