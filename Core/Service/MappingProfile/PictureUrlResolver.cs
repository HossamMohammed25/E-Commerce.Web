using AutoMapper;
using Domain.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.ProductModulesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    internal class PictureUrlResolver(IConfiguration configuration ) : IValueResolver<Product, ProductDto, string>
    {
        //$"https://localhost:7128/{src.PictureUrl}"
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.PictureUrl)) return string.Empty;
            else
            {
                //var base =
                var pictureUrl = $"{configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
                return pictureUrl ;
            }
            
        }
    }
}
