using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.AccountsModel
{
    public class VirifiPhoneNumberCode
    {
        [Required(ErrorMessage = "الرجاء كتابة الكود الذي تم ارسالة اليك - please fill the code that send to you")]
        public string Code { get; set; }
    }
}
