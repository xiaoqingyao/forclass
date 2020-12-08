using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNetCore.CAP.Processor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using CoursePlatform.Application.Modules;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.IO;

namespace CoursePlatform.Test
{

    public class ContainerHelper
    {



        public static IHttpContextAccessor Context(string role = "teacher")
        {
            var hca = new Mock<IHttpContextAccessor>();

            var context = new DefaultHttpContext();

            string token = "D415A1DAE1A49669DBA5295085A654";
            if (role == "teacher")
            {
                token = "05986DDB458B8F10A31D59B613D";
            }
            if (role == "Region")
            {
                token = "B5662D871C40739E68D2CC08C7605";
            }


            context.Request.Headers.Add(CoursePlatformContext.HeaderToken, new Microsoft.Extensions.Primitives.StringValues(token));

            hca.Setup(_ => _.HttpContext).Returns(context);


            return hca.Object;
        }

        public static IContainer Builder(string role = "teacher")
        {

            var Services = new ServiceCollection();
            Services.AddLogging();
            //Services.AddTransient(s => new LoggerFactory().CreateLogger<Dispatcher>());
            //Services.AddTransient(s => new LoggerFactory().CreateLogger<DotNetCore.CAP.Internal.IMessageSender>());

            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.EnvironmentName).Returns("Development");
            env.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());
    




            IConfiguration cfg = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var startup = new CoursePlatform.Application.Startup(cfg, env.Object)
            {
                IocHttp = false
            };

            startup.ConfigureServices(Services);

            var builder = new ContainerBuilder();

            builder.Register(ctx => Context(role));

            startup.ConfigureContainer(builder);

            builder.Populate(Services);

            return builder.Build();


        }

    }

}