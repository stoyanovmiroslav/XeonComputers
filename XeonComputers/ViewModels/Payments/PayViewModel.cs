using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.ViewModels.Payments
{
    public class PayViewModel
    {
        public string Encoded { get; set; }

        public string SubmitUrl { get; set; }

        public string ChechSum { get; set; }
    }
}