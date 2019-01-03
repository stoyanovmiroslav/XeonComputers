using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.ViewModels.ChildCategory
{
    public class EditChildCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1} символа.")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Снимка")]
        public IFormFile FormImage { get; set; }

        [Required(ErrorMessage = "Моля, изберете {0}.")]
        [Display(Name = "Основна категория")]
        public int? ParentCategoryId { get; set; }

        public ICollection<XeonComputers.Models.ParentCategory> ParentCategories { get; set; }
    }
}