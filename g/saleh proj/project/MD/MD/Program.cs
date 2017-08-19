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
            private string From { get; set; }
            private  string PassWord { get; set; }
            private string TO { get; set; }
            private string Sub { get; set; }
            private string Massege { get; set; }
            private int Port { get; set; }
            private string Host { get; set; }
            private bool Ssl { get; set; }

            public SendMail(string umail, string password, string der,string sub,string massege ,int port , string host , bool ssl)
            {
                this.From = umail;
                this.PassWord = password;
                this.TO = der;
                this.Sub = sub;
                this.Massege = massege;
                this.Port = port;
                this.Host = host;
                    
            }
            public void MailTest()
            {
                MailMessage msg = new MailMessage(From, TO, Sub,Massege);
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
