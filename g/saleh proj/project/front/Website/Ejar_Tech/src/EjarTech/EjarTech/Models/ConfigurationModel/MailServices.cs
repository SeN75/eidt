using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;

namespace EjarTech.Models.ConfigurationModel
{
    public class MailServices
    {
        public string From { get; set; }
        public string Password { get; set; }
        public string GmailName { get; set; }
        public string Smtp  { get; set; }
        public int SmtpPort { get; set; }
        public bool Ssl { get; set; }

        public async Task SendVirifyMailAsync(string to, string subject, string token)
        {

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ejar Tech", From));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Virify Your Email";
            message.Body = new TextPart("plain")
            {
                Text = $"Click This Url {token}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect(Smtp, SmtpPort, Ssl);
                client.Authenticate(GmailName, Password); 
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
