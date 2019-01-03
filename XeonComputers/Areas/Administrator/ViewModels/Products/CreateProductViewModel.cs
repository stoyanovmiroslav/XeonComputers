using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;

namespace XeonComputers.Areas.Administrator.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        [Required(ErrorMessage = "Полето\"{0}\" e задължително.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1}.")]
        public string Name { get; set; }

        [Display(Name = "Тип на продукта")]
        public ProductType ProductType { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Спецификация")]
        public string Specification { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [Range(1, int.MaxValue, ErrorMessage = "Полето \"{0}\" трябва да е число в диапазона от {1} до {2}")]
        public decimal Price { get; set; }

        [Display(Name = "Цена за фирми")]
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [Range(1, int.MaxValue, ErrorMessage = "Полето \"{0}\" трябва да е число в диапазона от {1} до {2}")]
        public decimal ParnersPrice { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        public int ChildCategoryId { get; set; }

        public ICollection<SelectListItem> ChildCategories { get; set; }

        [Display(Name = "Снимки")]
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        public ICollection<IFormFile> FormImages { get; set; }
    }
}
