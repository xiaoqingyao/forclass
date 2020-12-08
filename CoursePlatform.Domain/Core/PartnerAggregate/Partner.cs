using CoursePlatform.Domain.Core.ValueFactory;
using CoursePlatform.Events;
using CoursePlatform.Events.Events.Partner;
using CoursePlatform.infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.PartnerAggregate
{
    public class Partner : AggregateRoot
    {

        public PartnerType Type { get; set; }

        public int CourseCount { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 对应资源接口的ID
        /// </summary>
        public int ResourceId { get; set; }


        /// <summary>
        /// 上级单位Id
        /// </summary>
        public int ParentId { get; set; }

        public CourseListedVal CourseListed { get; set; }

        internal void Create(int objId, string name, PartnerType type, int parentId)
        {
            this.ResourceId = objId;
            this.Name = name;
            this.Type = type;
            this.AddEvent(new PartnerCreated(ResourceId, name, (int)type)
            {
                Id = this.ID,
                ParentId = parentId 
            });
        }

        internal bool AddToList(string courseId, int creator)
        {
            this.CourseListed = this.CourseListed.Binder();
            bool rev = this.CourseListed.Add(new ValItem<string>(courseId));
            if (rev)
            {
                this.AddEvent(new PartnerSetCourseListed(creator,this.ResourceId, this.ID,this.ParentId, this.Name, (int)this.Type, courseId, this.CourseListed.Count));
            }
            return rev;

        }

        internal bool Unlisted(string courseId)
        {
            if (this.CourseListed is null or {Count: <= 0 })
            {
                return false;
            }

            bool rev = this.CourseListed.Remove(courseId);
            if (rev is false)
            {
                return false;
            }
            this.AddEvent(new PartnerCourseListRemoved(this.ResourceId, (int)this.Type, this.CourseListed.Count, courseId));

            return true;

        }
    }


}
