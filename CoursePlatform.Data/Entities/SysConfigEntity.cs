using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Data.Entities
{
    public class SysConfigEntity : EntityBase
    {

        [Column(TypeName = "nvarchar(1000)")]
        public string TagAttr { get; set; }

    }
}
