using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace mainService.Models
{
    [DataContract]
    public class Registration
    {
        [DataMember]
        public string MembershipType { get; set; }
        [DataMember]
        public string AccountCategory { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string SecondName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string SecondaryMobileNumber { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string IdNumber { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string WhatsappNumber { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string DateofBirth { get; set; }        
        [DataMember]
        public string EmploymentInfo { get; set; }
        [DataMember]
        public string EmployerName { get; set; }
        [DataMember]
        public string EmployerAddress { get; set; }
        [DataMember]
        public string FacebookName { get; set; }
        [DataMember]
        public string ExpectedMonthlyIncomeAmount { get; set; }
        [DataMember]
        public string KinFullNames { get; set; }
        [DataMember]
        public string KinSecondName { get; set; }
        [DataMember]
        public string KinIdNumber { get; set; }
        [DataMember]
        public string KinMobile { get; set; }
        [DataMember]
        public string KinEmail { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string BankBranch { get; set; }
        [DataMember]
        public string BankAccountNumber { get; set; }
        [DataMember]
        public string MaritalStatus { get; set; }
        [DataMember]
        public string Town { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string passportPhoto { get; set; }
        [DataMember]
        public string signaturePhoto { get; set; }
        [DataMember]
        public string passportFileName { get; set; }
        [DataMember]
        public string signatureFileName { get; set; }
        [DataMember]
        public int OtpCode { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string ConfirmPassword { get; set; }

    }

    [DataContract]
    public class RegisterReply
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string RegNumber { get; set; }
        [DataMember]
        public string ResponseBody { get; set; }
    }
}