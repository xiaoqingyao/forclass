using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Data.Entities
{
    public class HostInfoEntity : EntityBase
    {

        [Column(TypeName = "nvarchar(100)")]
        public string UUID { get; set; }


        public bool IsRunning { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string IP { get; set; }




    }
}
