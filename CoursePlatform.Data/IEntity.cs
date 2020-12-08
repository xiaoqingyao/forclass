using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Data
{
     interface IEntity
    {

        int IndentityId { get; set; }


        long Id { get; set; }

         int Deleted { get; set; }


         DateTime? CreationTime { get; set; }// = DateTime.Now;


         DateTime? UpdateTime { get; set; }// = DateTime.Now;
    }
}
