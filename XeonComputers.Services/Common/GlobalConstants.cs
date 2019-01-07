using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.Services.Common
{
    public static class GlobalConstants
    {
        public const string WWWROOT = "wwwroot";

        public const int BULGARIAN_HOURS_FROM_UTC_TIME = 2;

        public const string CHILD_CATEGORY_PATH_TEMPLATE = "wwwroot/images/ChildCategories/image{0}.jpg";
        public const string CHILD_CATEGORY_SRC_ROOT_TEMPLATE = "/images/ChildCategories/image{0}.jpg";

        public const string PRODUCT_PATH_TEMPLATE = "wwwroot/images/Products/image{0}.jpg";
        public const string PRODUCT_SRC_ROOT_TEMPLATE = "/images/Products/image{0}.jpg";

        public const string CONFIRM_ORDER_EMAIL_TEMPLATE = "<p>Здравей,</p><p> Благодарим ти за направената поръчка!</p><div><div class=\"card card-body\"><div><strong>Име:</strong> {0}</div><div><strong>Телефонен номер:</strong> {1}</div><div><strong>Град:</strong> {2}, {3}</div><div><strong>Адрес:</strong> {4} {5}</div><div><strong>Цена:</strong> {6}лв. (В цената не е включена доставката.)</div></div></div>";

    }
}