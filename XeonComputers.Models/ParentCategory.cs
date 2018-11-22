using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class ParentCategory
    {
        public int Id { get; set; }

        public string Name  { get; set; }

        public ICollection<ChildCategory> ChildCategories { get; set; }
    }
}