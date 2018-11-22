using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class CategoryProduct
    {
        public int ChildCategoryId { get; set; }
        public ChildCategory ChildCategory { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}