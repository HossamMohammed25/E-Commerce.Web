using AutoMapper;
using Domain.Contracts;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper,IBasketRepository basketRepository,UserManager<ApplicationUser> userManager,IConfiguration configuration) 
    {
        private readonly Lazy<IProductService> _LazyProductService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
        private readonly Lazy<IBasketService> _LazybasketService = new Lazy<IBasketService>(()=> new BasketService(basketRepository,mapper));
        private readonly Lazy<IAuthenticationService> _LazyAuhthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager,configuration,mapper));
        private readonly Lazy<IOrderService> _LazyOrderService = new Lazy<IOrderService>(() => new OrderService(mapper, basketRepository, unitOfWork));
        public IProductService ProductService => _LazyProductService.Value;


        public IBasketService BasketService => _LazybasketService.Value;

        public IAuthenticationService AuthenticationService => _LazyAuhthenticationService.Value;

        public IOrderService OrderService => _LazyOrderService.Value;
    }
}
