using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Models.SupportComplexModel
{
    public class ComplexDateTime 
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public bool IsConflictWith(string from, string to)
        {
            ComplexDateTime dateTime = new ComplexDateTime { From = DateTime.Parse(from), To = DateTime.Parse(to) };
            if (dateTime.From >= this.From && dateTime.From <= this.To)
                return true;
            if (dateTime.To >= this.From && dateTime.To <= this.To)
                return true;
            return false;
        }
    }
}
