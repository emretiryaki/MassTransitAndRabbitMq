using System.Runtime.Serialization;

namespace Sender.Contract
{
    [DataContract]
    public class ResponseBase
    {
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string ResponseMessage { get; set; }
    }
}