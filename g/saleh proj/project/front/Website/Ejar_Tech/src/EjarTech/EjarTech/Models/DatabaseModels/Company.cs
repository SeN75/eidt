using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DatabaseModels
{
    public class Company
    {
        public ObjectId _id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
        public string SupervisorName { get; set; }
        public string EMail { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string StatePhone { get; set; }
        public bool Active { get; set; }
        public ObjectId ManagerId { get; set; }
    }
}
