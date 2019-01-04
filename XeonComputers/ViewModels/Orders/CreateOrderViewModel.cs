using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.Orders
{
    public class CreateOrderViewModel
    {
        public List<OrderAdressViewModel> OrderAddressesViewModel { get; set; }

        public OrderAdressViewModel OrderAdressViewModel { get; set; }

        [Display(Name = "Адрес на получаване")]
        [Required(ErrorMessage = "Моля изберете \"{0}\".")]
        public int? DeliveryAddressId { get; set; }

        [Display(Name = "Име на получателя")]
        [Required(ErrorMessage = "Моля въведете \"{0}\".")]
        public string FullName { get; set; }

        [Display(Name = "Телефонен номер")]
        [Required(ErrorMessage = "Моля въведете \"{0}\".")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Начин на плащане")]
        [Required(ErrorMessage = "Моля изберете \"{0}\".")]
        public PaymentType PaymentType { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}