using Microsoft.AspNetCore.Identity;
using System;

namespace XeonComputers.Models
{
    public class XeonUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual Address DeliveryAddress { get; set; }

        public virtual Company Company { get; set; }
    }
}