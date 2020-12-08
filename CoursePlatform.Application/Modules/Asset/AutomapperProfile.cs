using AutoMapper;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain;
using CoursePlatform.Domain.Commands;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.Core.PartnerAggregate;
using CoursePlatform.Domain.Core.PlatformUserAggregate;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.Domain.Permission;
using CoursePlatform.Domain.VO;
using CoursePlatform.Events.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Modules.Asset
{
    public class AutomapperProfile : Profile
    {

        public AutomapperProfile()
        {


            /**********course***************/

            CreateMap<CourseEntity, Course>().ReverseMap();
            CreateMap<CourseEntity, CoursePagingVO>()
                .ForMember(c => c.LearnerCount, opt => opt.MapFrom(src => src.LeanerCount)); 
                //.ReverseMap();
            CreateMap<CourseCreated, CourseEntity>(); 
            CreateMap<Course, CourseCreated>();
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<Course, CourseVO>()
                .ForMember(c => c.Status, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<QuoteDSVal, QuoteDsEventData>();
            CreateMap<QuoteDSVal, DsItemVO>();
            CreateMap<QuoteDSDTOItem, QuoteDSVal>();

            CreateMap<QuoteDsEventData, QuoteDsEntity>();




            
            /*******Tag*******/

            CreateMap<Tag, TagVal>();
            CreateMap<Domain.DTOS.TagDTOItem, Domain.Core.CourseAggregate.TagItem>();
            CreateMap<TagVal, TagEventVal>();
            CreateMap<TagItem, TagEventItem>();

            CreateMap<TagVal, TagVO>();
            CreateMap<TagItem, TagVOItem>();


            /***********AppUser*******************/

            CreateMap<PlatformUser, PlatformUserEntity>().ReverseMap();


            /****************command -> event *********************/

            CreateMap<CourseAuditCommand, CourseBeAudited>();



            CreateMap<CourseBeAudited, CourseReviewLogEntity>();


            /*************************************************/


            CreateMap<Partner, PartnerEntity>().ReverseMap();


            /*****************Collabrator***********************/
            CreateMap<CollabratorSchoolDTO, CollabratorEventItem>();
            CreateMap<CollbratorObjDTO, CollabratorEventItem>();
            CreateMap<CollabratorEventItem, CollabratorEntity>();


            /*******************Operator***********************/
            CreateMap<CourseOperationItem, OperationVO>();
        }

    }
}
