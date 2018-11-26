using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Areas.Administrator.ViewModels.ParentCategory
{
    public class ParentCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ChildCategoriesCount { get; set; }
    }
}