using Autofac;
using CoursePlatform.Domain.Core;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.Core.PartnerAggregate;
using CoursePlatform.Domain.Core.PlatformUserAggregate;
using CoursePlatform.Domain.Core.ValueFactory;
using CoursePlatform.Domain.Permission;
using CoursePlatform.Domain.Queries;
using CoursePlatform.Domain.Queries.Catalog;
using CoursePlatform.Domain.Queries.Collabrator;
using CoursePlatform.Events;
using CoursePlatform.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoursePlatform.Domain
{
    public static class DomainExtentions
    {

        public static ContainerBuilder AddDomain(this ContainerBuilder builder, bool iocHttp, IConfiguration config)
        {

            var opt = config.GetSection(DomainOptions.OptionsSection).Get<DomainOptions>();

            builder.Register(ctx => Options.Create(opt)).SingleInstance();

            builder.AddEventBus(config);

            builder.AddIDTools(config);


            builder.RegisterType<IdSetter>().Keyed<IDomainSetter>(DomainSetter.ID).SingleInstance();
            builder.RegisterType<MapperSetter>().Keyed<IDomainSetter>(DomainSetter.Mapper).SingleInstance();

            builder.RegisterType<Toolbox>().As<IToolbox>().InstancePerLifetimeScope();
            builder.RegisterType<CourseServices>().As<ICourseServices>().InstancePerLifetimeScope();
            builder.RegisterType<PlatformUserServcie>().As<IPlatformUserService>().InstancePerLifetimeScope();

            builder.RegisterType<PartnerService>().As<IPartnerService>().InstancePerLifetimeScope();

            builder.RegisterType<PermissionServices>().As<IPermission>().InstancePerLifetimeScope();


            builder.RegisterType<FilterQuery>().As<IFilterQuery>().InstancePerLifetimeScope();
            builder.RegisterType<CourseQuery>().As<ICourseQuery>().InstancePerLifetimeScope();

            builder.RegisterType<CollabratorQuery>().As<ICollabratorQuery>().InstancePerLifetimeScope();
            builder.RegisterType<BindQuery>().As<IBindQuery>().InstancePerLifetimeScope();


            builder.RegisterType<ValueFactory>().As<IValueFactory>().SingleInstance();


            builder.RegisterType<CourseLoader>().As<ICourseLoader>().InstancePerLifetimeScope();

            return builder;
        }

    
    }
}
