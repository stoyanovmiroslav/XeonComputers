using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal PriceToHome { get; set; }

        public decimal PriceToOffice { get; set; }

        public bool IsDefault { get; set; }
    }
}
