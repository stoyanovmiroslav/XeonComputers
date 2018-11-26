using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.ViewModels.ChildCategory
{
    public class AddChildCategoryViewModel
    {
        [Required(ErrorMessage = "Моля, въведете {0}!")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Снимка")]
        public IFormFile FormImage { get; set; }

        [Required(ErrorMessage = "Моля, изберете {0}!")]
        [Display(Name = "Основна категория")]
        public int? ParentId { get; set; }

        public ICollection<XeonComputers.Models.ParentCategory> ParentCategories { get; set; }
    }
}