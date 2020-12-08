using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Events.Events.Partner;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.BusServices
{
    public class PartnerEventHandler : ICapSubscribe
    {
        private readonly IUnitOfWork<CPDbContext> _unitOfWork;

        public PartnerEventHandler(IUnitOfWork<CPDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [CapSubscribe(nameof(PartnerCreated))]
        public async Task Create(PartnerCreated @event)
        {
            await this._unitOfWork.GetRepositoryAsync<PartnerEntity>()
                .AddAsync(new PartnerEntity
                {
                   ParentId = @event.ParentId,
                   ResourceId = @event.ResourceId,
                   Name = @event.Name,
                   Type = @event.Type,
                   ID = @event.Id
                });

            this._unitOfWork.SaveChanges();
        }


        [CapSubscribe(nameof(PartnerSetCourseListed))]
        public async Task SetCourseList(PartnerSetCourseListed @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseListedEntity>()
                .AddAsync(new CourseListedEntity
                {
                      CourseId = @event.CourseId,
                      OrgId = @event.OrgId,
                      OrgName = @event.OrgName,
                      PltId = @event.PltId,
                      Type = @event.Type,
                      ParentId = @event.ParentId,
                      Creator = @event.Creator
                });

            await this._unitOfWork.GetRepositoryAsync<PartnerEntity>()
                .UpdateBach(p => p.ID == @event.PltId, p => new PartnerEntity
                {
                    CourseCount = @event.Count,
                    UpdateTime = DateTime.Now
                });

            this._unitOfWork.SaveChanges();

        }



        [CapSubscribe(nameof(PartnerCourseListRemoved))]
        public async Task CourseListRemove(PartnerCourseListRemoved @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseListedEntity>()
                .DelAsync(c => c.CourseId == @event.CourseId && c.OrgId == @event.ObjId && c.Type == @event.ObjType);

            await this._unitOfWork.GetRepositoryAsync<PartnerEntity>()
                .UpdateBach(p => p.Type == @event.ObjType && p.ResourceId == @event.ObjId, p => new PartnerEntity
                {
                    CourseCount = @event.Count,
                    UpdateTime = DateTime.Now
                });

            this._unitOfWork.SaveChanges(); 
        }




    }

}
