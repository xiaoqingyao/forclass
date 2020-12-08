using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace CoursePlatform.Domain.BusServices
{
   public static class EventHandlerExtentions
    {
        public static void AddEventHandler(this IServiceCollection sc)
        {
            sc.AddTransient<UserEventHandler>();
            sc.AddTransient<CourseEventHandler>();
            sc.AddTransient<QuoteDsEventHandler>();
            sc.AddTransient<CollabratorEventHandler>();
            sc.AddTransient<CourseAuditHandler>();
            sc.AddTransient<PartnerEventHandler>();
        }
    }
}
