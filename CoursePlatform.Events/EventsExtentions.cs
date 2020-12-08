using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events
{
    public static class EventsExtentions
    {
        public static ContainerBuilder AddEventBus(this ContainerBuilder builder, IConfiguration config)
        {


            builder.RegisterType<EventSender>().As<IEventSender>().InstancePerLifetimeScope();

            return builder;
        }
    }
}
