using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Enums;

namespace XeonComputers.Models
{
    public class ChildCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Image ImageUrl { get; set; }

        public ParentCategory ParentCategory { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}