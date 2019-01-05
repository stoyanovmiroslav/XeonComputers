using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XeonComputers.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "Платено")]
        Paid = 1,

        [Display(Name = "Очаква плащане")]
        Unpaid = 2,

        [Display(Name = "Изтекла")]
        Expired = 3,

        [Display(Name = "Отказана")]
        Denied = 4
    }
}