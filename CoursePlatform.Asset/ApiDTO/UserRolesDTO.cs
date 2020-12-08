using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Asset.ApiDTO
{
    public class UserRoleDTO
    {
        public IList<RoleItem> result { get; set; }
        public string NextPage { get; set; }
        public int ReturnCode { get; set; }
        public string ReturnText { get; set; }
    }

    public class RoleItem
    {
        public int Idx { get; set; }
        public string Name { get; set; }
        public string Prop { get; set; }
    }
}
