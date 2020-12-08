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
    public class CourseListedEntity : EntityBase
    {

        [Column(TypeName = "nvarchar(50)")]
        public string CourseId { get; set; }


        [IndexColumn]
        public int OrgId { get; set; }

        [IndexColumn]
        public int Type { get; set; }



        [Column(TypeName = "nvarchar(50)")]
        public string OrgName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string PltId { get; set; }


        public int ParentId { get; set; }



        public int Creator { get; set; }

    }
}
