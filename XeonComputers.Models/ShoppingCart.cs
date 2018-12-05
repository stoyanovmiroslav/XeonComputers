using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public virtual XeonUser User { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}