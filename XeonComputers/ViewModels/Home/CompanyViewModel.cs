using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.ViewModels.Home
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UniqueIdentifier { get; set; }

        public string Manager { get; set; }

        public string Owner { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string AddressCityName { get; set; }

        public string AddressStreet { get; set; }

        public string AddressDescription { get; set; }
    }
}
