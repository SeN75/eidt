using EjarTech.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.SearchModels
{
    public class ViewReservationInfo
    {
        public CarForRent Car { get; set; }
        public Branch Office { get; set; }
        public int Days { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
