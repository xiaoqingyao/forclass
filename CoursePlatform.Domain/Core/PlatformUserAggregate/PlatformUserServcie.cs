using Autofac.Features.Indexed;
using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Events;
using CoursePlatform.Infrastructure.Caching;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoursePlatform.infrastructure.Validators;
using AutoMapper.Configuration.Conventions;
using CoursePlatform.infrastructure.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Domain.Core.PlatformUserAggregate
{


    public interface IPlatformUserService
    {
        Task<bool> CreateCourse(PltUserParam param, string courseId);
        Task<bool> DelCourse(int userId, string courseId);
        Task<PlatformUser> GetOrCreate(PltUserParam param);
        Task<bool> JoinCourse(int userId, string courseId, int creatorId);
        Task<bool> LeaveCourse(int userId, string courseId, int creatorId);
        Task<bool> ListCourseRemove(int userId, string courseId);
        Task<bool> ListedCourse(int userId, string courseId);
    }


    public class PlatformUserServcie : IPlatformUserService
    {



        private readonly IToolbox toolbox;

        public PlatformUserServcie(IToolbox toolbox)
        {
            this.toolbox = toolbox;
        }

        private string Key(int id)
        {
            return String.Concat(this.toolbox.Options.Value.UserCachePrifex, id);
        }


        private PlatformUser _currentUesr;

        private async Task<PlatformUser> LoaderAsync(int id)
        {

            if (id <= 0)
            {
                throw new CPValidateExceptions("userid must be greater than 0");
            }


            if (_currentUesr != null)
            {
                return _currentUesr;
            }

            _currentUesr = await this.toolbox.Cachor.Get(Key(id), async () =>
            {

                var entity = await this.toolbox.Reader.GetRepositoryAsync<PlatformUserEntity>()

                       .SingleAsync(u => u.UserId == id);

                if (entity == null)
                {
                    return null;
                }

                var userRoot = this.toolbox.Mapper.Map<PlatformUser>(entity);


                #region 加载上架的课程........
                //加载已上架的课程

                var listed = await this.toolbox.Reader.GetRepositoryAsync<CourseListedEntity>()
                             .Queryable(c => c.Creator == id)
                             .Select(c => new
                             {
                                 c.CourseId
                             }).Distinct()
                             .ToListAsync();

                if (listed.NoData() is false)
                {

                    userRoot.ListedCourse = userRoot.ListedCourse.Binder();

                    foreach (var item in listed)
                    {
                        userRoot.ListedCourse.Add(new ItemVal(item.CourseId));
                    }
                }

                #endregion

                return userRoot;




            });


            return _currentUesr;

        }


        private async Task<bool> Save(PlatformUser user)
        {
            await this.toolbox.Cachor.SetAsync(Key(user.UserId), user);
            await this.toolbox.Sender.SendAsync(user.Events);

            return true;
        }



        public async Task<PlatformUser> GetOrCreate(PltUserParam param)
        {

            if (param.UserId <= 0)
            {
                throw new CPValidateExceptions("userid must be greater than 0");
            }


            var user = await this.LoaderAsync(param.UserId);

            if (user == null)
            {
                user = new PlatformUser();
                user.Apply(this.toolbox.DomainSetter[DomainSetter.ID], this.toolbox.DomainSetter[DomainSetter.Mapper]);
                user.Create(param.UserId, param.Name, param.SchoolId, param.RegionId, param.SchoolName/*, param.ResearchGroup*/);
                await this.toolbox.Sender.SendAsync(user.Events);
                await this.Save(user);
            }

            return user;

        }


        public async Task<bool> JoinCourse(int userId, string courseId, int creator)
        {
            var user = await this.LoaderAsync(userId);

            var rev = user.JoinCourse(courseId, creator);

            if (rev == false)
            {
                return false;
            }

            await this.Save(user);
            await this.toolbox.Sender.SendAsync(user.Events);

            return true; ;
        }


        public async Task<bool> LeaveCourse(int userId, string courseId, int creatorId)
        {
            var user = await this.LoaderAsync(userId);

            var rev = user.LeaveCourse(courseId, creatorId);

            if (rev == false)
            {
                return false;
            }

            await this.Save(user);
            await this.toolbox.Sender.SendAsync(user.Events);

            return true; ;
        }




        public async Task<bool> ListedCourse(int userId, string courseId)
        {
            var user = await this.LoaderAsync(userId);
            var rev = user.CourseListed(courseId);
            if (rev is false)
            {
                return false;
            }
            await this.Save(user);
            return true;
        }


        public async Task<bool> ListCourseRemove(int userId, string courseId)
        {
            var user = await this.LoaderAsync(userId);
            var rev = user.CourseListedRemove(courseId);
            if (rev is false)
            {
                return false;
            }
            await this.Save(user);
            return true;
        }


        public async Task<bool> CreateCourse(PltUserParam param, string courseId)
        {
            var user = await this.GetOrCreate(param);
            Prosecutor.NotNull(user, "用户信息异常");
            var ret = user.CreateCoures(courseId);
            if (ret is false)
            {
                return false;
            }

            await this.Save(user);
            return true;
        }

        public async Task<bool> DelCourse(int userId, string courseId)
        {
            var user = await this.LoaderAsync(userId);

            Prosecutor.NotNull(user, "用户信息异常");

            var ret = user.DelCourse(courseId);

            if (ret is false)
            {
                return false;
            }

            return true;
        }


        //public async Task<PlatformUser> Get(int userid, string name, int schoolId, int regionId)
        //{
        //    var user = await this.LoaderAsync(userid);

        //    if (user == null)
        //    {
        //        user = new PlatformUser();
        //        user.Apply(this.toolbox.DomainSetter[DomainSetter.ID], this.toolbox.DomainSetter[DomainSetter.Mapper]);
        //        user.Create(userid, name, schoolId, regionId);
        //        await this.toolbox.Sender.SendAsync(user.Events);
        //        await this.Save(user);
        //    }
        //    return user;
        //}








    }
}
