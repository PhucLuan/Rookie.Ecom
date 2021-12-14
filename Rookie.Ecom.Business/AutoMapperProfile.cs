using AutoMapper;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rookie.Ecom.Business
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryDto, Category>()
               .ForMember(p => p.Categoris, x => x.Ignore())
               .ForMember(p => p.CategoriesList, x => x.Ignore())
               .ForMember(p => p.Products, x => x.Ignore());

            CreateMap<Category, CategoryDto>()
               .ForMember(p => p.CategoryMenus, x => x.Ignore())
               .ForMember(p => p.Categories, x => x.Ignore());

            CreateMap<BrandDto, Brand>()
               .ForMember(p => p.Products, x => x.Ignore());

            CreateMap<Brand, BrandDto>();

            CreateMap<ProductListDto, Product>()
               .ForMember(p => p.ProductStock, x => x.Ignore())
               .ForMember(p => p.Category, x => x.Ignore())
               .ForMember(p => p.Category, x => x.Ignore())
               .ForMember(p => p.Brand, x => x.Ignore())
               .ForMember(p => p.Unit, x => x.Ignore())
               .ForMember(p => p.ProductCommentses, x => x.Ignore())
               .ForMember(p => p.ProductImages, x => x.Ignore())
               .ForMember(p => p.OrderDetails, x => x.Ignore());

            CreateMap<Product, ProductListDto>()
               .ForMember(x => x.TotalImage, p => p.Ignore())
               .ForMember(x => x.ProductComments, p => p.Ignore())
               .ForMember(x => x.FinalPrice, p => p.Ignore())
               .ForMember(x => x.ProductCommentsList, p => p.Ignore())
               .ForMember(x => x.AverageRating, p => p.Ignore())
               .ForMember(x => x.ImageTitle, p => p.Ignore())
               .ForMember(x => x.ImagePath, p => p.Ignore())
               .ForMember(x => x.ImageList, p => p.Ignore())
               .ForMember(x => x.SecondImagePath, p => p.Ignore());

            CreateMap<ProductDto, Product>()
               .ForMember(x => x.Category, p => p.Ignore())
               .ForMember(x => x.Brand, p => p.Ignore())
               .ForMember(x => x.Unit, p => p.Ignore())
               .ForMember(x => x.ProductCommentses, p => p.Ignore())
               .ForMember(x => x.ProductImages, p => p.Ignore())
               .ForMember(x => x.OrderDetails, p => p.Ignore())
               .ForMember(x => x.AddedDate, p => p.Ignore())
               .ForMember(x => x.ModifiedDate, p => p.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.CategoryList, p => p.Ignore())
                .ForMember(x => x.BrandList, p => p.Ignore())
                .ForMember(x => x.UnitList, p => p.Ignore());

            CreateMap<Category, CategoryListDto>()
               .ForMember(x => x.CategoryParentName, p => p.Ignore())
               .ForMember(x => x.TotalProduct, p => p.Ignore());

            CreateMap<Orders, OrdersDto>()
                .ForMember(x => x.Email, p => p.Ignore())
                .ForMember(x => x.DeliveryAddress, p => p.Ignore());

            CreateMap<Orders, OrderDetailsAdminDto>()
                .ForMember(x => x.DeliveryAddress, p => p.Ignore())
                .ForMember(x => x.CustomerName, p => p.Ignore())
                .ForMember(x => x.CustomerEmail, p => p.Ignore())
                .ForMember(x => x.OrderProductLists, p => p.Ignore())
                .ForMember(x => x.UserAddressId, p => p.Ignore());
        }
    }
}
