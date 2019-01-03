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
    public class DeleteProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

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
    }
}
