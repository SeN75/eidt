using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Services.MessageService.Helpers
{
    public interface ISmsService
    {
        Task SendSmsVirifyAsync(string to, string token, char languageToken, string customerName = "");
        Task SendSmsForgetPassword(string to, string username, string password, char languageToken);
    }
}
