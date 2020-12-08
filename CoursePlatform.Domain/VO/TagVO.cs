using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoursePlatform.Domain.VO
{

    /// <summary>
    /// 课程署名
    /// </summary>
    public class TagVO
    {


        /// <summary>
        /// 如果Id
        /// </summary>
        public int Id { get; set; }


        public string Name { get; set; }


        public IList<TagVOItem> Items { get; set; }


        public IList<TagVOItem> AddItem(params TagVOItem[] val)
        {
            if (Items == null)
            {
                this.Items = new List<TagVOItem>();
            }

            foreach (var item in val)
            {
                this.Items.Add(item);

            }


            return this.Items;
        }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TagVO val))
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
            return HashCode.Combine(this.GetHashCode(), this.Id, this.Name, String.Join(",", this.Items.Select(item => item.ToString()).ToArray()));
        }
    }


    public class TagVOItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }


        public override bool Equals(object obj)
        {
            if (!(obj is TagVOItem val))
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
