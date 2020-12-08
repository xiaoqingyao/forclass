using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data.EFProvider
{
    public abstract class EntityBase
    {


        /// <summary>
        /// 自增Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int IndentityId { get; set; }


        [IndexColumn,Column(TypeName = "nvarchar(100)")]
        public string ID { get; set; }


        [IndexColumn]
        public int Deleted { get; set; }


        public DateTime? CreationTime { get; set; } = DateTime.Now;


        public DateTime? UpdateTime { get; set; }// = DateTime.Now;
    }
}
