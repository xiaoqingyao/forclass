using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CourseDSQuoted : ICoursePlatformEvent
    {


            public IList<QuoteDsEventData> Items { get; set; }

    }


    public class QuoteDsEventData
    {
        public int OperatorId { get; set; }

        public string OperatorName { get; set; }

        public Guid DsId { get; set; }

        public string DsName { get; set; }


        public int SortVal { get; set; }


        public int CatalogId { get; set; }


        public bool IsOpen { get; set; }


        public string CourseId { get; set; }


        public string Cover { get; set; }

        public bool IsShared { get; set; }

        /// <summary>
        /// 是否原创
        /// </summary>
        public bool IsOriginal { get; set; }

    }
}
