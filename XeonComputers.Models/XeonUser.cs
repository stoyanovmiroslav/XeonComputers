using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace XeonComputers.Models
{
    public class XeonUser : IdentityUser
    {
        public XeonUser()
        {
            this.Addresses = new HashSet<Address>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual ICollection<XeonUserFavoriteProduct> FavoriteProducts { get; set; }

        public virtual PartnerRequest PartnerRequest { get; set; }
    }
}