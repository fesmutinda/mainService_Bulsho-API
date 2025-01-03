using mainService.Model;
using mainService.Models;
using Newtonsoft.Json;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace mainService.Controllers
{
    public class BulshoController : ApiController
    {
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/Mobile/Login")]
        public async Task<HttpResponseMessage> PostLogin([FromBody] LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            HttpResponseMessage jsonresponse = Request.CreateResponse();

            // Validate the request object and its properties
            if (request == null)
            {
                response.Response = "error";
                response.ResponseBody = "Request cannot be null";
                jsonresponse.Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
                return jsonresponse;
            }
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                response.Response = "error";
                response.ResponseBody = "Username cannot be null or empty";
                jsonresponse.Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
                return jsonresponse;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                response.Response = "error";
                response.ResponseBody = "Password cannot be null or empty";
                jsonresponse.Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
                return jsonresponse;
            }

            var username = request.Username;
            var password = request.Password;
            var membernumber = string.Empty;
            response.MemberData = new MemberData();
            try
            {
                var login = string.Empty;
                password = new BulshoConfig().GetSHA1HashData(password);

                login = new BulshoConfig().mainLogin(username, password);

                if (login.Contains("username exists"))
                {
                    //username exisits, get the member number.
                    response.Response = "success";
                    response.ResponseBody = new BulshoConfig().getBULNumber(username);
                    if (!string.IsNullOrEmpty(response.ResponseBody))
                    {
                        membernumber = response.ResponseBody;
                        //get the shares...
                        response.MemberData.MemberShares = new BulshoConfig().getShares(username, membernumber);
                        //get member details...
                        var memberData = new BulshoConfig().getMemberDetails(username, response.ResponseBody);
                        string[] array = memberData.Split(new string[1]
                        {
                        ":::"
                        }, StringSplitOptions.None);
                        response.MemberData.MemberNumber = array[0];
                        response.MemberData.FullName = array[1];
                        response.MemberData.IDNumber = array[2];
                        response.MemberData.PassportNumber = array[3];
                        response.MemberData.PhoneNumber = array[4];
                        response.MemberData.EmailAddress = array[5];
                        response.MemberData.SaccoPosition = array[6];
                        response.MemberData.MembershipType = array[7];
                    }
                }
                else if (login.Contains("user exisits"))
                {
                    //email exisits, can create a username.....
                    response.Response = "success";
                    response.ResponseBody = "create username";
                }
                else if (login.Contains("wrong password"))
                {
                    response.Response = "sorry";
                    response.ResponseBody = "The Password you provided is not correct, please provide a valid password";
                }
                else if (login.Contains("wrong username"))
                {
                    response.Response = "sorry";
                    response.ResponseBody = "The Username you provided does not exist, please activate your account.";
                }
                else
                {
                    response.Response = "sorry";
                    response.ResponseBody = "Your request has failed, please contact Bulsho Sacco IT desk ASAP.";
                }
            }
            catch (Exception ex)
            {
                response.Response = "problems";
                response.ResponseBody = ex.Message;

                Utils.LogEntryOnFile(ex.Message);
                if (ex.InnerException != null)
                    Utils.LogEntryOnFile(ex.InnerException.Message);
                Utils.LogEntryOnFile(ex.StackTrace);
            }

            jsonresponse.Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
            return jsonresponse;
        }


        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/Mobile/CheckVersion")]
        public async Task<HttpResponseMessage> Post([FromBody] versionModelRequest versionModelRequest)
        {
            HttpResponseMessage jsonresponse = Request.CreateResponse();
            versionModelResponse responseContent = null;
            string versionNo = string.Empty;
            string sessionId = string.Empty;
            string Token = string.Empty;
            string versionResponse = null;

           
            if (versionModelRequest != null)
            {
                string clientCode = versionModelRequest.clientCode;
                if (clientCode != null)
                {
                    try
                    {
                        if (clientCode == "BULSHO")
                        {
                            versionNo = "6.6";
                            if (versionModelRequest.versionNumber == versionNo)
                            {
                                responseContent = new versionModelResponse
                                {
                                    status = true,
                                    Description = "Latest version installed"
                                };
                                jsonresponse.StatusCode = HttpStatusCode.OK;
                                versionResponse = JsonConvert.SerializeObject(responseContent);
                            }
                            else
                            {
                                responseContent = new versionModelResponse
                                {
                                    status = false,
                                    Description = "Install the latest version to continue."
                                };
                                jsonresponse.StatusCode = HttpStatusCode.Unauthorized;
                                versionResponse = JsonConvert.SerializeObject(responseContent);
                            }
                        }
                        else
                        {
                            SuccessResponse successResponse = new SuccessResponse()
                            {
                                status_code = "409",
                                Description = "Your request seems to be from an invalid device, please get a verified application and try again"
                            };
                            jsonresponse.StatusCode = HttpStatusCode.Conflict;
                            versionResponse = JsonConvert.SerializeObject(successResponse);
                        }
                    }
                    catch (Exception ex)
                    {
                        SuccessResponse successResponse = new SuccessResponse()
                        {
                            status_code = "502",
                            Description = ex.Message
                        };
                        jsonresponse.StatusCode = HttpStatusCode.BadGateway;
                        versionResponse = JsonConvert.SerializeObject(successResponse);
                    }
                }
                else
                {
                    SuccessResponse successResponse = new SuccessResponse()
                    {
                        status_code = "409",
                        Description = "An error occurred"
                    };
                    jsonresponse.StatusCode = HttpStatusCode.Conflict;
                    versionResponse = JsonConvert.SerializeObject(successResponse);
                }
            }
            else
            {
                SuccessResponse successResponse = new SuccessResponse()
                {
                    status_code = "400",
                    Description = "Bad Request"
                };
                jsonresponse.StatusCode = HttpStatusCode.BadRequest;
                versionResponse = JsonConvert.SerializeObject(successResponse);
            }


            jsonresponse.Content = new StringContent(versionResponse, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = jsonresponse;
            return httpResponseMessage;
        }

        [ResponseType(typeof(ResetPasswordResponse))]
        [HttpPost]
        [Route("api/Mobile/ResetPassword")]
        public async Task<HttpResponseMessage> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new ResetPasswordResponse();
            HttpResponseMessage jsonresponse = Request.CreateResponse();

            var emailAddress = request.EmailAddress;
            var accountNumber =new BulshoConfig().getMemberNumber(emailAddress);
            var password = request.Password;
            var confirmPassword = request.ConfirmPassword;
            var otpCode = request.OtpCode;

            string reply = string.Empty;
            string replyBody = string.Empty;
            string responseServer = "";

            try
            {
                password = new BulshoConfig().GetSHA1HashData(password);
                confirmPassword = new BulshoConfig().GetSHA1HashData(confirmPassword);

                responseServer = WSConfig.ObjNav.FnResetPassword(accountNumber, password, confirmPassword, otpCode);
                //WSConfig.ObjNav.
                responseServer = responseServer.ToUpper();
                if (responseServer.Equals(""))
                {
                    response.Response = "sorry";
                    replyBody = "Please Enter a valid OTP code";
                }
                else
                {
                    response.Response = "success";
                    replyBody = responseServer;
                }
            }
            catch (Exception ex)
            {
                response.Response = "Error";
                response.ResponseBody = ex.Message;
                Utils.LogEntryOnFile(ex.Message);
                if (ex.InnerException != null)
                    Utils.LogEntryOnFile(ex.InnerException.Message);
                Utils.LogEntryOnFile(ex.StackTrace);
            }

            jsonresponse.Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
            return jsonresponse;
        }


    }
}
