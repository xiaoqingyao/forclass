using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Asset.ApiDTO
{
    public class OrgItem
    {
        [JsonProperty("Idx")]
        public int? Idx { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Prop")]
        public string Prop { get; set; }

        public int Num { get; set; }

        [JsonProperty("ChildList")]
        public IList<OrgItem> ChildList { get; set; }


        public void AddChild(OrgItem item)
        {
            if (this.ChildList == null)
            {
                this.ChildList = new List<OrgItem>();
            }
            this.ChildList.Add(item);
        }

        public override bool Equals(object obj)
        {

            if (obj is OrgItem orgItem)
            {
                if (orgItem.Idx == this.Idx)
                {
                    return true;
                }
            }

            return false;

        }

        public bool HasChildern()
        {
            return this.ChildList != null && this.ChildList.Count > 0;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Idx);
        }
    }



    public class OrgDTO 
    {
        [JsonProperty("result")]
        public IList<OrgItem> Result { get; set; }

        [JsonProperty("NextPage")]
        public object NextPage { get; set; }

        [JsonProperty("ReturnCode")]
        public long ReturnCode { get; set; }

        [JsonProperty("ReturnText")]
        public object ReturnText { get; set; }
    }

}
