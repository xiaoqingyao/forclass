using CoursePlatform.Domain.Core.PartnerAggregate;
using CoursePlatform.Domain.Core.ValueFactory;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace CoursePlatform.Domain.Core.CourseAggregate
{
    [Serializable]
    public class CollabratorVal : ValueBase<int, CollabratorItem>
    {
  
    }

    public record CollabratorItem(int ObjId): ValItem<int>(ObjId)
    {


        
    }
}
