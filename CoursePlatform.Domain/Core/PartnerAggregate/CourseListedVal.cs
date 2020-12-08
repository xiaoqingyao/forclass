using CoursePlatform.Domain.Core.ValueFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.PartnerAggregate
{
    public class CourseListedVal : ValueBase<string, ValItem<string>>
    {
        
    }


    public record CourseItem(string Key) : ValItem<string>(Key)
    {

       
    }
}
