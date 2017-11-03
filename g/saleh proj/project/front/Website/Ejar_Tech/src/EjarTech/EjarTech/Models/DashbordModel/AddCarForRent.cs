using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DashbordModel
{
    public class AddCarForRent
    {
        [Required(ErrorMessage = "Please Enter Car Type - الرجاء ادخال نوع السيارة")]
        public string CarType { get; set; }
        [Required(ErrorMessage = "Please Enter Car Company - الرجاء ادخال شركة السيارة")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Please Enter Car Name - الرجاء ادخال موديل السيارة")]
        public string ModelName { get; set; }
        [Required(ErrorMessage = "Please Enter Car Model Year - الرجاء ادخال سنة الموديل")]
        public string ModelYear { get; set; }
        [Required(ErrorMessage = "Please Enter Car Doors Number - الرجاء ادخال عدد الأبواب")]
        public int? DoorsNumber { get; set; }
        [Required(ErrorMessage = "Please Enter Car Seats Number - الرجاء ادخال عدد المقاعد")]
        public int? SeatsNumber { get; set; }
        [Required(ErrorMessage = "Please Enter Car Seats Number - الرجاء ادخال عدد الحقائب")]
        public int? BagsNumber { get; set; }
        [Required(ErrorMessage = "Please Enter Car Gear Type - الرجاء ادخال نوع ناقل الحركة")]
        public string GearType { get; set; }
        [Required(ErrorMessage = "Please Enter Car Painting - الرجاء ادخال نص اللوحة")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Please Enter Valid Car Paintining Text - الرجاء ادخال أحرف اللوحة بصورة صحيحة")]
        public string CarPanelText { get; set; }
        [Required(ErrorMessage = "Please Enter Car Painting - الرجاء ادخال نص اللوحة")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Please Enter Valid Car Paintining Number - الرجاء ادخال أحرف اللوحة بصورة صحيحة")]
        public string CarPanelNumber { get; set; }
        [Required(ErrorMessage = "Please Choose If Car Open Mile Or No - الرجاء تحديد ما إذا كان مفتوح الاميال ام لا")]
        public bool IsOpenMile { get; set; }
        //[Required(ErrorMessage = "Please Choose If Car can Return in defferent branch - الرجاء تحديد ما إذا كان يمكن إعادتها في فرع آخر")]
        //public bool CanReturnInDefferentPlace { get; set; }
        [Required(ErrorMessage = "Please Enter Mile Number - الرجاء تحديد عدد الكيلومترات")]
        public int? MileNumber { get; set; }
        [Required(ErrorMessage = "Please Enter InsurancesType - الرجاء ادخال التامينات")]
        public string Insurances { get; set; }
        [Required(ErrorMessage = "Please Enter Car Price Per Day - الرجاء ادخال سعر السيارة اليومي")]
        public decimal? PricePerDay { get; set; }
        [Required(ErrorMessage = "Please Enter Car Price Per Week - الرجاء ادخال سعر السيارة الإسبوعي")]
        public decimal? PricePerWeek { get; set; }
        [Required(ErrorMessage = "Please Enter Car Price Per Month - الرجاء ادخال سعر السيارة الشهري")]
        public decimal? PricePerMonth { get; set; }
    }
}
