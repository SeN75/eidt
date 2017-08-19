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
        public string TypeOfCar { get; set; }
        [Required]
        public string  ReceivedDate { get; set; }
        [Required]
        public string DeliveryDate { get; set; }
        [Required]
        public string  City { get; set; }    
        
    }
}
