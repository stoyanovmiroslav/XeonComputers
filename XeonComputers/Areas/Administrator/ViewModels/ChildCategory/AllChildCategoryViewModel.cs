using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.ViewModels.ChildCategory
{
    public class AllChildCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ParentCategoryName { get; set; }

        public int ProductsCount { get; set; }
    }
}