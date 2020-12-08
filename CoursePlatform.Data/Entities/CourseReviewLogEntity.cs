using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Data.Entities
{
    public class CourseReviewLogEntity : EntityBase
    {


        [Column(TypeName = "nvarchar(20)")]
        public string CourseId { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string ReviewerName { get; set; }

        public int ReviewerId { get; set; }

        [Column(TypeName ="nvarchar(1000)")]
        public string Desc { get; set; }

        public int ReviewerOrgId { get; set; }
        
        [Column(TypeName = "nvarchar(20)")]
        public string ReviewerOrgName { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string StatusDesc { get; set; }
    }
}
