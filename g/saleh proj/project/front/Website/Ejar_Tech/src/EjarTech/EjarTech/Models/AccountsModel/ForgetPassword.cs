using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.AccountsModel
{
    public class ForgetPassword
    {
        [Required(ErrorMessage = "Please Enter Phone Number - الرجاء إدخال رقم الجوال")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Phone nuber must be 12 field - رقم الجوال يتكون من 12 خانة")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please Enter EMail - الرجاء إدخال الإيميل")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
    }
}
