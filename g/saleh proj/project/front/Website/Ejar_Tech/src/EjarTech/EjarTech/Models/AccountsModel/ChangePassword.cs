using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.AccountsModel
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "الرجاء كتابة كلمة المرور الحالية - Please enter current password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8 , ErrorMessage = "طول كلمة المرور من 8 الى 20 خانة - Password length must be 8 to 20 field")]
        public string OldOne { get; set; }
        [Required(ErrorMessage = "الرجاء كتابة كلمة المرور الجديدة - please enter new password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "طول كلمة الجديدة المرور من 8 الى 20 خانة - New Password length must be 8 to 20 field")]
        public string NewOne { get; set; }
        [Required(ErrorMessage = "الرجاء كتابة اعادة الباسوورد - Please enter re-password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "طول اعادة كلمة المرور من 8 الى 20 خانة - Re Password length must be 8 to 20 field")]
        public string ReNewOne { get; set; }
    }
}
