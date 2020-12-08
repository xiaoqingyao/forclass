using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Asset.ApiDTO
{
    public class TeacherGroupClassRoot
    {
        public IList<TeacherGroupClassResult> result { get; set; }
        public object NextPage { get; set; }
        public int ReturnCode { get; set; }
        public object ReturnText { get; set; }
    }

    public class TeacherGroupClassResult
    {
        public int Num { get; set; }
        public int Idx { get; set; }
        public string Name { get; set; }
        public string Prop { get; set; }
        public IList<TeacherGroupClassResult> ChildList { get; set; }
    }
}
