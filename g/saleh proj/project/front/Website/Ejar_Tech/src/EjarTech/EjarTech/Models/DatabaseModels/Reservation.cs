using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.DatabaseModels
{
    public class Reservation
    {
        public ObjectId _id { get; set; }
        public ObjectId CarId { get; set; }
        public ObjectId BranchId { get; set; }
        public Object UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal Price { get; set; }
        public string States { get; set; }

    }
}
