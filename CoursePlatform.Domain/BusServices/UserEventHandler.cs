using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Events.Events;
using CoursePlatform.Events.Events.platformUser;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.BusServices
{




    public class UserEventHandler : ICapSubscribe
    {



        private readonly IUnitOfWork<CPWriteDbContext> _unitOfWork;
        private readonly IMapper _mapper;

        public UserEventHandler(IUnitOfWork<CPWriteDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [CapSubscribe(nameof(PlatformUserCreated))]
        public async Task Create(PlatformUserCreated @event)
        {
            await this._unitOfWork.GetRepositoryAsync<PlatformUserEntity>()
                 .AddAsync(new PlatformUserEntity
                 {
                     UserId = @event.Userid,
                     Name = @event.Name,
                     SchoolId = @event.SchoolId,
                     SectionId = @event.SectionId,
                     ID = @event.ID,
                     SchoolName = @event.SchoolName,
                     //ResearchGroupId = @event.ResearchGroup.Code,
                     //ResearchGroupName = @event.ResearchGroup.Name
                 });

            this._unitOfWork.SaveChanges();
        }


        [CapSubscribe(nameof(UserJoined))]
        public async Task Join(UserJoined @event)
        {
            await this._unitOfWork.GetRepositoryAsync<PlatformUserJoinedCourseEntity>()
                .AddAsync(new PlatformUserJoinedCourseEntity
                {
                    CourseId = @event.CourseId,
                    PlatUserId = @event.PlatUserId,
                    UserId = @event.UserId
                });

            await this._unitOfWork.GetRepositoryAsync<PlatformUserEntity>()
                .UpdateBach(u => u.UserId == @event.CreatorId, u => new PlatformUserEntity
                {
                    UpdateTime = DateTime.Now,
                    StdJoined = u.StdJoined + 1
                });


            this._unitOfWork.SaveChanges();
        }


        [CapSubscribe(nameof(UserCourseLeaved))]
        public async Task Leave(UserCourseLeaved @event)
        {
            await this._unitOfWork.GetRepositoryAsync<PlatformUserJoinedCourseEntity>()
                .DelAsync(p => p.UserId == @event.UserId && p.CourseId == @event.CourseId);

            await this._unitOfWork.GetRepositoryAsync<PlatformUserEntity>()
                        .UpdateBach(u => u.UserId == @event.CreatorId, u => new PlatformUserEntity
                        {
                            UpdateTime = DateTime.Now,
                            StdJoined = u.StdJoined - 1
                        });


            this._unitOfWork.SaveChanges();
        }



       



        [CapSubscribe(nameof(CourseListedChanged))]
        public async Task CourseListedChangedAction(CourseListedChanged @event)
        {

            await this._unitOfWork.GetRepositoryAsync<PlatformUserEntity>()
                .UpdateBach(u => u.UserId == @event.UserId, u => new PlatformUserEntity
                {
                     CourseShelves = @event.Count,
                     UpdateTime = DateTime.Now
                });

            this._unitOfWork.SaveChanges();

        }





        [CapSubscribe(nameof(PltUserCourseCountChanged))]
        public async Task PltUserCourseCreatedAction(PltUserCourseCountChanged @event)
        {
            await this._unitOfWork.GetRepositoryAsync<PlatformUserEntity>()
                .UpdateBach(u => u.UserId == @event.User, p => new PlatformUserEntity {
                    CourseCount = @event.Count 
                });

            this._unitOfWork.SaveChanges();
        }



    }
}
