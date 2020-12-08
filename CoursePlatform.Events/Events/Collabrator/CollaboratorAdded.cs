using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CollaboratorAdded : ICoursePlatformEvent
    {
        public string CourseId { get; set; }

        public IList<CollabratorEventItem> Items { get; set; }

    }


    public class CollabratorEventItem
    {

        public int ObjId { get; set; }

        public string CourseId { get; set; }


        public string ObjName { get; set; }


        /// <summary>
        /// 根Id
        /// </summary>

        public int RootId { get; set; }



        /// <summary>
        /// 根名称
        /// </summary>

        public string RootName { get; set; }


        /// <summary>
        /// 组织Id，班级或角色
        /// </summary>

        public int OrgId { get; set; }


        /// <summary>
        /// 组织名称
        /// </summary>

        public string OrgName { get; set; }


        /// <summary>
        /// 分享类型
        /// </summary>

        public int Type { get; set; }
    }
}
