using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.Events;
using CoursePlatform.Events.Events;
using CoursePlatform.infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using CoursePlatform.infrastructure.Validators;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.Domain.Commands;
using CoursePlatform.Domain.Queries.Share;
using CoursePlatform.Events.Events.Collabrator;
using CoursePlatform.infrastructure.Utility;
using System.Text.Json.Serialization;

namespace CoursePlatform.Domain.Core.CourseAggregate
{
    public class Course : AggregateRoot
    {



        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 署名ID  如果大于零为教研组，反之为自签
        /// </summary>
        public int SignatureId { get; set; }


        public string SignatureName { get; set; }


        public string CoverUrl { get; set; }


        public string CatalogId { get; set; }


        public string CatalogName { get; set; }

        public string Intro { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public string Goal { get; set; }

        public CourseStatus Status { get; set; }

        public CourseStatus RegionStatus { get; set; }


        public TagVal Tag { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public UserPropVal Region { get; set; }



        /// <summary>
        /// 学校
        /// </summary>
        public UserPropVal School { get; set; }



        /// <summary>
        /// 创建人
        /// </summary>
        public UserPropVal Creator { get; set; }




        /// <summary>
        /// 引用的学程
        /// </summary>

        public IList<QuoteDSVal> DsItems { get; set; }


        /// <summary>
        /// 协作者
        /// </summary>
        [JsonIgnore]
        public AsyncLazy<CollabratorVal> CollaboratorLib { get; set; }


        /// <summary>
        /// 加入学习的对象
        /// </summary>
        public LeanerVal Leaner { get; set; }


        public DateTime? CreationTime { get; set; }



        /// <summary>
        /// 年级
        /// </summary>
        public int GradeCode { get; set; }


        public bool IsJoined(int appuserId)
        {
            if (this.Leaner == null || appuserId == 0)
            {
                return false;
            }
            return this.Leaner.Has(appuserId);
        }




        private void Qoute(QuoteDSVal val)
        {
            if (this.DsItems == null)
            {
                this.DsItems = new List<QuoteDSVal>();
            }
            this.DsItems.Add(val);
        }


        private int Section()
        {
            if (this.CatalogId is null or {Length: <= 0 })
            {
                return 0;
            }

            var ary = this.CatalogId.Split(new char[] { '/' });

            if (ary.Length is 0)
            {
                return 0;
            }
            if (int.TryParse(ary[0],out int val))
            {
                return val;
            }
            return 0;

        }


        //public CourseEntity Base { get; set; }

        internal void Create(UserPropVal region, UserPropVal school, UserPropVal creator)
        {

            this.Region = region;
            this.School = school;
            this.Creator = creator;

            this.CreationTime = DateTime.Now;

            this.Status = CourseStatus.Draft;
            this.RegionStatus = CourseStatus.RegiogDefault;

            var @event = this.Mapper.Map<CourseCreated>(this);

            @event.Section = this.Section();

            this.AddEvent(@event);
        }

        internal void Update(CreateCourseDTO dto, int editorId)
        {


            if (this.Creator.Code != editorId)
            {
                throw new CPValidateExceptions($"无权限操作此课程数据{dto.Id}");
            }

            this.CatalogId = dto.CatalogId;
            this.CatalogName = dto.CatalogName;
            this.Name = dto.Name;
            this.SignatureId = dto.SignatureId;
            this.SignatureName = dto.SignatureName;
            this.CoverUrl = dto.CoverUrl;

            this.Intro = dto.Intro;
            //this.Goal = dto.Goal;

            var @event = new CourseUpdated(this.CatalogId,
                                           this.CatalogName,
                                           this.Name,
                                           this.SignatureId,
                                           this.SignatureName,
                                           this.CoverUrl,
                                           this.Intro,
                                           this.Goal,
                                           this.ID);


            this.AddEvent(@event);



            var tag = this.Mapper.Map<TagVal>(dto.Tag);

            if (this.Tag.Equals(tag))
            {
                return;
            }


            this.Tag = tag;

            var tagEvent = new TagUpdated
            {
                CourseId = this.ID,
                SchoolId = this.School.Code,
                RegtionId = this.Region.Code,
                CreatorId = this.Creator.Code,
                RegionName = this.Region.Name,
                SchoolName = this.School.Name
            };


            tagEvent.Val = this.Mapper.Map<TagEventVal>(this.Tag);

            this.AddEvent(tagEvent);


        }

        internal bool QuoteDS(QuoteDSDTO dto, int operatorId, string operatorName)
        {


            if (this.Creator.Code != operatorId && (this.CollaboratorLib != null && !this.CollaboratorLib.Value.Has(operatorId)))
            {
                throw new CPPermissionException($"无权限{this.Creator.Code} -- {operatorId}");
            }


            var sorteData = new List<SortedData>();
            var qouteData = new List<QuoteDsEventData>();

            int i = 0;
            foreach (var item in dto.Items)
            {

                i++;

                var val = this.DsItems?.FirstOrDefault(d => d.DsId == item.DsId && d.CatalogId == dto.CatalogId);

                if (val != null)
                {
                    if (val.SortVal == i)
                    {
                        continue;
                    }

                    val.SortVal = i;

                    sorteData.Add(new SortedData
                    {
                        SortVal = i,
                        CatalogId = dto.CatalogId,
                        CourseId = this.ID,
                        DsId = item.DsId
                    });

                    continue;
                }

                var dsVal = new QuoteDSVal
                {
                    IsOpen = item.IsOpen,
                    DsId = item.DsId,
                    CatalogId = dto.CatalogId,
                    OperatorId = operatorId,
                    DsName = item.DsName,
                    OperatorName = operatorName,
                    IsShared = item.IsShared,
                    IsOriginal = item.IsOriginal 
                };

                //加到最后
                dsVal.SortVal = this.DsItems == null ? 0 : this.DsItems.Count + 1;

                var eventVal = this.Mapper.Map<QuoteDsEventData>(dsVal);
                eventVal.CourseId = this.ID;

                this.Qoute(dsVal);

                qouteData.Add(eventVal);

            }

            if (sorteData.Count > 0)
            {
                this.AddEvent(new CourseDSSorted
                {
                    Items = sorteData
                });
            }

            if (qouteData.Count > 0)
            {
                this.AddEvent(new CourseDSQuoted
                {
                    Items = qouteData
                });
            }



            this.AddEvent(new CourseDsCountChanged(this.ID, this.DsItems.Count));

            return this.Events?.Count > 0;

        }

        internal bool SetDsStatus(int catalogId, Guid dsId, bool isOpen)
        {

            var item = this.DsItems?.FirstOrDefault(d => d.CatalogId == catalogId && d.DsId == dsId);

            if (item == null)
            {
                return false;
            }
            if (item.IsOpen == isOpen)
            {
                return false;
            }

            item.IsOpen = isOpen;

            this.AddEvent(new CourseDsStatusUpdated
            {
                DsId = dsId,
                CatalogId = catalogId,
                IsOpen = isOpen,
                CourseId = this.ID
            });

            return true;

        }

        internal bool DelDs(int catalogId, Guid dsId, int operatorId)
        {
            var item = this.DsItems?.FirstOrDefault(d => d.DsId == dsId && d.CatalogId == catalogId);

            Prosecutor.NotNull(item, "学程不存在");


            if (item.OperatorId != operatorId && this.Creator.Code != operatorId)
            {
                throw new CPPermissionException("无权限");
            }

            bool removed = this.DsItems.Remove(new QuoteDSVal
            {
                DsId = dsId,
                CatalogId = catalogId
            });

            if (!removed)
            {
                return false;
            }

            this.AddEvent(new CourseDsDeleted
            {
                CourseId = this.ID,
                CatalogId = catalogId,
                DsId = dsId
            });


            this.AddEvent(new CourseDsCountChanged(this.ID, this.DsItems.Count));

            return true;

        }


        private void CheckIsOwner(int operatorId)
        {
            if (this.Creator.Code != operatorId)
            {
                throw new CPPermissionException($"无权限{this.Creator.Code} - {operatorId}");
            }
        }


        #region 协作者....
        private IList<CollabratorEventItem> InsertCollabrator(CollabratorDTO dto, int operatorId)
        {
            this.CheckIsOwner(operatorId);


            this.CollaboratorLib = this.CollaboratorLib.Binder();
            if (this.CollaboratorLib.Value is null)
            {
                this.CollaboratorLib = new AsyncLazy<CollabratorVal>(new CollabratorVal());
            }

            IList<CollabratorEventItem> items = new List<CollabratorEventItem>();

            if (dto.CommunityObjs.NoData() is false)
            {

                foreach (var item in dto.CommunityObjs)
                {
                    if (this.CollaboratorLib.Value.Add(new CollabratorItem(item.ObjId)) is false)
                    {
                        continue;
                    }

                    CollabratorEventItem colltrator = this.Mapper.Map<CollabratorEventItem>(item);
                    colltrator.Type = (int)CollabratorType.Community;
                    items.Add(colltrator);
                }
            }

            if (dto.GradeObjs.NoData() is false)
            {

                foreach (var item in dto.GradeObjs)
                {
                    if (this.CollaboratorLib.Value.Add(new CollabratorItem(item.ObjId)) is false)
                    {
                        continue;
                    }

                    CollabratorEventItem colltrator = this.Mapper.Map<CollabratorEventItem>(item);
                    colltrator.Type = (int)CollabratorType.School;
                    items.Add(colltrator);
                }

            }
            return items;
        }

        internal bool AddCollaborator(CollabratorDTO dto, int operatorId)
        {

            var items = this.InsertCollabrator(dto, operatorId);

            if (items is { Count: <= 0 })
            {
                return false;
            }

            this.AddEvent(new CollaboratorAdded
            {
                CourseId = this.ID,
                Items = items
            });

            this.AddEvent(new CollabratorChanged
            {
                CollabratorCount = this.CollaboratorLib.Value.Count,
                CourseId = this.ID

            }); ;

            return true;

        }


        internal bool ReplaceCollaborator(CollabratorDTO dto, int operatorId)
        {

            this.CollaboratorLib?.Value.Clear();



            var items = this.InsertCollabrator(dto, operatorId);

            if (items is null or { Count: <= 0 })
            {
                this.AddEvent(new CollabratorAllDeleted(dto.CourseId));
            }
            else
            {
                this.AddEvent(new CollabratorReplaced
                {
                    CourseId = this.ID,
                    Items = items
                });
            }
            this.AddEvent(new CollabratorChanged
            {
                CollabratorCount = CollaboratorLib is null ? 0 : CollaboratorLib.Value.Count,
                CourseId = this.ID
            });

            return true;

        }
        #endregion


        #region 加入学习...
        internal bool LeanerLeave(int userId)
        {
            if (this.Leaner == null)
            {
                return false;
            }
            if (this.Leaner.Count is 0)
            {
                return false;
            }

            if (this.Leaner.Remove(userId) is false)
            {
                return false;
            }

            this.AddEvent(new CourseJoinedUpdate
            {
                CourseId = this.ID,
                Number = this.Leaner.Count
            });

            return true;

        }

        internal bool JoinLeaner(int userId, string pltUserId)
        {

            if (this.Status != CourseStatus.Accept
                &&this.Status != CourseStatus.Listed 
                && this.RegionStatus !=  CourseStatus.RegionAccept 
                && this.RegionStatus != CourseStatus.RegionListed)
            {
                throw new CPValidateExceptions("该课程状态异常");
            }

            this.Leaner = this.Leaner.Binder();


            bool rev = this.Leaner.Add(new(userId, pltUserId));

            //var rev = this.Leaner.Add(new LeanerItemVal
            //{
            //    PltUserId = pltUserId,
            //    UserId = userId
            //});
            if (rev is false)
            {
                throw new CPValidateExceptions("已经加入");
                //return false;
            }
            this.AddEvent(new CourseJoinedUpdate
            {
                CourseId = this.ID,
                Number = this.Leaner.Count
            });
            return true;

        }

        #endregion

        #region 学校审核...



        internal bool SchoolReview(int operatorId)
        {
            this.CheckIsOwner(operatorId);

            if (this.Status is not CourseStatus.Draft or CourseStatus.Reject)
            {
                throw new CPValidateExceptions($"课程状态异常{this.Status}");
            }
            this.Status = CourseStatus.Review;
            this.AddEvent(new CourseSchoolStatusChanged(this.ID, (int)this.Status));
            return true;
        }

        internal void SchoolPass(CourseAuditCommand cmd)
        {
            if (this.Status != CourseStatus.Review)
            {
                throw new CPValidateExceptions($"原状态异常{this.Status}");
            }
            this.Status = CourseStatus.Accept;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.Status;
            @event.StatusDesc = "校审核通过";
            this.AddEvent(@event);
            this.AddEvent(new CourseSchoolStatusChanged(this.ID, Status: (int)this.Status));
        }

        internal void SchoolReject(CourseAuditCommand cmd)
        {

            if (this.RegionStatus is not CourseStatus.RegiogDefault and not  CourseStatus.RegionReject)
            {
                throw new CPValidateExceptions($"区域状态{this.RegionStatus},禁止此操作");
            }

            //通过、提审、下架后可以拒绝
            if ((this.Status & (CourseStatus.Accept | CourseStatus.UnListed |  CourseStatus.Review)) == 0)
            {
                throw new CPValidateExceptions($"原状态异常{this.Status}");
            }
            this.Status = CourseStatus.Draft;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.Status;
            @event.StatusDesc = "校审核拒绝";
            this.AddEvent(@event);
            this.AddEvent(new CourseSchoolStatusChanged(this.ID, (int)this.Status));
        }

        internal void ListToSchool(CourseAuditCommand cmd)
        {
            if ((this.Status & (CourseStatus.Accept | CourseStatus.UnListed)) is 0)
            {
                throw new CPValidateExceptions($"原状态异常{this.Status}");
            }
            this.Status = CourseStatus.Listed;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.Status;
            @event.StatusDesc = "学校上架课程";
            this.AddEvent(@event);
            this.AddEvent(new CourseSchoolStatusChanged(this.ID, (int)this.Status));
        }

        internal void UnListToSchool(CourseAuditCommand cmd)
        {
            if (this.Status != CourseStatus.Listed)
            {
                throw new CPValidateExceptions($"原状态异常{this.Status}");
            }
            this.Status = CourseStatus.UnListed;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.Status;
            @event.StatusDesc = "学校下架课程";
            this.AddEvent(@event);
            this.AddEvent(new CourseSchoolStatusChanged(this.ID, (int)this.Status));
        }



        internal bool SchoolReviewCancel(int operatorId)
        {
            this.CheckIsOwner(operatorId);
            if (this.Status != CourseStatus.Review)
            {
                throw new CPValidateExceptions($"状态异常{this.Status}");
            }
            this.Status = CourseStatus.Draft;

            this.AddEvent(new CourseSchoolStatusChanged(this.ID, (int)this.Status));

            return true;
        }
        #endregion



        #region 区域审核....


        internal bool RegionReview(int operatorId)
        {
            if (this.Status != CourseStatus.Accept && this.Status != CourseStatus.UnListed && this.Status != CourseStatus.Listed)
            {
                throw new CPValidateExceptions("课程未经校审通过");
            }

            if (this.RegionStatus is not CourseStatus.RegiogDefault and not  CourseStatus.RegionReject)
            {
                throw new CPValidateExceptions($"课程状态异常{this.RegionStatus}");
            }
            this.RegionStatus = CourseStatus.RegionReview;
            this.AddEvent(new CourseRegionStatusChanged(this.ID, (int)this.RegionStatus));
            return true;
        }

        internal void RegionPass(CourseAuditCommand cmd)
        {
            if (this.RegionStatus != CourseStatus.RegionReview)
            {
                throw new CPValidateExceptions($"原状态异常{this.RegionStatus}");
            }
            this.RegionStatus = CourseStatus.RegionAccept;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.RegionStatus; @event.StatusDesc = "区域审核通过";
            this.AddEvent(@event);
            this.AddEvent(new CourseRegionStatusChanged(this.ID, (int)this.RegionStatus));
        }

        internal void RegionReject(CourseAuditCommand cmd)
        {
            if (this.RegionStatus != CourseStatus.RegionAccept && this.RegionStatus != CourseStatus.RegionUnlisted)
            {
                throw new CPValidateExceptions($"原状态异常{this.RegionStatus}");
            }
            this.RegionStatus = CourseStatus.RegionReject;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.RegionStatus;
            @event.StatusDesc = "区域审核拒绝";
            this.AddEvent(@event);
            this.AddEvent(new CourseRegionStatusChanged(this.ID, (int)this.RegionStatus));
        }

        internal void ListToRegion(CourseAuditCommand cmd)
        {
            if ((this.RegionStatus & (CourseStatus.RegionAccept | CourseStatus.RegionUnlisted)) == 0) 
            {
                throw new CPValidateExceptions($"原状态异常{this.RegionStatus}");
            }
            this.RegionStatus = CourseStatus.RegionListed;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.RegionStatus;
            @event.StatusDesc = "区域上架";
            this.AddEvent(@event);
            this.AddEvent(new CourseRegionStatusChanged(this.ID, (int)this.RegionStatus));
        }

        internal bool UpdateDs(UpdateDsDTO dto, int userId)
        {

            //if (userId != this.Creator.Code && )
            //{
            //    throw new CPPermissionException();
            //}

            if (this.DsItems.NoData())
            {
                return false;
            }
            var item = this.DsItems.FirstOrDefault(d => d.CatalogId == dto.CatalogId && d.DsId == dto.Item.DsId);

            if (item is null)
            {
                return false;
            }

            if (userId != item.OperatorId)
            {
                throw new CPPermissionException();
            }

            item.Cover = dto.Item.Cover;
            item.DsName = dto.Item.DsName;
            item.IsOpen = dto.Item.IsOpen;
            item.IsShared = dto.Item.IsShared;

            this.AddEvent(new CourseDsUpdated(this.ID,dto.CatalogId, dto.Item.DsId, dto.Item.IsOpen, item.IsShared, item.DsName, item.Cover));

            return true;

        }

        internal bool RegionReviewCancel(int operatorId)
        {
            //this.CheckIsOwner(operatorId);
            if (this.RegionStatus != CourseStatus.RegionReview)
            {
                throw new CPValidateExceptions($"状态异常{this.RegionStatus}");
            }
            this.RegionStatus = CourseStatus.RegiogDefault; //null; //CourseStatus.;

            this.AddEvent(new CourseRegionStatusChanged(this.ID, (int)this.RegionStatus));

            return true;
        }


        internal void UnListToRegion(CourseAuditCommand cmd)
        {
            if (this.RegionStatus != CourseStatus.RegionListed)
            {
                throw new CPValidateExceptions($"原状态异常{this.RegionStatus}");
            }
            this.RegionStatus = CourseStatus.RegionUnlisted;

            var @event = this.Mapper.Map<CourseBeAudited>(cmd);
            @event.Status = (int)this.RegionStatus;
            @event.StatusDesc = "区域下架课程";
            this.AddEvent(@event);
            this.AddEvent(new CourseRegionStatusChanged(this.ID, (int)this.RegionStatus));
        }


        #endregion




        internal void Delete(int operaoterId)
        {
            if ((this.Status & (CourseStatus.Draft | CourseStatus.Reject | CourseStatus.Review)) ==0)
            {
                throw new CPValidateExceptions($"当前课程状态不能允许删除操作:{this.Status}");
            }

            this.AddEvent(new CourseDeleted(this.ID, operaoterId));
        }



       




    }
}
