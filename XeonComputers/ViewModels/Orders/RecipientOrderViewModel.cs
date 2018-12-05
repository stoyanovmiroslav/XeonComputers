using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.Orders
{
    public class RecipientOrderViewModel
    {
        [Display(Name = "Име на получателя")]
        public string FullName { get; set; }

        [Display(Name = "GSM номер")]
        public string PhoneNumber { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}