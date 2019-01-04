using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Areas.Administrator.ViewModels.Components
{
    public class AdminNavbarViewModel
    {
        public int PartnerRequestsCount { get; set; }

        public int UserRequestsCount { get; set; }

        public int UnprocessedOrdersCount { get; set; }
    }
}
