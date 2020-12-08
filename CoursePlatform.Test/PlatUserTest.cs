using Autofac;
using AutoMapper;
using CoursePlatform.Application.Controllers;
using CoursePlatform.Application.Modules;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.Core.PlatformUserAggregate;
using CoursePlatform.infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Test
{
    [TestFixture]
    public class PlatUserTest
    {


        
        [Test]
        public async Task JoinTest()
        {
            var c = ContainerHelper.Builder();

            var hub = c.Resolve<IUserHub>();
            var platUsr = c.Resolve<IPlatformUserService>();
            var courseSvc = c.Resolve<ICourseServices>();
            var appUsr = c.Resolve<IAppUser>();
            var mapper = c.Resolve<IMapper>();

            var ctrl = new PlatforUserController(platUsr, hub, courseSvc,appUsr, mapper);

            string cid = await new CourseTest().Create();

            var rev = await ctrl.Join(new Domain.DTOS.UserJoinDTO
            {
                CourseId = cid //"9093749692826648576"
            });

            Assert.IsTrue(rev.Data());

            rev = await ctrl.Leavel(new Domain.DTOS.IdDto(cid));

            Assert.IsTrue(rev.Data());

        }


    }
}
