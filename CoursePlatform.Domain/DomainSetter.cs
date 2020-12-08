using Autofac;
using AutoMapper;
using CoursePlatform.Events;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Tools;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoursePlatform.Domain
{


    public enum DomainSetter
    {
        ID,
        Mapper
    }

    /// <summary>
    /// Id Generator
    /// </summary>
    public class IdSetter : IDomainSetter
    {

        private readonly IIDTools idTools;

        public IdSetter(IIDTools idTools)
        {
            this.idTools = idTools;
        }

        public void Set(AggregateRoot aggregate)
        {
            aggregate.ID = this.idTools.ID();
        }
    }

    /// <summary>
    /// Automapper
    /// </summary>
    public class MapperSetter : IDomainSetter
    {

        private readonly IMapper _mapper;

        public MapperSetter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Set(AggregateRoot aggregate)
        {
            aggregate.Mapper = this._mapper;
        }
    }




}
