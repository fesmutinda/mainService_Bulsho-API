using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace mainService.Model
{
    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string ResponseBody { get; set; }
        [DataMember]
        public MemberData MemberData { get; set; }
    }
    public class MemberData
    {
        [DataMember]
        public string MemberShares { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("MemberNumber")]
        public string MemberNumber { get; set; }
        [JsonProperty("FullName")]
        public string FullName { get; set; }
        [JsonProperty("IDNumber")]
        public string IDNumber { get; set; }
        [JsonProperty("PassportNumber")]
        public string PassportNumber { get; set; }
        [JsonProperty("SaccoPosition")]
        public string SaccoPosition { get; set; }
        [JsonProperty("MembershipType")]
        public string MembershipType { get; set; }
    }
}