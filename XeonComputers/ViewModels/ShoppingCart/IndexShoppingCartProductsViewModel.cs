using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XeonComputers.Areas.Administrator.ViewModels.Suppliers;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.ShoppingCart
{
    public class IndexShoppingCartProductsViewModel
    {
        public  IList<ShoppingCartProductsViewModel> ShoppingCartProducts { get; set; }

        public IList<SupplierViewModel> Suppliers { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        public DeliveryType DeliveryType { get; set; }

        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        public int SupplierId { get; set; }
    }
}