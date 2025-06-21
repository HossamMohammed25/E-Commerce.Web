using Domain.Models.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string Key);
        public Task<CustomerBasket?> CreatedOrUpdatedBasketAsync(CustomerBasket basket,TimeSpan? TimeToLive = null);
        public Task<bool> DeleteBasketAsync(string Id);
    }
}
