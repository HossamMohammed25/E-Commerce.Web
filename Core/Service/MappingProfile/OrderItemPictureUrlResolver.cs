using AutoMapper;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.OrderDto;
using Shared.DTOs.ProductModulesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    internal class OrderItemPictureUrlResolver(IConfiguration configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl)) return string.Empty;
            else
            {
                //var base =
                var pictureUrl = $"{configuration.GetSection("Urls")["BaseUrl"]}{source.Product.PictureUrl}";
                return pictureUrl;
            }
        }
    }
}
