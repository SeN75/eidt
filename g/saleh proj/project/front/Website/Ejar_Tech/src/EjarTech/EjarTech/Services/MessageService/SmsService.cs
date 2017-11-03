using EjarTech.Models.ConfigurationModel;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using EjarTech.Services.MessageService.Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EjarTech.Services.MessageService
{
    public class SmsService : ISmsService
    {
        private readonly SmsOption _options;
        private readonly IDatabaseConnection _database;

        public SmsService(IOptions<SmsOption> smsOptions, IDatabaseConnection database)
        {
            _options = smsOptions.Value;
            _database = database;
        }

        public async Task SendSmsForgetPassword(string to, string username, string password, char languageToken)
        {
            string message;
            if (to.Length == 9)
                to = $"966{to}";
            if (!ContainsArabic(username))
                message = $"Hello {username}, your new password is: {password}";
            else
                message = ToArabicFormat($"Hello {username}, your new password is: {password}");
            string url = "http://www.sms.malath.net.sa/httpSmsProvider.aspx";
            string data = $"username={_options.UserName}&password={_options.Password}&mobile={to}&unicode={languageToken}&message={message}&sender={_options.Sender}";
            HttpWebRequest httpwr = (HttpWebRequest)WebRequest.Create(url);
            httpwr.Method = "POST";
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            httpwr.ContentType = "application/x-www-form-urlencoded";
            //set data to stream
            Stream reqStream = await httpwr.GetRequestStreamAsync();
            await reqStream.WriteAsync(byteData, 0, byteData.Length);

            WebResponse httpres = await httpwr.GetResponseAsync();
            Stream resStream = httpres.GetResponseStream();
            string codeResponse;
            using (StreamReader sReader = new StreamReader(resStream, Encoding.UTF8))
                codeResponse = await sReader.ReadLineAsync();
            Debug.Write(codeResponse);
        }

        public async Task SendSmsVirifyAsync(string to, string token, char languageToken, string customerName = "")
        {
            string message;
            if (ContainsArabic(customerName))
                message = ToArabicFormat($"Hello {customerName} your virify code is : {token}");
            else
                message = $"Hello {customerName} your virify code is : {token}";
            if (to.Length == 9)
                to = $"966{to}";
            string url = "http://www.sms.malath.net.sa/httpSmsProvider.aspx";
            string data = $"username={_options.UserName}&password={_options.Password}&mobile={to}&unicode={languageToken}&message={message}&sender={_options.Sender}";

            HttpWebRequest httpwr = (HttpWebRequest)WebRequest.Create(url);
            httpwr.Method = "POST";
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            httpwr.ContentType = "application/x-www-form-urlencoded";
            //set data to stream
            Stream reqStream = await httpwr.GetRequestStreamAsync();
            await reqStream.WriteAsync(byteData, 0, byteData.Length);

            WebResponse httpres = await httpwr.GetResponseAsync();
            Stream resStream = httpres.GetResponseStream();
            string codeResponse;
            using (StreamReader sReader = new StreamReader(resStream,Encoding.UTF8))
                codeResponse = await sReader.ReadLineAsync();
            Debug.Write(codeResponse);
        }

        private string ToArabicFormat(string input)
        {
            string arabicmessage = string.Empty;
            //Get HEX
            string ToHexaDecimal(string entry)
            {
                string temp = string.Empty;

                switch (entry.Length)
                {
                    case 1:
                        temp = "000" + entry;
                        break;
                    case 2:
                        temp = "00" + entry;
                        break;
                    case 3:
                        temp = "0" + entry;
                        break;
                    case 4:
                        temp = entry;
                        break;
                }

                return temp;
            }
            //ToChar
            string ToChar(char ch)
            {
                System.Text.UnicodeEncoding class1 = new System.Text.UnicodeEncoding();
                byte[] msg = class1.GetBytes(System.Convert.ToString(ch));

                return ToHexaDecimal(msg[1] + msg[0].ToString("X"));
            }
            for (int i = 0; i < input.Length; i++)
            {
                arabicmessage += ToChar(Convert.ToChar(input.Substring(i, 1)));
            }

            return arabicmessage;
        }

        private bool IsArabicLetter(char character)
        {
            if (character >= 0x600 && character <= 0x6ff) return true;
            if (character >= 0x750 && character <= 0x77f) return true;
            if (character >= 0xfb50 && character <= 0xfc3f) return true;
            if (character >= 0xfe70 && character <= 0xfefc) return true;
            return false;
        }

        private bool ContainsArabic(string txt)
        {
            char[] charArray = txt.ToCharArray();
            foreach (char character in charArray)
            {

                if (IsArabicLetter(character)) return true;
                //if (character >= 0x600 && character <= 0x6ff) return true;
                //if (character >= 0x750 && character <= 0x77f) return true;
                //if (character >= 0xfb50 && character <= 0xfc3f) return true;
                //if (character >= 0xfe70 && character <= 0xfefc) return true;
            }
            return false;
        }
    }
}
