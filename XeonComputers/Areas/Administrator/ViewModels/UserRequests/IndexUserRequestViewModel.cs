using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.ViewModels.UserRequests
{
    public class IndexUserRequestViewModel
    {
        public IList<UserRequestViewModel> UserRequestsViewModel { get; set; }

        public UserRequestViewModel UserRequestViewModel  { get; set; }
    }
}
