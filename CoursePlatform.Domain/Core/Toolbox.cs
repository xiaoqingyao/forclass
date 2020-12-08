using Autofac.Features.Indexed;
using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Events;
using CoursePlatform.Infrastructure.Caching;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.Core
{

    public interface IToolbox
    {
        IOptions<DomainOptions> Options { get; set; }
        ICachingProvider Cachor { get; set; }
        IUnitOfWork<CPDbContext> Reader { get; set; }
        IIndex<DomainSetter, IDomainSetter> DomainSetter { get; set; }
        IEventSender Sender { get; set; }
        IMapper Mapper { get; set; }

        

    }


    public class Toolbox : IToolbox
    {
        public Toolbox(IOptions<DomainOptions> options, ICachingProvider cachor, IUnitOfWork<CPDbContext> reader, IIndex<DomainSetter, IDomainSetter> domainSetter, IMapper mapper, IEventSender sender)
        {
            Options = options;
            Cachor = cachor;
            Reader = reader;
            DomainSetter = domainSetter;
            Mapper = mapper;
            Sender = sender;
        }

        public IOptions<DomainOptions> Options { get; set; }
        public ICachingProvider Cachor { get; set; }
        public IUnitOfWork<CPDbContext> Reader { get; set; }
        public IIndex<DomainSetter, IDomainSetter> DomainSetter { get; set; }
        public IMapper Mapper { get; set; }
        public IEventSender Sender { get; set; }
    }
}
