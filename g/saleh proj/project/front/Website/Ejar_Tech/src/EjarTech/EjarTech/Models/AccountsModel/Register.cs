using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace EjarTech.Models.AccountsModel
{
    public class Register
    {
        [Required(ErrorMessage = "يجب عليك ادخل الاسم - You must enter your name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "يجب عليك ادخل الاسم - You must enter your name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "يجب عليك ادخال كلمة المرور - You Must enter your name")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "كلمة المرور لابد ان تكون من 8 الى 20 خانة - Password must be between 8 to 20 field", MinimumLength = 8)]
        public string Password { get; set; }
        [Required(ErrorMessage = "يجب عليك ادخال إعادة كلمة المرور - You Must enter your name")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
        [Required(ErrorMessage = "يجب عليك كتابة بريدك الالكتروني - You Must Write Your Email")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        [Required(ErrorMessage = "يجب عليك ادخال تاريخ الميلاد - You must enter your birthday")]
        [DataType(DataType.Date)]
        public string BirthDay { get; set; }
        [Required(ErrorMessage = "يجب عليك كتابة رقم الجوال - You must write your phone number")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "الرجاء كتابة الجوال بالصيغة الصحيحة - Please Write Phone Number Correctly")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }



        //[Required(ErrorMessage = "يجب عليك اختيار لغتز المفضلة - You must choose your language")]
        //public string FavoriteLanguage { get; set; }
        //[Required(ErrorMessage = "يجب عليك ادخال المدينة - You must choose your city")]
        //public string City { get; set; }
        //[Required(ErrorMessage = "يجب عليك اختيار نوعك - you must choose your gender")]
        //public bool IsMale { get; set; }
    }
}
