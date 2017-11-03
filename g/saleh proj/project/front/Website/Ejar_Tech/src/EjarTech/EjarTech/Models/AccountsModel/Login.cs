using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.AccountsModel
{
    public class Login
    {
        [Required(ErrorMessage = "يجب كتابة رقم الجوال - you must write phone number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "الرجاء كتابة الجوال بالصيغة الصحيحة  Please Write Phone Number Correctly")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "يجب كتابة كلمة المرور - You must write password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
