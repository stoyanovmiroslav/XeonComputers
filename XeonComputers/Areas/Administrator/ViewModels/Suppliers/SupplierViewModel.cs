using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Areas.Administrator.ViewModels.Suppliers
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal PriceToHome { get; set; }

        public decimal PriceToOffice { get; set; }

        public bool IsDefault { get; set; }
    }
}