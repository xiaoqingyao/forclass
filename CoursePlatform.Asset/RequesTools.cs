using CoursePlatform.infrastructure.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Asset
{
    public class RequesTools
    {
        public static async Task<T> GetAsync<T>(string session, string urlPart, Action<IRestRequest> rqBuilder = null) where T : class, new()
        {

            IRestResponse rs = null;

            try
            {
                rs = await Client(urlPart).ExecuteAsync(Rq(session, rqBuilder));

                var rev = JsonConvert.DeserializeObject<T>(rs.Content);


                return rev;

            }
            catch (Exception)
            {

                throw new CP3PartApiException($"接口异常:{rs?.Content}-{urlPart}");
            }
        }


        public static T Get<T>(string session, string urlPart, Action<IRestRequest> rqBuilder = null) where T : class, new()
        {

            IRestResponse rs = null;

            try
            {
                rs = Client(urlPart).Execute(Rq(session, rqBuilder));

                var rev = JsonConvert.DeserializeObject<T>(rs.Content);


                return rev;

            }
            catch (Exception)
            {

                throw new CP3PartApiException($"接口异常:{rs?.Content}--{urlPart}");
            }
        }


        public static IRestRequest Rq(string session, Action<IRestRequest> rqBuild)
        {
            var ret = new RestRequest(Method.POST).AddParameter("session", session);
            rqBuild?.Invoke(ret);
            return ret;
        }

        public static IRestClient Client(string urlPart)
        {
            return new RestClient(urlPart);
        }

    }
}
