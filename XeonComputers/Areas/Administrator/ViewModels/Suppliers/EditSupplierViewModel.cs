using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Areas.Administrator.ViewModels.Suppliers
{
    public class EditSupplierViewModel
    {
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1} символа.")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [Range(1, int.MaxValue, ErrorMessage = "Полето \"{0}\" трябва да е число в диапазона от {1} до {2}")]
        [Display(Name = "Цена на доставката до адрес")]
        public decimal PriceToHome { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Полето \"{0}\" трябва да е число в диапазона от {1} до {2}")]
        [Display(Name = "Цена на доставката до офис")]
        public decimal PriceToOffice { get; set; }
    }
}