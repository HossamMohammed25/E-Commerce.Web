using AutoMapper;
using Domain.Models.ProductModule;
using Shared.DTOs.ProductModulesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dist => dist.ProductBrand, option => option.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dist => dist.ProductType, option => option.MapFrom(src => src.ProductType.Name))
                .ForMember(dist => dist.PictureUrl, option => option.MapFrom<PictureUrlResolver>());

            CreateMap<ProductType, TypeDto>();
            CreateMap<ProductBrand, BrandDto>();
        }
    }
}
