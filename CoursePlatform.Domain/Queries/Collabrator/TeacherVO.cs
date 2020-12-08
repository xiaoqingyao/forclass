using System.Collections.Generic;

namespace CoursePlatform.Domain.Queries.Collabrator
{



    /// <summary>
    /// 已经分享的对象
    /// </summary>
    public class QueryCollabratorVO
    {
        /// <summary>
        /// 教研组
        /// </summary>
        public IList<TeacherVO> CommunityObj { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        public IList<TeacherVO> SchoolObj { get; set; }
    }





    /// <summary>
    /// 教师信息
    /// </summary>
    public class TeacherVO
    {

        public TeacherVO()
        {
            this.Children = new List<TeacherVO>();
        }



        /// <summary>
        /// 教师ID
        /// </summary>
        public int Id { get; set; }



        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 子级
        /// </summary>
        public IList<TeacherVO> Children { get; set; }


    }
}
