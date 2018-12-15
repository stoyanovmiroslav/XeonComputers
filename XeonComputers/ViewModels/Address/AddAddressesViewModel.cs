using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.ViewModels.Address
{
    public class AddAddressesViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Адрес за доставка")]
        [Required(ErrorMessage = "Моля въведете \"{0}\".")]
        public string Street { get; set; }

        [Display(Name = "Допълнение към адреса")]
        public string Description { get; set; }

        [Display(Name = "Град")]
        [Required(ErrorMessage = "Моля въведете \"{0}\".")]
        public string CityName { get; set; }

        [Display(Name = "Пощенски код")]
        [Required(ErrorMessage = "Моля въведете \"{0}\".")]
        public string CityPostcode { get; set; }
    }
}