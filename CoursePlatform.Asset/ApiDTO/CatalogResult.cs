using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Asset.ApiDTO
{
     public class CatalogResult
    {
        public string Idx { get; set; }
        public string Name { get; set; }
        public string Prop { get; set; }
        public int Count { get; set; }
        public List<CatalogResult> ChildList { get; set; }

        public int Id
        {
            get
            {
                if (int.TryParse(this.Idx, out int val))
                {
                    return val;
                }

                return 0;
            }
        }
    }

    public class CatalogRoot
    {
        public int retcode { get; set; }
        public List<CatalogResult> result { get; set; }
        public object NextPage { get; set; }
        public int ReturnCode { get; set; }
        public object ReturnText { get; set; }
    }

    public class CatalogRequestParam
    {
        public string Session { get; set; }

        [JsonProperty("period_id")]
        public int Period_id { get; set; }

        [JsonProperty("subject_id")]
        public int SubjectId { get; set; }


        [JsonProperty("term_id")]
        public int  TermId { get; set; }


        [JsonProperty("edition_id")]
        public int EditionId { get; set; }

        [JsonProperty("book_id")]
        public int BookId { get; set; }

        [JsonProperty("uIdx")]
        public int UIdx { get; set; }

        [JsonProperty("seeBookIdx")]
        public int SeeBookIdx { get; set; }
    }
}
