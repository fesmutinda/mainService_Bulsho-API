using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace mainService.Models
{
    [DataContract]
    public class Member
    {
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string RegNumber { get; set; }
        [DataMember]
        public string RequestBody { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
    }
    [DataContract]
    public class MemberReply
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string ResponseBody { get; set; }
        [DataMember]
        public string MemberProfile { get; set; }
    }
    public class UserProfile
    {

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

    [DataContract]
    public class MaternityDetails
    {
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string LMPDate { get; set; }
        [DataMember]
        public string AgeOfPregnancy { get; set; }
        [DataMember]
        public string DueDate { get; set; }
        [DataMember]
        public string ResponseBody { get; set; }
    }
    [DataContract]
    public class MiniStatementItem
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember] 
        public string Amount { get; set; }
    }
}