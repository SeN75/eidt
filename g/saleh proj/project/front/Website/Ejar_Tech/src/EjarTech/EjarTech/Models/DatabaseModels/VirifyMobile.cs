using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DatabaseModels
{
    public class VirifyMobile
    {
        public ObjectId _id { get; set; }
        public ObjectId UserId { get; set; }
        public string Code { get; set; }
        public int SendTime { get; set; }
        public bool IsUsed { get; set; }
    }
}
