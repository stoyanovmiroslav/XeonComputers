using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.ViewModels.Home
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1} символа.")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [Display(Name = "ЕИК")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "{0} трябва да съдържа точно {1} символа.")]
        public string UniqueIdentifier { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1} символа.")]
        [Display(Name = "Управител")]
        public string Manager { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1} символа.")]
        [Display(Name = "Град")]
        public string AddressCityName { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1} символа.")]
        [Display(Name = "Адрес")]
        public string AddressStreet { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
