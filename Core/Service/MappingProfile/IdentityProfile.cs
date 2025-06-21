using AutoMapper;
using Domain.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
     class IdentityProfile :Profile
    {
        public IdentityProfile()
        {
            CreateMap<Address, Address>().ReverseMap();
        }
    }
}
