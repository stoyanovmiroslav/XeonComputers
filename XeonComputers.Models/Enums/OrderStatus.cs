using System.ComponentModel.DataAnnotations;

namespace XeonComputers.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Обработва се...")]
        Processing = 1,

        [Display(Name = "Обработена")]
        Processed = 2,

        [Display(Name = "Изпратена")]
        Sent = 3,

        [Display(Name = "Доставена")]
        Delivered = 4
    }
}