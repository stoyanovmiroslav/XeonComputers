using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;
using XeonComputers.ViewModels.Favorites;
using XeonComputers.ViewModels.Home;
using XeonComputers.ViewModels.Orders;
using XeonComputers.ViewModels.Products;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.MappingConfiguration
{
    public class XeonComputersProfile : Profile
    {
        public XeonComputersProfile()
        {
            this.CreateMap<ParentCategory, IndexParentCategoriesViewModel>();

            this.CreateMap<ChildCategory, IndexChildCategoriesViewModel>();

            this.CreateMap<Address, OrderAdressViewModel>();

            this.CreateMap<Order, ConfirmOrderViewModel>();

            this.CreateMap<Product, IndexProductViewModel>()
                          .ForMember(x => x.ImageUrl, y => y.MapFrom(src => src.Images.FirstOrDefault().ImageUrl));

            this.CreateMap<XeonUserFavoriteProduct, AllFavoriteViewModel>()
                          .ForMember(x => x.ProductImageUrl, y => y.MapFrom(src => src.Product.Images.FirstOrDefault().ImageUrl));

            this.CreateMap<Order, MyOrderViewModel>()
                          .ForMember(x => x.Status, y => y.MapFrom(src => src.Status.GetDisplayName()))
                          .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus.GetDisplayName()))
                          .ForMember(x => x.PaymentType, y => y.MapFrom(src => src.PaymentType.GetDisplayName()));

            this.CreateMap<Product, DetailsProductViewModel>()
                          .ForMember(x => x.ImageUrls, y => y.MapFrom(src => src.Images.Select(x => x.ImageUrl)));

            this.CreateMap<Product, ShoppingCartProductsViewModel>()
                          .ForMember(x => x.ImageUrl, y => y.MapFrom(src => src.Images.FirstOrDefault().ImageUrl));
        }
    }
}