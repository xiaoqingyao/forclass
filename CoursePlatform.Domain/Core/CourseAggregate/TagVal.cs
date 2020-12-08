using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoursePlatform.Domain.Core.CourseAggregate
{


    /// <summary>
    /// 课程署名
    /// </summary>
    public class TagVal
    {


        /// <summary>
        /// 如果Id > 0 教研组, 反之 自签
        /// </summary>
        public int Id { get; set; }


        public string Name { get; set; }


        public IList<TagItem> Items { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TagVal val))
            {
                return false;
            }

            if (val.Id != this.Id)
            {
                return false;
            }

            if (val.Items.Count != this.Items.Count)
            {
                return false;
            }

            foreach (var item in this.Items)
            {
                if (val.Items.Contains(item) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.GetHashCode(), this.Id, this.Name, String.Join(",",this.Items.Select(item => item.ToString()).ToArray()));
        }
    }


    public class TagItem
    {
        public int Id { get; }

        public string Name { get; set; }

        public string TypeName { get; set; }


        public override bool Equals(object obj)
        {
            if (!(obj is TagItem val))
            {
                return false;
            }

            if (val.Id != this.Id
                || val.Name != this.Name
                || val.TypeName != this.Name)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return String.Concat(this.Id, this.Name, this.TypeName);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.Id, this.Name, this.TypeName);
        }

    }


}
