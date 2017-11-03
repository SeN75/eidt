using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DatabaseModels
{
    public class User
    {
        public ObjectId _id{ get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }
        //public bool VirifyEMail { get; set; }
        public DateTime BirthDay { get; set; }
        public string PhoneNumber { get; set; }
        public bool VirifyPhoneNumber { get; set; }
        public string Permission { get; set; }
        public bool IsBlocked { get; set; }
        public bool CardPayOnly { get; set; }
        public ObjectId TempCode { get; set; }
    }
}
