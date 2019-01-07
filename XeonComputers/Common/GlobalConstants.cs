using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Common
{
    public static class GlobalConstants
    {
        public const string WWWROOT = "wwwroot";

        public const string SESSION_SHOPPING_CART_KEY = "shoppingCart";

        public const int BULGARIAN_HOURS_FROM_UTC_TIME = 2;

        public const string CHILD_CATEGORY_PATH_TEMPLATE = "wwwroot/images/ChildCategories/image{0}.jpg";
        public const string CHILD_CATEGORY_SRC_ROOT_TEMPLATE = "/images/ChildCategories/image{0}.jpg";

        public const string PRODUCT_PATH_TEMPLATE = "wwwroot/images/Products/image{0}.jpg";
        public const string PRODUCT_SRC_ROOT_TEMPLATE = "/images/Products/image{0}.jpg";

        public const string URL_TEMPLATE_AUTOCOMPLETE = "https://xeoncomputers.azurewebsites.net/Products/Details/{0}";

    }
}