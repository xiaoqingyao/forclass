using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Events.Events;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.BusServices
{
    public class CourseAuditHandler : ICapSubscribe
    {

        private readonly IUnitOfWork<CPDbContext> _unitOfWork;
        private readonly IMapper _mapper;

        public CourseAuditHandler(IUnitOfWork<CPDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [CapSubscribe(nameof(CourseBeAudited))]
        public async Task AuditLog(CourseBeAudited @event)
        {
            var entity = this._mapper.Map<CourseReviewLogEntity>(@event);

            await this._unitOfWork.GetRepositoryAsync<CourseReviewLogEntity>()
                    .AddAsync(entity);

            this._unitOfWork.SaveChanges();
        }

    }
}
