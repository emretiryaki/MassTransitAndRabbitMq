using System;
using System.Runtime.Serialization;

namespace Sender.Contract
{
    [DataContract]
    public class ProductRequest : RequestBase
    {
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string ProductName { get; set; }
    }
      
    [DataContract]
    public class RequestBase
    {
        public Guid TrackId { get; set; }
    }
}
