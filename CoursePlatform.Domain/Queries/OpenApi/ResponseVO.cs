using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries.OpenApi
{
    public class GetCourseListRsp
    {


        /// <summary>
        /// 课程信息
        /// </summary>
        public IList<CourseInfo> Courses { get; set; }

        /// <summary>
        /// 课程总数
        /// </summary>
        public int Totalcount { get; set; }



    }

    public class CourseInfo
    {

        /// <summary>
        /// 课程Id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreateId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>

        public string CreateName { get; set; }


        /// <summary>
        /// 课时数
        /// </summary>
        public int LessonCount { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        /// <remarks>
        ///       "区域上架": 80
        /// </remarks>
        public int Status { get; set; }

        /// <summary>
        /// 造用年级
        /// </summary>
        public IList<int> GradeIds { get; set; }


        /// <summary>
        /// 加入人数
        /// </summary>
        public int JoinCount { get; set; }

        /// <summary>
        /// 完成人数，（无数据）
        /// </summary>
        public int FinishCount { get; set; }


        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        public string CreationTime { get; set; }


        public static int[] GradeInfo(string cid, string cname)
        {
            var rev = cid?.Split('/');


            if (rev is null or { Length: < 4 })
            {
                return null;
            }

            if (int.TryParse(rev[3], out int result))
            {
                return new int[] { result };
            }

            return null;
            //rev += '.';

            //rev += cname?.Split('/')[3];

            //return new string[] { rev }jkk;

        }

    }
}
