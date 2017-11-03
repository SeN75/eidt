﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.AdminPortalModel
{
    public class AddCompany
    {
        [Required(ErrorMessage = "Please Enter Company Name - الرجاء كتابة اسم الشركة")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Please Enter Company Id - الرجاء كتابة السجل التجاري")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Company id must be 10 field - رقم السجل لابد ان يكون 10 خانات")]
        public string CompanyId { get; set; }
        [Required(ErrorMessage = "Please Enter Sipervisor Name - الرجاء كتابة اسم المشرف")]
        public string SupervisorName { get; set; }
        [Required(ErrorMessage = "Please Enter EMail - الرجاء كتابة البريد الالكتروني")]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Please Enter Phone Number1")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "الرجاء كتابة الجوال بالصيغة الصحيحة - Please Write Phone Number Correctly")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string StatePhone { get; set; }
    }
}
