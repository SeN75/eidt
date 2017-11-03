using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DatabaseModels
{
    public class Branch
    {
        public ObjectId _id { get; set; }
        public string BranchName { get; set; }
        public string BranchEMail { get; set; }
        public string BranchSupervisor { get; set; }
        public string PhoneNumber { get; set; }
        public string CityName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool Active { get; set; }
        public ObjectId CompanyId { get; set; }
        public ObjectId ManagerId { get; set; }
    }
}
