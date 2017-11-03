using EjarTech.Models.SupportComplexModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DatabaseModels
{
    public class CarForRent
    {
        public ObjectId _id{ get; set; }
        public string CarType { get; set; }
        public string CompanyName { get; set; }
        public string ModelName { get; set; }
        public string ModelYear { get; set; }
        public int? DoorNumber { get; set; }
        public int? SeatsNumber { get; set; }
        public string GearType { get; set; }
        public bool IsOpenMile { get; set; }
        public int? MileLimit { get; set; }
        public int? BagsNumber { get; set; }
        //public bool CanReturnInDefferentPlace { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal PricePerWeek { get; set; }
        public decimal PricePerMonth { get; set; }
        public string Insurances { get; set; }
        public string PanelText { get; set; }
        public string PanelNumber { get; set; }
        public bool IsActiive { get; set; }
        public ComplexDateTime[] OrdersDates { get; set; }
        public ObjectId CompanyId { get; set; }
        public ObjectId BranchId { get; set; }
    }
}
