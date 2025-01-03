
using mainService.memberReg;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace mainService.Model
{
    public static class Utils
    {

        public static onlineregistration ProxyService = new onlineregistration();
        public static string logpath = @"C:\Server\Bulsho\api\Logs";

        public static string LogFileName
        {
            get
            {
                if (!Directory.Exists(logpath))
                    Directory.CreateDirectory(logpath);
                return String.Format("{0}{1}{2}{3}.txt", logpath, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
        }


        public static void LogEntryOnFile(string clientRequest)
        {
            File.AppendAllText(LogFileName, String.Format("{0}: {1}\n", DateTime.Now, clientRequest));

        }
        public static string numbers()
        {
            List<long> keys = new List<long>();
            Random ran = new Random(DateTime.Now.Millisecond);

            int keyPart1 = 0;
            int keyPart2 = 0;
            int keyPart3 = 0;
            int keyPart4 = 0;
            int keyPart5 = 0;
            long result = 0;
            do
            {
                keyPart1 = ran.Next(10000, 99999);

                keyPart2 = ran.Next(10000, 99999);

                keyPart3 = ran.Next(10000, 99999);

                keyPart4 = ran.Next(10000, 99999);

                keyPart5 = ran.Next(10000, 99999);
                result = long.Parse((keyPart1.ToString()).Substring(0, 1) + keyPart2.ToString().Substring(0, 1)
                    + keyPart3.ToString().Substring(0, 1) + keyPart4.ToString().Substring(0, 1)
                    + keyPart5.ToString().Substring(0, 1));
            }
            while (keys.Contains(result));
            keys.Add(result);

            return result.ToString();
        }

        public static string gnrPin()
        {
            List<long> keys = new List<long>();
            Random ran = new Random(DateTime.Now.Millisecond);

            int keyPart1 = 0;
            int keyPart2 = 0;
            int keyPart3 = 0;
            int keyPart4 = 0;
            long result = 0;
            do
            {
                keyPart1 = ran.Next(10000, 99999);

                keyPart2 = ran.Next(10000, 99999);

                keyPart3 = ran.Next(10000, 99999);

                keyPart4 = ran.Next(10000, 99999);

                result = long.Parse((keyPart1.ToString()).Substring(0, 1) + keyPart2.ToString().Substring(0, 1)
                    + keyPart3.ToString().Substring(0, 1) + keyPart4.ToString().Substring(0, 1));
            }
            while (keys.Contains(result));
            keys.Add(result);

            return result.ToString();
        }

        public static string HexString2B64String(this string input)
        {
            return Convert.ToBase64String(HexStringToHex(input));
        }
        public static byte[] HexStringToHex(string inputHex)
        {
            var resultantArray = new byte[inputHex.Length / 2];
            for (var i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = System.Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }
        public static string GenerateOTP()
        {

            string sOTP = String.Empty;

            string sTempChars = String.Empty;
            int iOTPLength = 4;
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;
        }
    }
}