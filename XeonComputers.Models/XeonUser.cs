using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace XeonComputers.Models
{
    public class XeonUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual Address DeliveryAddress { get; set; }

        public virtual Company Company { get; set; }

        public ICollection<Order> Orders { get; set; }

        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}