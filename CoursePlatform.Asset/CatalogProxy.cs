using CoursePlatform.Asset.ApiDTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Asset
{
    public interface ICatalogProxy
    {
        Task<IList<CatalogResult>> GetAsync(CatalogRequestParam param);
    }


    public class CatalogProxy:ICatalogProxy
    {

        private readonly IOptions<ApiOptions> _opt;

        public CatalogProxy(IOptions<ApiOptions> opt)
        {
            _opt = opt;
        }

        public async Task<IList<CatalogResult>> GetAsync(CatalogRequestParam param)
        {
            var client = new RestClient($"{_opt.Value.CatalogUrl}?session={param.Session}&period_id={param.Period_id}&subject_id={param.SubjectId}&edition_id={param.EditionId}&term_id={param.TermId}&book_id={param.BookId}&uIdx={param.UIdx}&seeBookIdx={param.SeeBookIdx}")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            var res = JsonConvert.DeserializeObject<CatalogRoot>(response.Content);

            if (res == null || res.result == null || res.result.Count == 0)
            {
                return null;
            }

            return res.result;

        }


    }
}
