using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Data.Entities
{

   
    /// <summary>
    /// 群组推荐的课程
    /// </summary>
    public class GroupRecommendEntity : EntityBase
    {

        public int GroupId { get; set; }

        public int GroupName { get; set; }


        public string CourseId { get; set; }
    }

}
