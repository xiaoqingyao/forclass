using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.DTOS.Queries
{
    public interface IFilterParam
    {

        /// <summary>
        /// 查询类型: t: 老师， s: 学用， a: 审核用
        /// </summary>
        [Required]
        public char QueryType { get; set; }



        /// <summary>
        /// 组织Id
        /// </summary>
        public int OrgId { get; set; }


        /// <summary>
        /// 组织类型 s:学校，r:区域
        /// </summary>
        public char OrgType { get; set; }


    }


    public class FilterParam : IFilterParam
    {
        public char QueryType { get; set; }
        public int OrgId { get; set; }
        public char OrgType { get; set; }
    }
}
