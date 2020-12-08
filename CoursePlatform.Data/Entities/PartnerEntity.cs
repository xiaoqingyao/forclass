using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data.Entities
{
    public  class PartnerEntity : EntityBase
    {

        [IndexColumn]
        public int Type { get; set; }


        public int CourseCount { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        /// <summary>
        /// 对应资源接口的ID
        /// </summary>
        public int ResourceId { get; set; }


        /// <summary>
        /// 上级单位Id
        /// </summary>
        public int ParentId { get; set; }



    }
}
