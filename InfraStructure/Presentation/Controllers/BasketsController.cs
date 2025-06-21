using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs.BasketModulesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class BasketsController(IServiceManager _serviceManager) : ApiBaseController
    {

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string id)
        {
            var Basket = await _serviceManager.BasketService.GetBasketAsync(id);
            return Ok(Basket);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket (BasketDto basket)
        {
            var Basket = await _serviceManager.BasketService.CreateOrDeleteBasketAsync(basket);
            return Ok(Basket);
        }
        [HttpDelete("{Key}")]
        public async Task<ActionResult<bool>> DeleteBasket(string Key)
        {
            var Basket = await _serviceManager.BasketService.DeleteBasketAsync(Key);
            return Ok(Basket);
        }

    }
}
