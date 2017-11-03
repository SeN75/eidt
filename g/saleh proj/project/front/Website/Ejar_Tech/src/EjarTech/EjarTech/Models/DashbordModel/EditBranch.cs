using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DashbordModel
{
    public class EditBranch
    {
        [Required(ErrorMessage = "Please Enter Branch Name - الرجاء كتابة اسم الفرع")]
        public string BranchName { get; set; }
        
        [Required(ErrorMessage = "Please Enter Branch EMail - الرجاء كتابة بريد  الفرع")]
        [DataType(DataType.EmailAddress)]
        public string BranchEMail { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Supervisor - الرجاء كتابة اسم مشرف الفرع")]
        public string BranchSupervisor { get; set; }

        [Required(ErrorMessage = "Please Select Location - الرجاء تحديد الموقع الجغرافي")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Please Select Location - الرجاء تحديد الموقع الجغرافي")]
        public decimal Longitude { get; set; }
    }
}
