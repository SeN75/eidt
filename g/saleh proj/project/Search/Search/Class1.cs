using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Search
{
    class Search
    {
        
        [Required]
        public string PickingUpLocations { get; set; }
        [Required]
        public string PickingUpDate { get; set; }
        [Required]
        public string PickingUpTime{ get; set; }
        [Required]
        public string DropingUpLocations { get; set; }
        [Required]
        public string DropingUpDate { get; set; }
        [Required]
        public string TypeOfCar { get; set; }
        [Required]
        public bool WithDriver { get; set; }


    }
}
