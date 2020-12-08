using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data.Entities
{
    public class QuoteDsEntity : EntityBase
    {

        [IndexColumn]
        public int OperatorId { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string OperatorName { get; set; }


        public Guid DsId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string DsName { get; set; }


        [IndexColumn]
        public int SortVal { get; set; }



        [IndexColumn]
        public int CatalogId { get; set; }



        public bool IsOpen { get; set; }


        [IndexColumn]
        [Column(TypeName = "nvarchar(50)")]
        public string CourseId { get; set; }


        [Column(TypeName = "nvarchar(500)")]
        public string Cover { get; set; }


        public bool IsShared { get; set; }


        /// <summary>
        /// 是否原创
        /// </summary>
        public bool IsOriginal { get; set; }
    }
}
