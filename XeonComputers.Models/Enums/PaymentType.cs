using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XeonComputers.Models.Enums
{
    public enum PaymentType
    {
        [Display(Name = "ePay.bg")]
        Epay = 1,

        [Display(Name = "В брой (Изипей)")]
        EasyPay = 2,

        [Display(Name = "Наложен платеж")]
        CashОnDelivery = 3,

        [Display(Name = "Visa, MasterCard и др.")]
        Card = 4
    }
}
