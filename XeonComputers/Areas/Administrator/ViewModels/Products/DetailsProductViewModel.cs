using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace XeonComputers.Areas.Administrator.ViewModels.Products
{
    public class DetailsProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Тип на продукта")]
        public ProductType ProductType { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Спецификация")]
        public string Specification { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Цена за фирми")]
        public decimal ParnersPrice { get; set; }

        [Display(Name = "Категория")]
        public string ChildCategoryName { get; set; }

        //public ICollection<Image> ImageUrls { get; set; }
    }
}