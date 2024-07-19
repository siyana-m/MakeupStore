using AutoMapper;
using Makeup.Data.Migrations;
using Makeup.Entities;
using Makeup.Services.DTO.Brands;
using Makeup.Services.DTO.Categories;
using Makeup.Services.DTO.Orders;
using Makeup.Services.DTO.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductSummaryDto>();
            CreateMap<Brand, BrandDto>().ForMember(x => x.Name, x => x.MapFrom(y => $"{y.Name}"));
            CreateMap<Category, CategoryDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailsDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Brands, opt => opt.Ignore())
            .ForMember(dest => dest.Categories, opt => opt.Ignore());
        }

    }
}
