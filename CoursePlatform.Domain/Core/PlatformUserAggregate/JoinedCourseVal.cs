using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Emit;
using System.Text;
using CoursePlatform.infrastructure.Validators;

namespace CoursePlatform.Domain.Core.PlatformUserAggregate
{
    public class JoinedCourseVal
    {

        public IList<CourseItemVal> Items { get; set; }

        public IEnumerable<CourseItemVal> AddItem(params CourseItemVal[] val)
        {
            if (this.Items == null)
            {
                this.Items = new List<CourseItemVal>();
            }

            foreach (var item in val)
            {
                if (this.Items.Contains(item))
                {
                    continue;
                }
                this.Items.Add(item);
                yield return item;
            }
        }

        public bool Remove(CourseItemVal val)
        {

            if (this.Items.NoData())
            {
                return false;
            }

            return this.Items.Remove(val);
               
            
        }
    }


    public class  CourseItemVal
    {
        public string CourseId { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CourseItemVal val))
            {
                return false;
            }

            if (val.CourseId != this.CourseId)
            {
                return false;
            }

            return true;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.CourseId);
        }
    }
}
