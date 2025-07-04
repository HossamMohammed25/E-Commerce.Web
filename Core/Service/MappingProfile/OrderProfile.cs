﻿using AutoMapper;
using Domain.Models.OrderModule;
using Shared.DTOs.IdentityDtos;
using Shared.DTOs.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName));

            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(D=>D.ProductName,O=>O.MapFrom(S=>S.Product.ProductName))
                .ForMember(D=>D.PictureUrl,O=>O.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
