using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Search
{
    class ViewCar
    {
        [Required ]
        public string CompanyName { get; set; }
        [Required ]
        public string TypeOfCar { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string DaysAvailable { get; set; }
        [Required]
        public string BranchSite { get; set; }
        [Required ]
        public int NumberOfBookings { get; set; }



    }
}
