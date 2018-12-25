using System.ComponentModel.DataAnnotations;

namespace XeonComputers.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "В процес...")]
        Processing = 1,

        [Display(Name = "Необработена")]
        Unprocessed = 2,

        [Display(Name = "Обработена")]
        Processed = 3,

        [Display(Name = "Доставена")]
        Delivered = 4
    }
}