using CoursePlatform.Events;
using CoursePlatform.Events.Events;
using CoursePlatform.Events.Events.platformUser;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoursePlatform.Domain.Core.PlatformUserAggregate
{
    public class PlatformUser : AggregateRoot
    {


        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// 所属学校ID
        /// </summary>
        public int SchoolId { get; set; }


        /// <summary>
        /// 区域Id
        /// </summary>
        public int SectionId { get; set; }

        public string SchoolName { get; set; }


        public int ResearchGroupName { get; set; }


        public string ResearchGroupId { get; set; }


        public string Name { get; set; }




        /// <summary>
        /// 已上架的商品
        /// </summary>
        public ListedVal ListedCourse { get; set; }


        /// <summary>
        /// 加入学习的学生数
        /// </summary>
        public int StdJoined { get; set; }



        public JoinedCourseVal Course { get; set; }

        
        public OwnerCourseVal OwnerCourse { get; set; }


        internal void Create(int userid, string name, int schoolId, int sectionId, string schoolName/*, UserPropVal researchGroup*/)
        {
            this.UserId = userid;
            this.Name = name;
            this.SchoolId = schoolId;
            this.SectionId = sectionId;

            this.AddEvent(new PlatformUserCreated(this.ID, userid, name, schoolId, sectionId, schoolName/*, researchGroup*/));

        }




        internal bool JoinCourse(string courseId, int creatorId)
        {
            this.Course = this.Course.Binder();


            var rev = this.Course.AddItem(new CourseItemVal
            {
                CourseId = courseId
            }).ToList();


            if (rev.NoData())
            {
                return false;
            }

            foreach (var item in rev)
            {
                this.AddEvent(new UserJoined
                {
                    CourseId = item.CourseId,
                    PlatUserId = this.ID,
                    UserId = this.UserId,
                    CreatorId = creatorId
                });
            }

            return true;
        }

        internal bool LeaveCourse(string courseId, int creatorId)
        {
            if (this.Course == null || this.Course.Items.NoData())
            {
                return false; 
            }

            if (this.Course.Remove(new CourseItemVal { CourseId = courseId }) == false)
            {
                return false;
            }

            this.AddEvent(new UserCourseLeaved
            {
                UserId = this.UserId,
                CourseId = courseId,
                CreatorId = creatorId
            });

            return true;

        }

        internal bool CourseListed(string courseId)
        {
            this.ListedCourse = this.ListedCourse.Binder();

            if(this.ListedCourse.Add(new ItemVal(courseId)) is false)
            {
                return false; 
            }
            this.AddEvent(new CourseListedChanged(this.UserId, this.ListedCourse.Count));
            return true;
        }

        internal bool CourseListedRemove(string courseId)
        {
            if (this.ListedCourse is null or {Count: <= 0 })
            {
                return false;
            }
            if (this.ListedCourse.Remove(courseId) is false)
            {
                return false;
            }
            this.AddEvent(new CourseListedChanged(this.UserId, this.ListedCourse.Count));
            return true;
        }

        internal bool CreateCoures(string courseId)
        {
            this.OwnerCourse = this.OwnerCourse.Binder();
            bool rev = this.OwnerCourse.Add(new OwnerCourseItemVal(courseId));

            if (rev is false)
            {
                return false;
            }

            this.AddEvent(new PltUserCourseCountChanged(this.UserId, this.OwnerCourse.Count));

            return true;
        }

        internal object DelCourse(string courseId)
        {
            this.OwnerCourse = this.OwnerCourse.Binder();
            bool rev = this.OwnerCourse.Remove(courseId);

            if (rev is false)
            {
                return false;
            }

            this.AddEvent(new PltUserCourseCountChanged(this.UserId, this.OwnerCourse.Count));

            return true;

        }
    }
}
