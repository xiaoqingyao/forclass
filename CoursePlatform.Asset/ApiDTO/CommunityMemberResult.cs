using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Asset.ApiDTO
{
    public class CommunityMemberRootobject
    {
        public IList<CommunityMemberResult> result { get; set; }
        public object NextPage { get; set; }
        public int ReturnCode { get; set; }
        public object ReturnText { get; set; }
    }

    public class CommunityMemberResult
    {
        public int Num { get; set; }
        public int Idx { get; set; }
        public string Name { get; set; }
        public object Prop { get; set; }
        public IList<CommunityMemberResult> ChildList { get; set; }
    }

}
