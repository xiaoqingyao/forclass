using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.Core.CourseAggregate
{

    public class QuoteDSVal
    {
        public int OperatorId { get; set; }

        public string OperatorName { get; set; }

        public Guid DsId { get; set; }

        public string DsName { get; set; }

        public int SortVal { get; set; }


        public int CatalogId { get; set; }


        public bool IsOpen { get; set; }


        public string Cover { get; set; }

        public bool IsShared { get; set; }

        /// <summary>
        /// 是否原创
        /// </summary>
        public bool IsOriginal { get; set; }



        public override bool Equals(object obj)
        {
            if (obj is not QuoteDSVal val)
            {
                return false;
            }
            if (val.CatalogId == this.CatalogId && val.DsId == this.DsId)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.CatalogId, this.DsId);
        }
    }
}
