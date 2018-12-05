using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.ViewModels.Orders
{
    public class CreateAddressOrderViewModel
    {
        public List<OrderAdressViewModel> OrderAddressesViewModel { get; set; }

        public OrderAdressViewModel OrderAdressViewModel { get; set; }

        public int DeliveryAddressId { get; set; }
    }
}