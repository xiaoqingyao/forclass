using Autofac;
using AutoMapper;
using CoursePlatform.Application.Controllers;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.Core.PartnerAggregate;
using CoursePlatform.Domain.Core.PlatformUserAggregate;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.Domain.Permission;
using CoursePlatform.Domain.Queries.Collabrator;
using CoursePlatform.Domain.VO;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Tools;
using Microsoft.AspNetCore.Mvc.Localization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoursePlatform.Test
{

    [TestFixture]
    public class CourseTest
    {



        [Test]
        public async Task CreateTest()
        {
            var rev = await this.Create();
            Assert.IsNotNull(rev);
            var vo = await this.Get(rev);

            Assert.IsNotNull(vo);

            Assert.IsNotNull(vo.PersonalOperation);
            
        }


        [Test]
        public async Task UpdateTest()
        {
            var id = await this.Create();

            var rev = await this.Update(id);

            Assert.AreEqual(id, rev.Data());
        }

        [Test]
        public async Task QouteDsTest()
        {
            string cid = await this.Create();

            var rev = await Ctrl().QuoteDS(new QuoteDSDTO
            {
                Items = new List<QuoteDSDTOItem> {
                    new QuoteDSDTOItem
                    {
                        DsId = Guid.NewGuid(),
                        DsName = "我是学程1"
                    },
                    new QuoteDSDTOItem
                    {
                        DsId = Guid.NewGuid(),
                        DsName = "我是学程2"
                    }
                },
                CatalogId = 12,
                CourseId = cid
            });

            Assert.IsTrue(rev.Data());
        }

        [Test]
        public async Task OpenDsTest()
        {
            var cid = await this.Create();
            var dsId = Guid.NewGuid();
            await this.QuoteDesgin(cid, dsId, 12);

            var ret = await Ctrl().SetDsStatus(new DsStatusDTO
            {
                IsOpen = true,
                CatalogId = 12,
                CourseId = cid,
                DsId = dsId
            });

            Assert.IsTrue(ret.Data());

            var course = await this.Get(cid);

            //var dsItem = course.DsItems?.FirstOrDefault(d => d.DsId == dsId);

            //if (dsItem == null)
            //{
            //    Assert.Fail("dsitem is null");
            //    return;
            //}

            //Assert.IsTrue(dsItem.IsOpen);
                
        }




        [Test]
        public async Task DelDsTest()
        {
            var cid = await this.Create();
            var dsId = Guid.NewGuid();
            await this.QuoteDesgin(cid, dsId, 12);

            var ret = await Ctrl().DelDs(new DsDelDTO 
            {
                CatalogId = 12,
                CourseId = cid,
                DsId = dsId
            });

            Assert.IsTrue(ret.Data());

            var course = await this.Get(cid);

            //var dsItem = course.DsItems?.FirstOrDefault(d => d.DsId == dsId);

            //Assert.IsNull(dsItem);


        }


         public  CollabratorController cbtCtrl()
        {
            var c = ContainerHelper.Builder();
            return new CollabratorController(
                c.Resolve<IAppUser>(),
                c.Resolve<IMapper>(),
                c.Resolve<ICourseServices>(),
                c.Resolve<ICollabratorQuery>());
        }


        [Test]
        public async Task Collabrator()
        {
            var cid = await this.Create();


           var rev = await cbtCtrl().AddCollabrator(new CollabratorDTO
            {
                CourseId = cid,
                GradeObjs= new List<CollbratorObjDTO>
                {
                    new CollbratorObjDTO
                    {
                        ObjId = 1,
                        ObjName = "小张",
                        OrgId = 2,
                        RootId = 0,
                        OrgName = "学校名",
                        RootName = "区域名"
                    }
                },
                CommunityObjs = new List<CollbratorObjDTO>
                {
                    new CollbratorObjDTO
                    {
                        ObjId = 2,
                        ObjName = "小王",
                        RootName = "教研组",
                        RootId = 1
                    }
                }
            });


            Assert.IsTrue(rev.Data());

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var vo = (await cbtCtrl().Get(new IdDto(cid))).Data();


            Assert.Greater(vo.CommunityObj.Count, 0);
            Assert.Greater(vo.SchoolObj.Count, 0);


            //var vo = await this.Get(cid);

            //Assert.AreEqual(1, vo.CollabratorId.First());

            //await this.Ctrl().ReplaceCollabrator(new CollabratorDTO
            //{
            //    CollabratorId = new int[] { 2 },
            //    CourseId = cid
            //});


            //vo = await this.Get(cid);

            //Assert.AreEqual(2, vo.CollabratorId.First());
        }



        [Test]
        public async Task SchoolReivewTest()
        {
            var ctrl = Ctrl();

            string cid = await this.Create();

            var rev = await ctrl.SchoolReview(new IdDto(cid));

            Assert.IsTrue(rev.Data());

            var vo = await this.Get(cid);

            Assert.AreEqual((int)CourseStatus.Review, vo.Status);

            await ctrl.SchoolReviewCancel(new IdDto(cid));

            vo = await this.Get(cid);


            Assert.AreEqual((int)CourseStatus.Draft, vo.Status);

           
            




        }


        [Test]
        public async Task SchoolPass()
        {
            var ctrl = Ctrl();

            string cid = await this.Create();

            var rev = await ctrl.SchoolReview(new IdDto(cid));

            Assert.IsTrue(rev.Data());

            var vo = await this.Get(cid);

            Assert.AreEqual((int)CourseStatus.Review, vo.Status);

            var adminCtrl = Ctrl("admin");

            await adminCtrl.SchoolPass(new AuditDTO(cid, "被审核通过了"));

            vo = await this.Get(cid);


            Assert.AreEqual(vo.Status, (int)CourseStatus.Accept);


        }



        [Test]
        public async Task SchoolReject()
        {
            var ctrl = Ctrl();

            string cid = await this.Create();

            var rev = await ctrl.SchoolReview(new IdDto(cid));

            Assert.IsTrue(rev.Data());

            var vo = await this.Get(cid);

            Assert.AreEqual((int)CourseStatus.Review, vo.Status);

            var adminCtrl = Ctrl("admin");

            await adminCtrl.SchoolPass(new AuditDTO(cid, "被审核通过了"));

            vo = await this.Get(cid);


            Assert.AreEqual(vo.Status, (int)CourseStatus.Accept);

            await adminCtrl.SchoolReject(new AuditDTO(cid, "被审核拒绝了"));

            vo = await this.Get(cid);


            Assert.AreEqual(vo.Status, (int)CourseStatus.Reject);


        }



        [Test]
       public async Task SchoolListed()
        {
            var cid = await this.CoursePassed();

            var ctrl = Ctrl("admin");

            await ctrl.ListToSchool(new AuditDTO(cid, "学校上架了"));

            var vo = await this.Get(cid);

            Assert.AreEqual((int)CourseStatus.Listed, vo.Status);

        }



        [Test]
       public async Task SchoolListed_Then_Remove()
        {
            var cid = await this.CoursePassed();

            var ctrl = Ctrl("admin");

            await ctrl.ListToSchool(new AuditDTO(cid, "学校上架了"));

            var vo = await this.Get(cid);

            Assert.AreEqual((int)CourseStatus.Listed, vo.Status);

            await ctrl.ListToSchoolRemove(new AuditDTO(cid, "学校下架了。"));

            vo = await this.Get(cid);

            Assert.AreEqual(vo.Status, (int)CourseStatus.Accept);

        }



        [Test]
        public async Task<string> Course_School_Submit_Review()
        {
            var cid = await this.CoursePassed();

            await Ctrl("School").RegionReview(new IdDto(cid));

            var vo = await  this.Get(cid);

            Assert.AreEqual(vo.RegionStatus, (int)CourseStatus.RegionReview);

            return cid;

        }



       
        [Test]
        public async Task<string> Course_Region_Pass()
        {
            string cid = await Course_School_Submit_Review();

            await Ctrl("Region").RegionPass(new AuditDTO(cid, "区域审核通过了"));

            var ov = await this.Get(cid);

            Assert.AreEqual(ov.RegionStatus, (int)CourseStatus.RegionAccept);

            return cid;
        }



        [Test]
        public async Task Course_Reigon_Pass_School_Not_Reject()
        {
            var cid = await this.Course_Region_Pass();
            try
            {
                await Ctrl("Admin").SchoolReject(new AuditDTO(cid, "学校拒绝了。"));

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is CPValidateExceptions);
                return;
            }
            Assert.Fail();
        }




        /*****************The following is internal method.***********************/



        public async Task<string> CoursePassed()
        {
            var ctrl = Ctrl();

            string cid = await this.Create();

            var rev = await ctrl.SchoolReview(new IdDto(cid));

            Assert.IsTrue(rev.Data());

            var vo = await this.Get(cid);

            Assert.AreEqual((int)CourseStatus.Review, vo.Status);

            var adminCtrl = Ctrl("admin");

            await adminCtrl.SchoolPass(new AuditDTO(cid, "被审核通过了"));

            vo = await this.Get(cid);


            Assert.AreEqual(vo.Status, (int)CourseStatus.Accept);

            return cid;
        }


        public async Task<CourseVO> Get(string id)
        {
            var rev = await Ctrl().Get(new Domain.DTOS.Queries.GetParam
            {
                Id = id
            });

            if (rev.Result == null)
            {
                return null;
            }
            return rev?.Data();
        }


        public async Task<string> Create()
        {


            var rev = await Ctrl().Create(createDto());

            return rev.Data();

        }


        [Test]

        public async Task DeleteTest()
        {
            var cid = await this.Create();

            await Ctrl().Delete(new IdDto(cid));

            var vo = await this.Get(cid);

            Assert.IsNull(vo);
        }


        public  Task<ReturnVal<string>> Update(string id)
        {
            return Ctrl().Create(this.createDto(id));
            
        }




      




        private static CourseController Ctrl(string role = "teacher")
        {
            var c = ContainerHelper.Builder(role);
            var svc = c.Resolve<ICourseServices>();
            var usr = c.Resolve<IAppUser>();
            var platUsr = c.Resolve<IPlatformUserService>();
            var mapper = c.Resolve<IMapper>();
            var partnerSv = c.Resolve<IPartnerService>();

            var ctrl = new CourseController(svc, platUsr, mapper, usr, partnerSv
                                            , c.Resolve<IPermission>()
                                            , c.Resolve<IIDTools>());

            return ctrl;
        }


        private CreateCourseDTO createDto(string id = null)
        {

            string stuffix = String.IsNullOrEmpty(id) ? "" : DateTime.Now.ToString("HH:mm:ss:ff");

            return new CreateCourseDTO
            {
                Name = "My course" + stuffix,
                Intro = "Intro" + stuffix,
                CatalogId = "/12/32/233/" + stuffix + "/",
                CatalogName = "/目录/目录1/目录2/" + stuffix + "/",
                CoverUrl = "https://automapper.org/images/black_logo.png",
                SignatureId = 1,
                SignatureName = "我是教研组" + stuffix,
                //Goal = "我是目标" + stuffix,
                Tag = new Tag
                {
                    Id = 1,
                    Name = "国家课程" + stuffix,
                    Items = new List<TagDTOItem>
                    {
                        new TagDTOItem
                        {
                            Name = "语文" + stuffix,
                            TypeName = "科目" + stuffix,
                            Id = 1
                        }
                    }
                },
                Id = id
            };
        }

        private Task<ReturnVal<bool>> QuoteDesgin(string courseId, Guid guid, int catalogId)
        {
            var rev = Ctrl().QuoteDS(new QuoteDSDTO
            {
                Items = new List<QuoteDSDTOItem> {
                    new QuoteDSDTOItem
                    {
                        DsId =  guid,//Guid.NewGuid(),
                        DsName = "我是学程1"
                    },
                    new QuoteDSDTOItem
                    {
                        DsId = Guid.NewGuid(),
                        DsName = "我是学程2"
                    }
                },
                CatalogId = catalogId,
                CourseId = courseId
            });

            return rev;

        }

    }
}
