using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Search
{
    class BookingInfo
    {
        [Required]
        public string DateInvoice { get; set; }
        [Required]
        public float OperationNumber { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string OperationType { get; set; }
        // Operation type == Rent  
        //[
        [Required]
        public string DateOfTaking { get; set; }
        [Required]
        public string DateOfReceving { get; set; }
        //]
        // Operation type == Sale
        //[
        [Required]
        public string DateOfPurchase { get; set; }
        //]
        [Required]
        public string OfficeName { get; set; }
        [Required]
        public string OfficeLoction { get; set; }
        [Required]
        public string ModelCar { get; set; }
        [Required]
        public string TypeCar { get; set; }
    }
}
