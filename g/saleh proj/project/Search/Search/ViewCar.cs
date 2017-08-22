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
        [Required]
        public string CarImage { get; set; }
        [Required]
        public string BranchSite { get; set; }
        [Required ]
        public string TypeOfCar { get; set; }
        [Required]
        public string ModelCar { get; set; }
        [Required]
        public string MoreInfo { get; set; }
        //more info 
        public string  BagSpace { get; set; }
        public string SeateNumber { get; set; }
        public string DoorNumber { get; set; }
        public string TankyeSpace { get; set; }
        public string TransmissionType { get; set; }

    }
}
