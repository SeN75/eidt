using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.AccountsModel
{
    public class ChangePhoneNumber
    {
        [Required(ErrorMessage = "Please Enter New Phone Number - الرجاء كتابة رقم الهاتف الجديد")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "الرجاء كتابة الجوال بالصيغة الصحيحة  Please Write Phone Number Correctly")]
        [DataType(DataType.PhoneNumber)]
        public string NewNumber { get; set; }
    }
}
