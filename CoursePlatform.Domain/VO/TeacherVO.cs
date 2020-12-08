using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.VO
{




    /// <summary>
    /// 已经分享的对象
    /// </summary>
    public class QuerySharedVO
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
        public string Id { get; set; }



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
