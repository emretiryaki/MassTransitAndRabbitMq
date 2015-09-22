using System;

namespace Rabbit.Core
{
    public interface IJsonSerializer
    {
        /// <summary>
        /// object tipli sınıfı json string döner
        /// </summary>
        /// <param name="source">json serialize edilecek sınıf</param>
        /// <returns>json string</returns>
        string JsonSerialize(dynamic source);

        /// <summary>
        /// json string object çevirir
        /// </summary>
        /// <typeparam name="T">dönüşümü yapılacak sınıf</typeparam>
        /// <param name="source">json string</param>
        /// <returns>dönüşüm yapılacak sınıf</returns>
        T JsonDeserialize<T>(string source);


        /// <summary>
        /// return dynamic after deserialization
        /// </summary>       
        dynamic JsonDeserialize(string source, Type type);
    }
}