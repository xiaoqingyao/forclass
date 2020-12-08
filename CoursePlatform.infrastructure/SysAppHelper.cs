using Autofac;
using CoursePlatform.infrastructure.Tools;
using Flakey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoursePlatform.infrastructure
{
    public static class SysAppHelper
    {
        public static IConfigurationBuilder AddJF(this IConfigurationBuilder builder, IHostEnvironment env, params string[] fileName)
        {

            foreach (var item in fileName)
            {

                builder.AddJsonFile($"{item}.json");
                builder.AddJsonFile($"{item}.{env.EnvironmentName}.json");
            }
            return builder;

        }


        public static void AddIDTools(this ContainerBuilder builder, IConfiguration config)
        {

           var section = config.GetSection(FlakyId.ConfigSectionName);

            builder.Register((ctx) => {
                return new FlakyId(section.GetValue<int>(FlakyId.MathinIdName), section.GetValue<DateTime>(FlakyId.EpochTime));
            }).As<IIDTools>().SingleInstance();


        }
    }
}
