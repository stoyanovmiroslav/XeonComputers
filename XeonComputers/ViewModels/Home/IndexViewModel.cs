using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace XeonComputers.ViewModels.Home
{
    public class IndexViewModel
    {
        public IPagedList<IndexProductViewModel> ProductsViewModel { get; set; }

        public IList<IndexParentCategoriesViewModel> CategoriesViewModel { get; set; }

        public int? ChildCategoryId { get; set; }

        public string SearchString { get; set; }
    }
}