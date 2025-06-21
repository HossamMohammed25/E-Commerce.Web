using Shared.DTOs.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        Task<OrderToReturnDto> CreateOrderAsync (OrderDto orderDto,string Email);
        //Get Delivery Methods
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodAsync();
        // Get All Orders
        Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email);
        //Get Order By Id 
        Task<OrderToReturnDto> GetOrderByIdAsync(Guid Id);
        
    }
}
