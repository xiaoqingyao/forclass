using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.BusServices
{
    public abstract class EventHandlerBase
    {
        protected readonly IUnitOfWork<CPWriteDbContext> _unitOfWork;
        protected readonly IMapper _mapper;

        protected EventHandlerBase(IUnitOfWork<CPWriteDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
