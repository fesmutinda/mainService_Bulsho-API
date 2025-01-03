using mainService.memberReg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace mainService.Model
{

    public class BulshoConfig
    {
        public onlineregistration service = new onlineregistration();
        public NetworkCredential credential;

        public BulshoConfig()
        {
            credential = new NetworkCredential(ConfigurationManager.AppSettings["NAVUsername"], ConfigurationManager.AppSettings["NAVPassword"], ConfigurationManager.AppSettings["NAVDomain"]);

            service = new onlineregistration();
            //service.Url = "http://192.168.3.2:7047/BC230/WS/MAFANIKIO%20SACCO/Codeunit/SURESMSSERVICE";
            service.Credentials = credential;
            service.PreAuthenticate = true;
        }
        public string mainLogin(string username, string password)
        { 
            string loggedIn = string.Empty;
            try
            {
                loggedIn = service.FnAccountLogin(username, password);
            }
            catch (Exception ex)
            {
                loggedIn = "";
            }
            return loggedIn;
        }
        public string getShares(string emailAddress, string regNumber)
        { 
            string memberShares = string.Empty;
            try
            {
                memberShares = service.FnShares(emailAddress, regNumber);
            }
            catch (Exception ex)
            {
                memberShares = "";
            }
            return memberShares;
        }
        public string getMemberDetails(string emailAddress, string regNumber)
        { 
            string memberDetails = string.Empty;
            try
            {
                memberDetails = service.FnProfileDetails(emailAddress, regNumber);
            }
            catch (Exception ex)
            {
                memberDetails = "";
            }
            return memberDetails;
        }

        public string getMemberNumber(string email)
        {
            string memberNumber = "";
            try
            {
                memberNumber = service.fnGetMemberNo(email);
            }
            catch (Exception ex)
            {
                memberNumber = "Could not load your Member Number, Please Conduct Bulsho Sacco for Support";
            }
            return memberNumber;
        }

        public string getBULNumber(string email)
        {
            string memberNumber = "";
            try
            {
                memberNumber = service.fnGetBULNumber(email);
            }
            catch (Exception ex)
            {
                memberNumber = "Could not load your Member Number, Please Conduct Bulsho Sacco for Support";
            }
            return memberNumber;
        }
        public string GetSHA1HashData(string data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            string hashedR = BitConverter.ToString(hashData).Replace("-", "");

            return hashedR.ToLower();
        }
    }
}