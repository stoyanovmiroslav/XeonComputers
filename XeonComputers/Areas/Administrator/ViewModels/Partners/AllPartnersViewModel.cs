using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Areas.Administrator.ViewModels.Partners
{
    public class AllPartnersViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyUniqueIdentifier { get; set; }

        public string CompanyManager { get; set; }

        public string CompanyAddressCityName { get; set; }

        public string CompanyAddressStreet { get; set; }

        public string CompanyAddressDescription { get; set; }
    }
}