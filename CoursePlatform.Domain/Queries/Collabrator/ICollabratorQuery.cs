using CoursePlatform.Domain.Queries.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries.Collabrator
{
    public interface ICollabratorQuery
    {

        /// <summary>
        /// 可以分享的教师 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dsId"></param>
        /// <returns></returns>
        Task<IList<TeacherVO>> EnableAsync(CollabratorType type, string dsId);




        /// <summary>
        /// 已经分享的教师
        /// </summary>
        /// <param name="dsId"></param>
        /// <returns></returns>
        Task<QueryCollabratorVO> GetAsync(string dsId);

    }
}
