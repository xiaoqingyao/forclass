using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Infrastructure.Serializer
{
    public class Serializer : ISerializer
    {

        public Serializer()
        {
            this.Settgins = new JsonSerializerSettings
            {

                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize

            };
        }


        private readonly JsonSerializerSettings Settgins;

        public T Deserial<T>(string source)
        {
            try
            {

            return JsonConvert.DeserializeObject<T>(source, this.Settgins);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string Serial(object obj)
        {
            return JsonConvert.SerializeObject(obj, this.Settgins);
        }


        public object Deserial(string source, Type type)
        {
            return JsonConvert.DeserializeObject(source, type, this.Settgins);
        }
    }
}
