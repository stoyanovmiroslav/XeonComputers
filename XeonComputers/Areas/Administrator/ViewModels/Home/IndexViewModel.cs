using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Areas.Administrator.ViewModels.Home
{
    public class IndexViewModel
    {
        public IList<IndexProcessedОrdersViewModels> ProcessedОrdersViewModel { get; set; }

        public IList<IndexUnprocessedОrdersViewModels> UnprocessedОrdersViewModel { get; set; }

        public int PartnerRequestsCount { get; set; }
    }
}