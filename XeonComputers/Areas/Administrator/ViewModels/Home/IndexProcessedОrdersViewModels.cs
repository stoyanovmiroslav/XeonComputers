using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;
using XeonComputers.Models.Enums;

namespace XeonComputers.Areas.Administrator.ViewModels.Home
{
    public class IndexProcessedОrdersViewModels
    {
        public int Id { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime? DispatchDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentType { get; set; }
    }
}
