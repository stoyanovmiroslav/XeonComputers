using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class ShoppingCartProduct
    {
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}