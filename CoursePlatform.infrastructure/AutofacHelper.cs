using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.infrastructure
{
    public static class AutofacHelper
    {
       public static ContainerBuilder Reg<TImplementer, T>(this ContainerBuilder builder, bool isHttp = true)
        {

            var reg = builder.RegisterType<TImplementer>().As<T>();

            if (isHttp)
            {
                reg.InstancePerRequest();
            }
            else
            {
                reg.InstancePerLifetimeScope();
            }

                
            return builder;
        }
    }
}
