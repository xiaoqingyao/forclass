using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Data.Migrations;
using CoursePlatform.Events.Events;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.BusServices
{
    public class CourseEventHandler :ICapSubscribe
    {


        private readonly IUnitOfWork<CPWriteDbContext> _unitOfWork;
        private readonly IMapper _mapper;

        public CourseEventHandler(IUnitOfWork<CPWriteDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Create..........
        [CapSubscribe(nameof(CourseCreated))]
        public async Task Creator(CourseCreated @event)
        {

            var entity = this._mapper.Map<CourseEntity>(@event);



            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                .AddAsync(entity);


            if (@event.Tag != null)
            {

                await this.InsertTags(@event.Tag, @event.Id, @event.School.Code, @event.Region.Code, @event.Creator.Code, @event.SchoolName, @event.RegionName);

            }

            this._unitOfWork.SaveChanges();

        }
        #endregion



        #region  Update.....




        [CapSubscribe(nameof(CourseUpdated))]
        public async Task UpdatedAsync(CourseUpdated @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                .UpdateBach(c => c.ID == @event.Id, c => new CourseEntity
                {
                    Name = @event.Name,
                    CatalogId = @event.CatalogId,
                    CatalogName = @event.CatalogName,
                    CoverUrl = @event.CoverUrl,
                    SignatureId = @event.SignatureId,
                    SignatureName = @event.SignatureName,
                    Intro = @event.Intro,
                    Goal = @event.Goal,
                });

            this._unitOfWork.SaveChanges();
        }

        #endregion


        #region TagUpdated


        [CapSubscribe(nameof(TagUpdated))]
        public async Task TagUpdated(TagUpdated @event)
        {
            var repos = this._unitOfWork.GetRepositoryAsync<TagsEntity>();


            //删除原有的。
            await repos.DelAsync(t => t.CourseId == @event.CourseId);

            //

            await this.InsertTags(
                @event.Val, @event.CourseId, @event.SchoolId, @event.RegtionId, @event.CreatorId,@event.SchoolName,@event.RegionName);

            this._unitOfWork.SaveChanges();

        }


        #endregion


        #region Update joined number count 


        [CapSubscribe(nameof(CourseJoinedUpdate))]
        public async Task JoinedNumberUpdate(CourseJoinedUpdate @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                .UpdateBach(c => c.ID == @event.CourseId, c => new CourseEntity
                {
                    LeanerCount = @event.Number,
                    UpdateTime = DateTime.Now
                });
            this._unitOfWork.SaveChanges();
        }

        #endregion


        #region Update CollabratorCount...


        [CapSubscribe(nameof(CollabratorChanged))]
        public async Task ChangeCollabrator(CollabratorChanged @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                 .UpdateBach(c => c.ID == @event.CourseId, c => new CourseEntity
                 {
                    CollbratorCount = @event.CollabratorCount,
                    UpdateTime = DateTime.Now
                 });

            this._unitOfWork.SaveChanges();
        }


        #endregion


        #region Status Change..


        [CapSubscribe(nameof(CourseSchoolStatusChanged))]

        public async Task ChangeStatus(CourseSchoolStatusChanged @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                .UpdateBach(c => c.ID == @event.CouserId,
                    c => new CourseEntity
                    {
                        Status = @event.Status,
                        UpdateTime = DateTime.Now
                    });
            this.Save();
        }



        [CapSubscribe(nameof(CourseRegionStatusChanged))]

        public async Task ChangeRegtionStatus(CourseRegionStatusChanged @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                .UpdateBach(c => c.ID == @event.CourseId,
                    c => new CourseEntity
                    {
                        RegionStatus = @event.Status,
                        UpdateTime = DateTime.Now
                    });
            this.Save();
        }


        #endregion


        #region Delete


        [CapSubscribe(nameof(CourseDeleted))]
        public async Task Delete(CourseDeleted @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                .DelAsync(c => c.ID == @event.CourseId);

            //await this._unitOfWork.GetRepositoryAsync<PlatformUserJoinedCourseEntity>()
            //    .UpdateBach(p => p.CourseId == @event.CourseId, p => new PlatformUserJoinedCourseEntity
            //    {
            //        CreatorDelete = true,
            //        UpdateTime = DateTime.Now
            //    });


            this.Save();
        }


        #endregion



        #region 更新引用的学程...

        [CapSubscribe(nameof(CourseDsUpdated))]
        public async Task CourseDsUpdatedAction(CourseDsUpdated @event)
        {
            await this._unitOfWork.GetRepositoryAsync<QuoteDsEntity>()
                   .UpdateBach(q => q.DsId == @event.DsId && q.CatalogId == @event.CatalogId && q.CourseId == @event.CoureId, q => new QuoteDsEntity
                   {
                       //IsOpen = @event.IsOpen,
                       DsName = @event.Name,
                       Cover = @event.CoverUrl,
                       //IsShared = @event.IsShared
                   });

            this.Save();

        }
        #endregion


        #region 更新的引用的学程总数 




        [CapSubscribe(nameof(CourseDsCountChanged))]
        public async Task CourseDsCountChangedAction(CourseDsCountChanged @event)
        {
            await this._unitOfWork.GetRepositoryAsync<CourseEntity>()
                .UpdateBach(c => c.ID == @event.CourseId, c => new CourseEntity
                {
                    CourseCount = @event.Count
                });

            this._unitOfWork.SaveChanges();


        }



        #endregion



        #region private install tags...

        void Save()
        {
            this._unitOfWork.SaveChanges();
        }

        private async Task InsertTags(TagEventVal val, string courseId, int schoolId,int regionId, int creatorId, string schoolName, string regiongName)
        {

            var tag = new TagsEntity
            {
                AssetId = val.Id,
                Name = val.Name,
                CourseId = courseId,
                SchoolId = schoolId,
                RegtionId = regionId,
                Creator = creatorId,
                SchoolName = schoolName,
                RegionName = regiongName
            };


            var tags = new List<TagsEntity>
            {
                tag
            };

            foreach (var item in val.Items)
            {
                tags.Add(new TagsEntity
                {
                    AssetId = item.Id,
                    Name = item.Name,
                    TypeName = item.TypeName,
                    CourseId = courseId,
                    CategoryId = tag.AssetId,
                   SchoolId = schoolId,
                   RegtionId = regionId,
                   Creator  = creatorId,
                   SchoolName = schoolName,
                   RegionName = regiongName
                     
                });
            }


            await this._unitOfWork.GetRepositoryAsync<TagsEntity>()
                 .AddAsync(tags.ToArray());
        }


        #endregion



     

    }
}
