using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class XeonUserFavoriteProduct
    {
        public string XeonUserId { get; set; }
        public XeonUser XeonUser { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}