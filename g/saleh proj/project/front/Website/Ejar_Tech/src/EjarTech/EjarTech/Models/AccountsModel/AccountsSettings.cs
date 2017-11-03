using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.AccountsModel
{
    public class AccountsSettings
    {
        [Required(ErrorMessage = "الرجاء كتابة الاسم - Pleaser enter your name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال تاريخ الميلاد - Please enter birthday")]
        [DataType(DataType.Date)]
        public string BirthDay { get; set; }
        [Required(ErrorMessage = "الرجاء كتابة البريد الالكتروني - Please enter email address")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
    }
}
