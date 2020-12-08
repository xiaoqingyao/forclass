using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Asset.ApiDTO
{
    public class UserCommunityRootobject
    {
        public IList<CommunityResult> result { get; set; }
        public object NextPage { get; set; }
        public int ReturnCode { get; set; }
        public object ReturnText { get; set; }
    }

    public class CommunityResult
    {
        public string Value { get; set; }
        public string Total { get; set; }
        public int Num { get; set; }
        public string Idx { get; set; }
        public string Name { get; set; }
        public string Prop { get; set; }
    }
}
