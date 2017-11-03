using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DashbordModel
{
    public class AddBranch
    {
        [Required(ErrorMessage = "Please Enter Branch Name - الرجاء كتابة اسم الفرع")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Please Enter Password - الرجاء كتابة كلمة المرور")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password Must Be 8 To 20 Field - كلمة المرور لابد أن تكون من 8 الى 20 خانة")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Branch EMail - الرجاء كتابة بريد  الفرع")]
        [DataType(DataType.EmailAddress)]
        public string BranchEMail { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Supervisor - الرجاء كتابة اسم مشرف الفرع")]
        public string BranchSupervisor { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number - الرجاء كتابة رقم الجوال")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Phone Number Must Be 12 Field - رقم الجوال يجب ان يكون 12 خانة")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter City Name - الرجاء كتابة اسم المدينة")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Please Select Location - الرجاء تحديد الموقع الجغرافي")]
        public decimal Latitude { get; set; }
        
        [Required(ErrorMessage = "Please Select Location - الرجاء تحديد الموقع الجغرافي")]
        public decimal Longitude { get; set; }
    }
}
