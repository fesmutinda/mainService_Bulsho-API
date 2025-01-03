using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace mainService.Models
{
    [DataContract]
    public class Login
    {
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string IdPassport { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string MemberNumber { get; set; }

        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
    [DataContract]
    public class LoginReply
    {
        [DataMember]
        public string apireply { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
    }
    public class regiReply
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string ResponseBody { get; set; }
    }
}