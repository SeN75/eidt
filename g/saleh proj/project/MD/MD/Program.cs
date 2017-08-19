using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;


namespace MD
{
    class Program
    {
       
        // Tash1 is Text encryption
        public string Tash1;
        // the result
        public string TashR;

        static void Main(string[] args)
        {
            string u = Console.ReadLine();
            string p = Console.ReadLine();
            string t = Console.ReadLine();
            string s= Console.ReadLine();
            string m = Console.ReadLine();
            string h = Console.ReadLine();
       
            
        }
        public void Tashfer()
        {
            string t = Tash1;
            MD5 m = MD5.Create();
            byte[] inp = Encoding.ASCII.GetBytes(t);
            byte[] hash = m.ComputeHash(inp);
            t = BitConverter.ToString(hash).Replace("-", "");
            TashR = t;

        }
      
            
        class SendMail
        {
            static string From { get; set; }
            static string PassWord { get; set; }
            static int Port { get; set; }
            static string Host { get; set; }
            static bool Ssl { get; set; }
            
           static void MailTest(string To, string Sub, string Massege)
        {
                MailMessage msg = new MailMessage(From, To, Sub,Massege);
                msg.IsBodyHtml = true;
                SmtpClient sn = new SmtpClient(Host,Port );
                sn.UseDefaultCredentials = false;
                NetworkCredential UserAccount = new NetworkCredential(From, PassWord);
                sn.EnableSsl = Ssl;
                sn.Credentials = UserAccount;
                sn.Send(msg);
            }
            
            }

        
    }
}
