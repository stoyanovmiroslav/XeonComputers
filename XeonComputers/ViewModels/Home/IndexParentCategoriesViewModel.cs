using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.ViewModels.Home
{
    public class IndexParentCategoriesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<IndexChildCategoriesViewModel> ChildCategories { get; set; }
    }
}