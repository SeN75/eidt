using EjarTech.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.SearchModels
{
    public class ViewCarsRent
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<CarForRent> Cars{ get; set; }
    }
}
