using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.DTOs.BasketModulesDtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Domain.Models.ProductModule.Product;
namespace Service
{
    public class PaymentService(IConfiguration _configuration,IBasketRepository _basketRepository
        ,IUnitOfWork _unitOfWork,
        IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            // install Package stripe.net
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            //get Basket by BasketId 
            var Basket = await _basketRepository.GetBasketAsync(BasketId)?? throw new BasketNotFoundException(BasketId);
            // Get Amount  Get  Product + Delivery Method
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach(var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id)?? throw new ProductNotFoundException(item.Id);
                item.Price = Product.Price;

            }
            ArgumentNullException.ThrowIfNull(Basket.deliveryMethodId);
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod,int>().GetByIdAsync(Basket.deliveryMethodId.Value) ?? throw new DeliveryMethodNotFoundException(Basket.deliveryMethodId.Value);
            Basket.shippingPrice = DeliveryMethod.Price;
            var BasketAmout = (long)(Basket.Items.Sum(item => item.Quantity * item.Price) + DeliveryMethod.Price)*100;

            //Create Payment Inetnt (Create / Update )
            var PaymentService = new PaymentIntentService();
            if (Basket.paymentIntentId is null)//Create
            {
                var Options = new PaymentIntentCreateOptions ()
                {
                    Amount= BasketAmout,
                    Currency = "USD",
                    PaymentMethodTypes= ["card"]
                };
             var PaymentIntent=  await PaymentService.CreateAsync(Options);
                Basket.paymentIntentId = PaymentIntent.Id;
                Basket.clientSecret = PaymentIntent.ClientSecret;

            }
            else //Update
            {
                var Options = new PaymentIntentUpdateOptions() { Amount = BasketAmout };
               await PaymentService.UpdateAsync(Basket.paymentIntentId, Options);
            }
            await _basketRepository.CreatedOrUpdatedBasketAsync(Basket);

            return _mapper.Map<BasketDto>(Basket);
        }
    }
}
