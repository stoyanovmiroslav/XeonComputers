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
        public string DeliveryAddress { get; set; }

        [Display(Name = "Допълнение към адреса")]
        public string AdditionТoАddress { get; set; }

        [Display(Name = "Град")]
        public string City { get; set; }

        [Display(Name = "Пощенски код")]
        public string Postcode { get; set; }
    }
}
