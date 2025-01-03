using mainService.memberReg;
using mainService.pictureReg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace mainService.Models
{
    public class WSConfig
    {
        public static onlineregistration ObjNav
        {
            get
            {
                onlineregistration regCodeunit = new onlineregistration();
                try
                {
                    //ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                    NetworkCredential credential = new NetworkCredential(ConfigurationManager.AppSettings["NAVUsername"], ConfigurationManager.AppSettings["NAVPassword"], ConfigurationManager.AppSettings["NAVDomain"]);
                    regCodeunit.Credentials = credential;
                    regCodeunit.PreAuthenticate = true;
                }
                catch (Exception exception)
                {
                    exception.Data.Clear();
                }
                return regCodeunit;
            }
        }
        public static MemberPicture_Service sendPicture
        {
            get
            {
                pictureReg.MemberPicture_Service picReg = new pictureReg.MemberPicture_Service();
                try
                {
                    NetworkCredential credential = new NetworkCredential(ConfigurationManager.AppSettings["NAVUsername"], ConfigurationManager.AppSettings["NAVPassword"], ConfigurationManager.AppSettings["NAVDomain"]);
                    picReg.Credentials = credential;
                    picReg.PreAuthenticate = true;
                }
                catch (Exception exception)
                {
                    exception.Data.Clear();
                }
                return picReg;
            }
        }
        public static signatureReg.MemberSignature_Service sendSignature
        {
            get
            {
                signatureReg.MemberSignature_Service picReg = new signatureReg.MemberSignature_Service();
                try
                {
                    NetworkCredential credential = new NetworkCredential(ConfigurationManager.AppSettings["NAVUsername"], ConfigurationManager.AppSettings["NAVPassword"], ConfigurationManager.AppSettings["NAVDomain"]);
                    picReg.Credentials = credential;
                    picReg.PreAuthenticate = true;
                }
                catch (Exception exception)
                {
                    exception.Data.Clear();
                }
                return picReg;
            }
        }

        public static bool sendMailNotification(string recipientEmail, string smsmessage, string subject)
        {
            bool emailSent = false;
            string senderEmail = "info@bulshosacco.com";
            string senderPassword = "D0Rtu.M%)g{W";

            // Create a new MailMessage object
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = subject;
            mail.Body = smsmessage;

            // Create a new SmtpClient object
            SmtpClient smtpClient = new SmtpClient("mail.bulshosacco.com", 465); // Gmail SMTP server and port

            // Provide credentials for the sender's email
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true; // Enable SSL encryption

            try
            {
                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
                emailSent = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email. Error message: " + ex.Message);
            }

            return emailSent;
        }
        private string sendNotification(string recipientEmail)
        {
            // Generate a 5 digit code
            int _min1 = 100000000;
            int _max1 = 999999999;
            Random _rdm = new Random();
            int code = (int)(_rdm.Next(_min1, _max1) * 90000) + 12345;

            if ((code.ToString().Length > 5))
            {
                code = (int)((_rdm.Next(_min1, _max1) * 90000));
            }

            string Sendcode = (code.ToString().Substring(1, 5).ToString());
            string smsmessage = "Your one time Verification Code for HeartBeat is " + Sendcode;

            string senderEmail = "snashyconnect@gmail.com";
            string senderPassword = "fvbojvtghkcrflzo";

            // Create a new MailMessage object
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = "Heartbeat Email Verification"; // Email subject
            mail.Body = smsmessage;

            // Create a new SmtpClient object
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // Gmail SMTP server and port

            // Provide credentials for the sender's email
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true; // Enable SSL encryption

            try
            {
                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email. Error message: " + ex.Message);
            }

            return Sendcode;
        }
        public static string otpCode()
        {
            int _min1 = 100000000;
            int _max1 = 999999999;
            Random _rdm = new Random();
            int code = (int)(_rdm.Next(_min1, _max1) * 90000) + 12345;

            if ((code.ToString().Length > 5))
            {
                code = (int)((_rdm.Next(_min1, _max1) * 90000));
            }

            return code.ToString().Substring(1, 5).ToString();
        }
    }
}