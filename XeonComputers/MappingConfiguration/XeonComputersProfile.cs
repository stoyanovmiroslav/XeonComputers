using AutoMapper;
using System.Linq;
using XeonComputers.Areas.Administrator.ViewModels.ChildCategory;
using XeonComputers.Areas.Administrator.ViewModels.PartnerRequests;
using XeonComputers.Areas.Administrator.ViewModels.Home;
using XeonComputers.Areas.Administrator.ViewModels.Orders;
using XeonComputers.Areas.Administrator.ViewModels.ParentCategory;
using XeonComputers.Areas.Administrator.ViewModels.Products;
using XeonComputers.Models;
using XeonComputers.ViewModels.Favorites;
using XeonComputers.ViewModels.Home;
using XeonComputers.ViewModels.Orders;
using XeonComputers.ViewModels.Products;
using XeonComputers.ViewModels.ShoppingCart;
using XeonComputers.Areas.Administrator.ViewModels.Partners;
using XeonComputers.Areas.Administrator.Controllers;
using XeonComputers.Areas.Administrator.ViewModels.UserRequests;
using XeonComputers.Areas.Administrator.ViewModels.Suppliers;

namespace XeonComputers.MappingConfiguration
{
    public class XeonComputersProfile : Profile
    {
        public XeonComputersProfile()
        {
            this.CreateMap<ParentCategory, IndexParentCategoriesViewModel>();
            this.CreateMap<ChildCategory, IndexChildCategoriesViewModel>();
            this.CreateMap<Address, OrderAdressViewModel>();
            this.CreateMap<CreateProductViewModel, Product>();
            this.CreateMap<Product, EditProductViewModel>();
            this.CreateMap<Product, DeleteProductViewModel>();
            this.CreateMap<EditProductViewModel, Product>();
            this.CreateMap<ParentCategory, ParentCategoryViewModel>();
            this.CreateMap<ChildCategory, EditChildCategoryViewModel>();
            this.CreateMap<ChildCategory, AllChildCategoryViewModel>();
            this.CreateMap<Order, CompleteOrderViewModel>();
            this.CreateMap<CompanyViewModel, Company>();
            this.CreateMap<XeonUser, RequestUserCompanyViewModel>();
            this.CreateMap<Supplier, SupplierViewModel> ();
            this.CreateMap<Supplier, EditSupplierViewModel> ();
            this.CreateMap<XeonUser, AllPartnersViewModel>();
            this.CreateMap<Company, XeonComputers.Areas.Identity.Pages.Account.Manage.IndexModel.InputModel>();

            this.CreateMap<UserRequest, UserRequestViewModel>()
                            .ForMember(x => x.RequestDate, y => y.MapFrom(src => src.RequestDate.ToLongTimeString()));

            this.CreateMap<Order, ConfirmOrderViewModel>()
                            .ForMember(x => x.PaymentTypeDisplayName, y => y.MapFrom(src => src.PaymentType.GetDisplayName()))
                            .ForMember(x => x.TotalPrice, y => y.MapFrom(src => src.TotalPrice));

            this.CreateMap<Order, ViewModels.Orders.OrderDetailsViewModel>()
                          .ForMember(x => x.Status, y => y.MapFrom(src => src.Status.GetDisplayName()))
                          .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus.GetDisplayName()))
                          .ForMember(x => x.PaymentType, y => y.MapFrom(src => src.PaymentType.GetDisplayName()));

            this.CreateMap<OrderProduct, ViewModels.Orders.OrderProductsViewModel>()
                          .ForMember(x => x.ImageUrl, y => y.MapFrom(src => src.Product.Images.FirstOrDefault().ImageUrl));
          
            this.CreateMap<OrderProduct, Areas.Administrator.ViewModels.Orders.OrderProductsViewModel>()
                          .ForMember(x => x.ImageUrl, y => y.MapFrom(src => src.Product.Images.FirstOrDefault().ImageUrl));

            this.CreateMap<Order, Areas.Administrator.ViewModels.Orders.OrderDetailsViewModel>()
                          .ForMember(x => x.Status, y => y.MapFrom(src => src.Status.GetDisplayName()))
                          .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus.GetDisplayName()))
                          .ForMember(x => x.PaymentType, y => y.MapFrom(src => src.PaymentType.GetDisplayName()));

            this.CreateMap<Order, IndexUnprocessedОrdersViewModels>()
                          .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus.GetDisplayName()))
                          .ForMember(x => x.PaymentType, y => y.MapFrom(src => src.PaymentType.GetDisplayName()));

            this.CreateMap<Order, IndexProcessedОrdersViewModels>()
                          .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus.GetDisplayName()))
                          .ForMember(x => x.PaymentType, y => y.MapFrom(src => src.PaymentType.GetDisplayName()));
     
            this.CreateMap<Order, MyOrderViewModel>()
                          .ForMember(x => x.Status, y => y.MapFrom(src => src.Status.GetDisplayName()))
                          .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus.GetDisplayName()))
                          .ForMember(x => x.PaymentType, y => y.MapFrom(src => src.PaymentType.GetDisplayName()));

            this.CreateMap<Order, DeliveredОrdersViewModels>()
                          .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus.GetDisplayName()))
                          .ForMember(x => x.PaymentType, y => y.MapFrom(src => src.PaymentType.GetDisplayName()));

            this.CreateMap<Product, DetailsProductViewModel>()
                          .ForMember(x => x.ImageUrls, y => y.MapFrom(src => src.Images.Select(x => x.ImageUrl)))
                          .ForMember(x => x.Raiting, y => y.MapFrom(src => src.Reviews.Count == 0 ? 0 : (double)src.Reviews.Sum(s => s.Raiting) / src.Reviews.Count()));

            this.CreateMap<Product, ShoppingCartProductsViewModel>()
                          .ForMember(x => x.ImageUrl, y => y.MapFrom(src => src.Images.FirstOrDefault().ImageUrl));

            this.CreateMap<Product, IndexProductViewModel>()
                           .ForMember(x => x.ImageUrl, y => y.MapFrom(src => src.Images.FirstOrDefault().ImageUrl))
                           .ForMember(x => x.Raiting, y => y.MapFrom(src => src.Reviews.Count == 0 ? 0 : (double)src.Reviews.Sum(s => s.Raiting) / src.Reviews.Count()));
            

            this.CreateMap<XeonUserFavoriteProduct, AllFavoriteViewModel>()
                          .ForMember(x => x.ProductImageUrl, y => y.MapFrom(src => src.Product.Images.FirstOrDefault().ImageUrl));
        }
    }
}