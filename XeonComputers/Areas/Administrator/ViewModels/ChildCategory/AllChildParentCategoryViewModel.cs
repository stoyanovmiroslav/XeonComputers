using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.ParentCategory;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.ViewModels.ChildCategory
{
    public class AllChildParentCategoryViewModel
    {
        public ICollection<AllChildCategoryViewModel> ChildCategoryViewModel { get; set; }

        public ICollection<ParentCategoryViewModel> ParentCategoryViewModels { get; set; }
    }
}