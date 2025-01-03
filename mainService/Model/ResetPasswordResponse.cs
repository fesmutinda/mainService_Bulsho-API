using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace mainService.Model
{
    [DataContract]
    public class ResetPasswordResponse
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string ResponseBody { get; set; }

    }
}