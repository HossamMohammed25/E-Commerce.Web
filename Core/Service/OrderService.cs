using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.BasketModule;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Service.Specifications;
using Service.Specifications.OrderModuleSpecification;
using ServiceAbstraction;
using Shared.DTOs.IdentityDtos;
using Shared.DTOs.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service
{
    public class OrderService(IMapper _mapper,IBasketRepository _basketRepository,IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string Email)
        {
            //Map Address 
            var OrderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.shipToAddress); 
            //Get Basket 
            var Basket = await _basketRepository.GetBasketAsync(orderDto.BasketId)
                ?? throw new BasketNotFoundException(orderDto.BasketId);
            ArgumentNullException.ThrowIfNullOrEmpty(Basket.paymentIntentId);
            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var OrderSpec = new OrderWithPaymentIntentIdSpecifications(Basket.paymentIntentId);
            var ExistOrder = await OrderRepo.GetByIdAsync(OrderSpec);
            if(ExistOrder is not null) OrderRepo.Remove(ExistOrder);
            // Create OrderItem List 
            List<OrderItem> OrderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id)
                  ?? throw new ProductNotFoundException(item.Id);
                OrderItems.Add(CreateOrderItem(item, Product));
            }
            //DeliveryMethod
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId)
                  ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);
            //Calc SubTotal 
            var SubTotal = OrderItems.Sum(I=>I.Quantity*I.Price );
            var Order = new Order(Email, OrderAddress, DeliveryMethod,OrderItems,SubTotal,Basket.paymentIntentId);
            
                await OrderRepo.AddAsync(Order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<Order,OrderToReturnDto>(Order);
        }

        private static OrderItem CreateOrderItem( BasketItem item, Product Product)
        {

            return new OrderItem()
            {
                Product = new ProductItemOrdered() { ProductId = Product.Id, PictureUrl = Product.PictureUrl, ProductName = Product.Name },
                Price = Product.Price,
                Quantity = item.Quantity,

            };
        }

    

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodAsync()
        {
           var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>,IEnumerable<DeliveryMethodDto>>(DeliveryMethod);

        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email)
        {
         var Spec = new OrderSpecification(Email);
            var Orders = await _unitOfWork.GetRepository<Order,Guid>().GetAllAsync(Spec);
            return _mapper.Map<IEnumerable<Order>,IEnumerable<OrderToReturnDto>>(Orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid Id)
        {
            var Spec = new OrderSpecification(Id);
            var Order = await _unitOfWork.GetRepository<Order,Guid>().GetByIdAsync(Spec);
            return _mapper.Map<Order, OrderToReturnDto>(Order);
        }
    }
}
