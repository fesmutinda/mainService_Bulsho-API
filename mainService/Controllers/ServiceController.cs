using mainService.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
//using System.Web.UI.WebControls;

namespace mainService.Controllers
{
    public class ServiceController : ApiController
    {
        public memberReg.onlineregistration thisOnline = new mainService.memberReg.onlineregistration();

        [Route("~/Login")]
        [HttpGet, HttpPost]
        public regiReply myreply(Login mylog)
        {
            string emailAddress = mylog.EmailAddress;
            string password = mylog.Password;
            string reply = "", replyBody=""; 
            string responseServer = string.Empty;
            try
            {
                bool login = false;
                password = GetSHA1HashData(password);

                login = WSConfig.ObjNav.FnLogin(emailAddress.ToLower(), password);
                if (login==true)
                {
                    reply = "success";
                    replyBody = getMemberNumber(emailAddress.ToLower());
                }
                else
                {
                    reply = "sorry";
                    replyBody = "The email Address and Password you provided are not in the active accounts, please confirm your details";
                }
            }
            catch (Exception ex)
            {
                reply = "problems";
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }

        [Route("~/FnAccountLogin")]
        [HttpGet, HttpPost]
        public regiReply FnAccountLogin(Login mylog)
        {
            string username = mylog.Username;
            string password = mylog.Password;
            string reply = "", replyBody = "";
            string responseServer = string.Empty;
            try
            {
                var login = string.Empty; ;
                password = GetSHA1HashData(password);

                login = WSConfig.ObjNav.FnAccountLogin(username, password);
                if (login.Contains("username exists"))
                {
                    //username exisits, get the member number.
                    //reply = "success";
                    //replyBody = getBULNumber(username);

                    reply = "sorry";
                    replyBody = "Please update your App from Playstore to the latest version";
                }
                else if (login.Contains("user exisits"))
                {
                    //email exisits, can create a username.....
                    reply = "success";
                    replyBody = "create username";
                }
                else if (login.Contains("wrong password"))
                {
                    reply = "sorry";
                    replyBody = "The Password you provided is not correct, please provide a valid password";
                }
                else if (login.Contains("wrong username"))
                {
                    reply = "sorry";
                    replyBody = "The Username you provided does not exist, please activate your account.";
                }
                else
                {
                    reply = "sorry";
                    replyBody = "Your request has failed, please contact Bulsho Sacco IT desk ASAP.";
                }
            }
            catch (Exception ex)
            {
                reply = "problems";
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }

        ///gettting the member number
        private string getMemberNumber(string email)
        {
            string memberNumber = "";
            try
            {
                memberNumber = WSConfig.ObjNav.fnGetMemberNo(email);
            }
            catch (Exception ex)
            {
                memberNumber = "Could not load your Member Number, Please Conduct Bulsho Sacco for Support";
            }
            return memberNumber;
        }

        private string getBULNumber(string email)
        {
            string memberNumber = "";
            try
            {
                memberNumber = WSConfig.ObjNav.fnGetBULNumber(email);
            }
            catch (Exception ex)
            {
                memberNumber = "Could not load your Member Number, Please Conduct Bulsho Sacco for Support";
            }
            return memberNumber;
        }

        [Route("~/Registration")]
        [HttpGet, HttpPost]
        public RegisterReply replyReg(Registration detailsClass)
        {
            var typeOfMembership = 0;
            string firstName = string.Empty;
            string midName = string.Empty;
            string lastName = string.Empty;
            string mobileNo = string.Empty;
            string email = string.Empty;
            string country = string.Empty;
            int maritalStatus = 0;
            int gender = 0;
            int employementInfo = 0;
            string readBirth = string.Empty;
            string town = string.Empty;
            string address = string.Empty;
            string name = string.Empty;
            string roCode = "APP-REGISTRATION";

            string KinFullNames = string.Empty;
            string KinMobile = string.Empty;

            DateTime regDate = DateTime.Parse(System.DateTime.Now.ToShortDateString());
            DateTime dateOfBirth = DateTime.MinValue;
            int accountC = 0;

            string reply = string.Empty;
            string replyBody = string.Empty;

            bool regStatus = false;
            string regNumber = string.Empty;
            try
            {
                typeOfMembership = int.Parse(detailsClass.MembershipType);
                firstName = detailsClass.FirstName.ToUpper();
                midName = detailsClass.SecondName.ToUpper();
                lastName = detailsClass.LastName.ToUpper();
                mobileNo = detailsClass.MobileNumber;
                email = detailsClass.EmailAddress;
                country = detailsClass.Country.ToUpper();
                maritalStatus = int.Parse(detailsClass.MaritalStatus);
                readBirth = detailsClass.DateofBirth;
                dateOfBirth = DateTime.Parse(readBirth);
                gender = int.Parse(detailsClass.Gender);
                employementInfo = int.Parse(detailsClass.EmploymentInfo);
                town = detailsClass.Town;
                address = detailsClass.Address;

                name = firstName + " " + midName + " " + lastName;

                accountC = int.Parse(detailsClass.AccountCategory);

                // Now process the uploaded images
                string passportPhotoPath = detailsClass.passportPhoto;
                string signaturePhotoPath = detailsClass.signaturePhoto;

                //Next of Kin-
                KinFullNames = detailsClass.KinFullNames.ToUpper();
                KinMobile = detailsClass.KinMobile;
            }
            catch (Exception ex)
            {
                return new RegisterReply { Response = "Sorry", ResponseBody = "You have Entered information wrongly, please provide all the required data", RegNumber = regNumber };
            }           
            try
            {
                regNumber = WSConfig.ObjNav.MembReg(firstName, midName, lastName, name, roCode, mobileNo, country, town, dateOfBirth, email, gender, maritalStatus, employementInfo, accountC, typeOfMembership);
                try
                {
                    WSConfig.ObjNav.NextOfKin(KinFullNames, KinMobile, regNumber);
                    reply = "Success";
                    replyBody = "Your details have been successfully recieved, your registration Number is " + regNumber;
                }
                catch (Exception ex)
                {
                    reply = "Success";
                    replyBody = "Your details have been successfully recieved, your registration Number is " + regNumber;

                }

            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new RegisterReply { Response = reply, ResponseBody = replyBody, RegNumber = regNumber };
        }


        [Route("~/RegistrationFile")]
        [HttpPost]
        public RegisterReply replySale()
        {
            var httpRequest = HttpContext.Current.Request;

            // Get the other form fields
            string accountType = httpRequest.Form["MembershipType"];
            string firstName = httpRequest.Form["FirstName"];
            string secondName = httpRequest.Form["SecondName"];
            string lastName = httpRequest.Form["LastName"];
            string idNumber = httpRequest.Form["IdNumber"];
            string phoneNumber = httpRequest.Form["MobileNumber"];
            string EmailAddress = httpRequest.Form["EmailAddress"];
            string country = httpRequest.Form["Country"];
            string faceBookName = httpRequest.Form["FacebookName"];
            string whatsappNumber = httpRequest.Form["WhatsappNumber"];
            //int maritalStatus = int.Parse(httpRequest.Form["MaritalStatus"]);
            string readBirth = httpRequest.Form["DateofBirth"];

            // Parse the readBirth date string into a DateTime object
            //DateTime dateOfBirth = DateTime.Parse(readBirth);

            // Handle the image files
            HttpPostedFile passportPhotoFile = httpRequest.Files["passportPhoto"];
            HttpPostedFile signaturePhotoFile = httpRequest.Files["signaturePhoto"];

            // Save the image files to the desired location
            string passportPhotoPath = SaveFile(passportPhotoFile);
            string signaturePhotoPath = SaveFile(signaturePhotoFile);

            // Continue processing the other fields and return the response
            // ...

            return new RegisterReply
            {
                Response = "Success",
                ResponseBody = "Your details have been successfully received"
            };
        }

        [Route("~/Activate-Copy")]
        [HttpGet, HttpPost]
        public regiReply ActivateReply(Login mylog)
        {
            string fullName = mylog.FullName;
            string idPassport = mylog.IdPassport;
            string phoneNumber = mylog.PhoneNumber;
            string emailAddress = mylog.EmailAddress;
            string dateCreated = System.DateTime.Now.ToShortDateString();
            
            string check = string.Empty;
            string response = string.Empty;
            
            string responseServer = string.Empty;

            try
            {
                responseServer = WSConfig.ObjNav.FnActivateAccount(fullName, phoneNumber, emailAddress);
                responseServer = responseServer.ToUpper();
                if (responseServer.Contains("MEMBER NOT FOUND"))
                {
                    check = "invalid";
                    response = "Your Email Address could Not be found, Please Register yourself with Bulsho Sacco First.";
                }
                else if (responseServer.Contains("PENDING"))
                {
                    check = "success";
                    response = "Please Enter the OTP Code sent to your Email Address";
                }
                else
                {
                    string memberNumber = responseServer;

                    check = "success";
                    response = memberNumber;
                } 
            }
            catch (Exception ex)
            {
                check = "Server Error";
                response = "There was a technical problem, please conduct Bulsho Sacco for Assistance.";
            }
            return new regiReply { Response = check, ResponseBody = response };
        }

        [Route("~/Activate")]
        [HttpGet, HttpPost]
        public regiReply ActivationReply(Login mylog)
        {
            string emailAddress = mylog.EmailAddress;
            var memberNumber = mylog.MemberNumber;

            string check = string.Empty;
            string response = string.Empty;

            string responseServer = string.Empty;

            try
            {
                responseServer = WSConfig.ObjNav.fnActivateAccount2(memberNumber, emailAddress.ToLower());
                responseServer = responseServer.ToUpper();
                if (responseServer.Contains("MEMBER NOT FOUND"))
                {
                    check = "invalid";
                    response = "Your Email Address could Not be found, Please Register yourself with Bulsho Sacco First.";
                }
                else if (responseServer.Contains("PENDING"))
                {
                    check = "success";
                    response = "Please Enter the OTP Code sent to your Email Address";
                }
                else
                {
                    string memberNumberRes = responseServer;

                    check = "success";
                    response = memberNumberRes;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The record in table PasswordManager already exists"))
                {
                    check = "invalid";
                    response = "Your Account seems already Activated for Application. Please liase with Bulsho ICT team if you have any challenges.";
                }
                else
                {
                    check = "Server Error";
                    response = "There was a technical problem, please conduct Bulsho Sacco for Assistance.";
                }
            }
            return new regiReply { Response = check, ResponseBody = response };
        }


        [Route("~/MemberDetails")]
        [HttpGet, HttpPost]
        public MemberReply reply(Member memberRequest)
        {
            string regNumber = memberRequest.RegNumber;
            string emailAddress = memberRequest.EmailAddress;
            string jsonBody = string.Empty;
            string responseBody = string.Empty;
            string response = string.Empty;
            try
            {              

                try
                {
                    //string responseServer = WSConfig.ObjNav.FnProfileDetails("maryanitaa0@gmail.com", "BUL25210003");
                    string responseServer = WSConfig.ObjNav.FnProfileDetails(emailAddress, regNumber);
                    responseServer = responseServer.ToUpper();
                    if (responseServer.Contains("NO"))
                    {
                        response = "invalid";
                        responseBody = "Your Email Address could Not be found, Please Register yourself with Bulsho Sacco First.";
                    }
                    else
                    {
                        response = "success";
                        responseBody = "Please Enter the OTP Code sent to your Email Address";

                        UserProfile memberInfo = new UserProfile();
                        //string myArray = "BUL25210003:::MARYAN HUSSEIN ISMAIL:::::::::0610996385:::maryanitaa0@gmail.com:::Staff:::An-Nisa(Women)";
                        string[] array = responseServer.Split(new string[1]
                        {
                        ":::"
                        }, StringSplitOptions.None);
                        memberInfo.MemberNumber = array[0];
                        memberInfo.FullName = array[1];
                        memberInfo.IDNumber = array[2];
                        memberInfo.PassportNumber = array[3];
                        memberInfo.PhoneNumber = array[4];
                        memberInfo.EmailAddress = array[5];
                        memberInfo.SaccoPosition = array[6];
                        memberInfo.MembershipType = array[7];

                        jsonBody = JsonConvert.SerializeObject(memberInfo);

                        response = "success";
                        responseBody = "Successfully obtained member detailed.";
                    }
                }
                catch (Exception ex)
                {
                    response = "sorry";
                    responseBody = "There was a problem reading your profile details, Please Conduct Bulsho Sacco Offices.";
                }
            }
            catch (Exception ex)
            {
                response = "sorry";
                responseBody = ex.Message + ".";
            }

            return new MemberReply { ResponseBody = responseBody, MemberProfile = jsonBody, Response = response };
        }

        [Route("~/EmailCode")]
        [HttpGet, HttpPost]
        public regiReply replymail(Registration memberSale)
        {
            string emailAddress = memberSale.EmailAddress;
            int otpCode = memberSale.OtpCode;

            string responseServer = "";
            string reply = string.Empty;
            string replyBody = string.Empty;

            try
            {
                responseServer = WSConfig.ObjNav.FnVerifyOTP(otpCode, emailAddress);
                responseServer = responseServer.ToUpper();
                if (responseServer.Contains("OTP VERIFIED SUCCESSFULLY"))
                {
                    reply = "success";
                    replyBody = responseServer;
                }
                else
                {
                    reply = "sorry";
                    replyBody = responseServer;
                }
            }
            catch (Exception ex)
            {
                reply = "Server Error";
                replyBody = "There was a technical problem, please conduct Bulsho Sacco for Assistance.";
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }

        [Route("~/Password")]
        [HttpGet, HttpPost]
        public regiReply replypassword(Registration memberSale)
        {
            string emailAddress = memberSale.EmailAddress;
            string password = memberSale.Password;
            
            string reply = string.Empty;
            string replyBody = string.Empty;
            string responseServer = "";

            try
            {
                password = GetSHA1HashData(memberSale.Password);

                responseServer = WSConfig.ObjNav.FnUpdatePassword(emailAddress, password);
                //responseServer = WSConfig.ObjNav.FnUpdatePassword2(emailAddress, password);
                responseServer = responseServer.ToUpper();
                if (responseServer.Contains("PASSWORD CREATED SUCCESSFULLY"))
                {
                    reply = "success";
                    replyBody = responseServer;
                }
                else
                {
                    reply = "sorry";
                    replyBody = responseServer;
                }
            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }

        [Route("~/FnCreateAccount")]
        [HttpGet, HttpPost]
        public regiReply FnCreateAccount(Registration memberSale)
        {
            var username = memberSale.Username;
            var emailAddress = memberSale.EmailAddress;
            var password = memberSale.Password;

            var reply = string.Empty;
            var replyBody = string.Empty;
            var responseServer = "";

            try
            {
                password = GetSHA1HashData(memberSale.Password);
                responseServer = WSConfig.ObjNav.FnCreateAccount(emailAddress, username, password);

                if (responseServer.Contains("Username Exisits"))
                {
                    reply = "sorry";
                    replyBody = "That username is Already used by another Member, Please create your unique username";
                }
                else if (responseServer.Contains("Password created successfully"))
                {
                    reply = "success";
                    replyBody = responseServer;
                }
                else
                {
                    reply = "sorry";
                    replyBody = responseServer;
                }
                                
            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }


        [Route("~/FnCreateUsername")]
        [HttpGet, HttpPost]
        public regiReply FnCreateUsername(Registration memberSale)
        {
            var username = memberSale.Username;
            var emailAddress = memberSale.EmailAddress;

            var reply = string.Empty;
            var replyBody = string.Empty;
            var responseServer = "";

            try
            {
                responseServer = WSConfig.ObjNav.FnCreateUsername(emailAddress, username);

                if (responseServer.Contains("success"))
                {
                    reply = "success";
                    replyBody = "You have successfully created your username, Please proceed to Login";
                }
                else if (responseServer.Contains("Username Exisits"))
                {
                    reply = "sorry";
                    replyBody = "That username is Already used by another Member, Please create your unique username";
                }
                else if (responseServer.Contains("invalid"))
                {
                    reply = "sorry";
                    replyBody = "Your email Address could not be Found, Please activate your account";
                }
                else
                {
                    reply = "sorry";
                    replyBody = responseServer;
                }

            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }
        
        [Route("~/ShareCapital")]
        [HttpGet, HttpPost]
        public regiReply replyShares(Member memberSale)
        {
            string emailAddress = memberSale.EmailAddress;
            string regNumber = memberSale.RegNumber;


            string reply = string.Empty;
            string replyBody = string.Empty;
            string responseServer = "";

            try
            {
                //responseServer = WSConfig.ObjNav.FnShares("maryanitaa0@gmail.com", "BUL25210003");
                responseServer = WSConfig.ObjNav.FnShares(emailAddress, regNumber);
                responseServer = responseServer.ToUpper();
                if (responseServer.Equals(""))
                {
                    reply = "sorry";                    
                    replyBody = responseServer;
                }
                else
                {
                    reply = "success";
                    replyBody = responseServer;
                }
            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }

        [Route("~/Ministatements")]
        [HttpGet, HttpPost]
        public regiReply replyStatements(Member memberSale)
        {
            string emailAddress = memberSale.EmailAddress;
            string regNumber = memberSale.RegNumber;


            string reply = string.Empty;
            string replyBody = string.Empty;
            string responseServer = "";

            try
            {
                //responseServer = WSConfig.ObjNav.FnMemberStatement("maryanitaa0@gmail.com", "BUL25210003");
                responseServer = WSConfig.ObjNav.FnMemberStatement(emailAddress, regNumber);
                //responseServer = responseServer.ToUpper();
                if (responseServer.Equals("Member not found"))
                {
                    reply = "sorry";
                    replyBody = responseServer;
                }

                else if (responseServer.Equals("No transactions were found"))
                {
                    reply = "sorry";
                    replyBody += responseServer;
                }
                else
                {
                    reply = "success";
                    List<string> list = new List<string>();
                    List<MiniStatementItem> statementItems = new List<MiniStatementItem>();

                    string[] source = responseServer.Split(new string[1]
                    {
                        ";"
                    }, StringSplitOptions.RemoveEmptyEntries);
                    list = source.ToList();
                    foreach (string item in list)
                    {
                        string[] array = item.Split(new string[1]
                        {
                            "|"
                        }, StringSplitOptions.None);
                        statementItems.Add(new MiniStatementItem
                        {
                            Date = array[0],
                            Description = array[1],
                            Amount = array[2]
                        });
                    }

                    string jsonString = JsonConvert.SerializeObject(statementItems);
                    replyBody = jsonString;// statementItems.ToString();
                }
            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }

        [Route("~/FnSendOtp")]
        [HttpGet, HttpPost]
        public regiReply replysendOtp(Member memberSale)
        {
            string emailAddress = memberSale.EmailAddress;
            string regNumber = memberSale.RegNumber;


            string reply = string.Empty;
            string replyBody = string.Empty;
            string responseServer = "";

            try
            {
                responseServer = WSConfig.ObjNav.FnChangeOTP(emailAddress);
                responseServer = responseServer.ToUpper();
                if (responseServer.Equals(""))
                {
                    reply = "sorry";
                    replyBody = responseServer;
                }
                else
                {
                    reply = "success";
                    replyBody = responseServer;
                }
            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }


        [Route("~/ResetPassword")]
        [HttpGet, HttpPost]
        public regiReply replyPasswordReset(Registration memberReset)
        {
            var emailAddress = memberReset.EmailAddress;
            var accountNumber = getMemberNumber(emailAddress); ;
            var password = memberReset.Password;
            var confirmPassword = memberReset.ConfirmPassword;
            var otpCode = memberReset.OtpCode;

            string reply = string.Empty;
            string replyBody = string.Empty;
            string responseServer = "";

            try
            {
                password = GetSHA1HashData(password);
                confirmPassword = GetSHA1HashData(confirmPassword);

                responseServer = WSConfig.ObjNav.FnResetPassword(accountNumber, password, confirmPassword, otpCode);
                //WSConfig.ObjNav.
                responseServer = responseServer.ToUpper();
                if (responseServer.Equals(""))
                {
                    reply = "sorry";
                    replyBody = "Please Enter a valid OTP code";
                }
                else
                {
                    reply = "success";
                    replyBody = responseServer;
                }
            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }


        [Route("~/ChangeRequest")]
        [HttpGet, HttpPost]
        public regiReply replyRequest(Member memberSale)
        {
            string emailAddress = memberSale.EmailAddress;
            string request = memberSale.RequestBody;


            string reply = string.Empty;
            string replyBody = string.Empty;
            string responseServer = "";

            try
            {
                //responseServer = WSConfig.ObjNav.FnShares("maryanitaa0@gmail.com", "BUL25210003");
                responseServer = WSConfig.ObjNav.FnChangeRequest(emailAddress, request);
                responseServer = responseServer.ToUpper();
                if (responseServer.Equals(""))
                {
                    reply = "sorry";
                    replyBody = responseServer;
                }
                else
                {
                    reply = "success";
                    replyBody = responseServer;
                }
            }
            catch (Exception ex)
            {
                reply = "Error";
                replyBody = ex.Message;
            }

            return new regiReply { Response = reply, ResponseBody = replyBody };
        }

        // Method to save the uploaded file and return the file path
        private string SaveFile(HttpPostedFile file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string uploadsFolder = HostingEnvironment.MapPath("~/Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string filePath = Path.Combine(uploadsFolder, fileName);
                file.SaveAs(filePath);
                return filePath;
            }

            return string.Empty;
        }

        private byte[] ReadImageAsByteArrayMain(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                return buffer;
            }
        }

        private byte[] ReadImageAsByteArray(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    return buffer;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or print the error message for debugging purposes
                Console.WriteLine("Error reading the image file: " + ex.Message);
                return null; // Return null or an empty array to indicate failure
            }
        }

        private string GetSHA1HashData(string data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            string hashedR = BitConverter.ToString(hashData).Replace("-", "");

            return hashedR.ToLower();
        }
    }
}
