using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.SearchModels
{
    public class SearchCarForRent
    {
        [Required]
        public string PickUpPlace { get; set; }
        //[Required]
        //public string DropOffPlace { get; set; }
        [Required]
        public string CarCompany { get; set; }
        [Required]
        public string CarType { get; set; }
        [Required]
        public string ModelYear { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string PickupDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string PickupTime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string DropOffDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string DropTime { get; set; }

    }
}
