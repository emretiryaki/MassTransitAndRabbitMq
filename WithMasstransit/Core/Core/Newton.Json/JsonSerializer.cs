using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rabbit.Core
{
    public class JsonSerializer : IJsonSerializer
    {
        public JsonSerializer()
        {

        }

        /// <summary>
        /// object tipli sınıfı json string döner
        /// </summary>
        /// <param name="source">json serialize edilecek sınıf</param>
        /// <returns>json string</returns>
        public string JsonSerialize(dynamic source)
        {
            if (source == null)
                return string.Empty;

            return JsonConvert.SerializeObject(source, Formatting.Indented);
        }

        /// <summary>
        /// json string object çevirir
        /// </summary>
        /// <typeparam name="T">dönüşümü yapılacak sınıf</typeparam>
        /// <param name="source">json string</param>
        /// <returns>dönüşüm yapılacak sınıf</returns>
        public T JsonDeserialize<T>(string source)
        {
            if (source == string.Empty)
            {
                return default(T);
            }

            var value = JsonConvert.DeserializeObject<T>(source);
            return value;
        }



        public dynamic JsonDeserialize(string source, Type type)
        {
            dynamic returnValue = JsonConvert.DeserializeObject(source, type);
            return returnValue;
        }
    }


}
