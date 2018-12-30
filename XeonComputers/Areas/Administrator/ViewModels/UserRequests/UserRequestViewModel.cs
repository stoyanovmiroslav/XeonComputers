using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Areas.Administrator.ViewModels.UserRequests
{
    public class UserRequestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public bool Seen { get; set; }

        public DateTime RequestDate { get; set; }
    }
}