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

        public virtual string ImageUrl { get; set; }

        public int ParentCategoryId { get; set; }
        public virtual ParentCategory ParentCategory { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}