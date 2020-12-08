using CoursePlatform.Asset;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.EFProvider.Paging;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Queries.Share;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries.Collabrator
{
    public class CollabratorQuery : ICollabratorQuery 
    {


        private readonly IUserProxy _uproxy;

        private readonly IUnitOfWork<CPDbContext> _unitOfWork;

        private readonly IAppUser _user;


        public CollabratorQuery(IUserProxy userAPI,IUnitOfWork<CPDbContext> unitOfWork, IAppUser user)
        {
            _uproxy = userAPI;
            _unitOfWork = unitOfWork;
            _user = user;
        }



        /// <summary> 
        ///  教育组可用对象
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        private async Task<IList<TeacherVO>> EnableComunity(IPaginate<CollabratorEntity> shared)
        {

            var rev = await this._uproxy.ComunityMemberByUser(this._user.Session);


            if (rev == null || rev.Count == 0)
            {
                return null;
            }

            IList<TeacherVO> tv = new List<TeacherVO>();

            foreach (var item in rev)
            {

                //教研组
                var comunity = new TeacherVO
                {
                    Id = item.Idx,
                    Name = item.Name
                };

                //角色
                foreach (var role in item.ChildList)
                {

                    var roleAry = new TeacherVO
                    {
                        Id = role.Idx,
                        Name = role.Name
                    };

                    // 教师
                    foreach (var tc in role.ChildList)
                    {
                        if (shared.Count > 0 && shared.Items.Any(t => t.ObjId == tc.Idx))
                        {
                            continue;
                        }
                        if (tc.Idx == this._user.UserId)
                        {
                            continue;
                        }
                        roleAry.Children.Add(new TeacherVO
                        {

                            Id = tc.Idx,
                            Name = tc.Name

                        });
                    }

                    comunity.Children.Add(roleAry);
                }

                tv.Add(comunity);
            }

            return tv;



        }


        private async Task<IList<TeacherVO>> SchooleEnabled(IPaginate<CollabratorEntity> shared)
        {


            var school = await this._user.GetSchool();

            var t = await this._uproxy.TeacherGroupClass(this._user.Session, school.Code);  //this._uproxy.TeacherByClass(session); //await this._uproxy.SchoolTeacherByUser(session);


            IList<TeacherVO> ts = new List<TeacherVO>();

            var section = t.First().ChildList;

            //学段

            foreach (var cl in section)
            {

                //年级
                foreach (var grade in cl.ChildList)
                {



                    // 年级节点
                    var tv = new TeacherVO
                    {
                        Id = grade.Idx,
                        Name = grade.Name,
                        Children = new List<TeacherVO>()
                    };

                    //年级下的班级
                    foreach (var tc in grade.ChildList)
                    {

                        //班级节点
                        var gradeNode = new TeacherVO
                        {
                            Id = tc.Idx,
                            Name = tc.Name
                        };

                        if (tc.ChildList == null || tc.ChildList.Count == 0)
                        {
                            continue;
                        }

                        //班级下的教师
                        foreach (var teacherItem in tc.ChildList)
                        {
                            if (shared != null && shared.Items.Count > 0 && shared.Items.Any(s => s.ObjId == teacherItem.Idx))
                            {
                                continue;
                            }
                            if (teacherItem.Idx == this._user.UserId)
                            {
                                continue;
                            }

                            var teacherNode = new TeacherVO
                            {
                                Id = teacherItem.Idx,
                                Name = teacherItem.Name
                            };
                            gradeNode.Children.Add(teacherNode);

                        }

                        tv.Children.Add(gradeNode);


                    }

                    ts.Add(tv);

                }

            }

            return ts;

        }



        /// <inheritdoc/>
        public async Task<IList<TeacherVO>> EnableAsync(CollabratorType type, string cid)
        {

            if ((type & (CollabratorType.Community | CollabratorType.School)) == 0)
            {
                throw new CPValidateExceptions("未知的查询对象");
            }

            var shared = await this._unitOfWork.GetRepositoryAsync<CollabratorEntity>()
                            .GetListAsync(d => d.CourseId == cid);


            if (type == CollabratorType.School)
            {
                return await this.SchooleEnabled(shared);
            }

            return await this.EnableComunity(shared);


        }

        /// <inheritdoc/>
        public async Task<QueryCollabratorVO> GetAsync(string cid)
        {
            var rev = await this._unitOfWork.GetRepositoryAsync<CollabratorEntity>()
                                                .GetListAsync(d => d.CourseId == cid );
            if (rev == null || rev.Items.Count == 0)
            {
                return null;
            }



            var objs = rev.Items.GroupBy(s => s.Type);



            IList<TeacherVO> schoolObj = BuildVO(objs.FirstOrDefault(f => f.Key == (int)CollabratorType.School)?.ToList());  //new List<TeacherVO>();
            IList<TeacherVO> GroupObj = BuildVO(objs.FirstOrDefault(f => f.Key == (int)CollabratorType.Community)?.ToList());




            return new QueryCollabratorVO
            {
                CommunityObj = GroupObj,
                SchoolObj = schoolObj
            };

            // return rev.Items.Select(s => new TeacherVO { Id = s.ObjId, Name = s.ObjName }).ToList();



        }


        private static IList<TeacherVO> BuildVO(IList<CollabratorEntity> data)
        {

            if (data == null || data.Count == 0)
            {
                return null;
            }

            IList<TeacherVO> revAry = new List<TeacherVO>();

            var rootGroup = data.GroupBy(r => new { r.RootId, r.RootName }); // root级

            foreach (var root in rootGroup)
            {
                var revItem = new TeacherVO
                {
                    Id = root.Key.RootId,
                    Name = root.Key.RootName
                };

                var orgGroup = root.GroupBy(o => new { o.OrgId, o.OrgName }); // org级

                foreach (var item in orgGroup)
                {
                    var revOrg = new TeacherVO
                    {
                        Id = item.Key.OrgId,
                        Name = item.Key.OrgName
                    };

                    foreach (var o in item) // obj级
                    {
                        revOrg.Children.Add(new TeacherVO
                        {
                            Id = o.ObjId,
                            Name = o.ObjName
                        });


                    }
                    revItem.Children.Add(revOrg);
                }
                if (revItem.Id == 0)
                {
                    ((List<TeacherVO>)revAry).AddRange(revItem.Children);
                    continue;
                }
                revAry.Add(revItem);
            }

            return revAry;
        }


    }
}
