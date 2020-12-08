using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Asset.ApiDTO
{


    public class UserInfoDTO
    {
        [JsonProperty("retcode")]
        public long? Retcode { get; set; }

        [JsonProperty("result")]
        public IList<UserItem> Result { get; set; }

        [JsonProperty("NextPage")]
        public int? NextPage { get; set; }

        [JsonProperty("ReturnCode")]
        public long? ReturnCode { get; set; }

        [JsonProperty("ReturnText")]
        public string ReturnText { get; set; }
    }

    public class UserItem
    {
        public int UserID { get; set; }
        public string RealName { get; set; }
        public string Avatar { get; set; }
        public string Session { get; set; }


        /// <summary>
        /// 组织
        /// </summary>
        public IDictionary<string, OrgItem> Org { get; set; }// = new Dictionary<string, IList<OrgItem>>();



        //public void SetOrg(OrgItem org)
        //{
        //    if (org == null)
        //    {
        //        return;
        //    }
        //    if (this.Org == null)
        //    {
        //        this.Org = new Dictionary<string, OrgItem>();
        //    }

        //    if (!String.IsNullOrEmpty(org.Prop) && this.Org.TryGetValue(org.Prop, out OrgItem item) == false)
        //    {
        //        this.Org[item.Prop] = item;
        //    }

        //    if (org.HasChildern)
        //    {

        //    }

        //}

    }
}
