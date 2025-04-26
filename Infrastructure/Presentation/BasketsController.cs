using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManger serviceManger) : ControllerBase 
    {
        [HttpGet] // GET : /api/baskets?id=sg
        public async Task<IActionResult> GetBasketByID(string id)
        {
            var result = await serviceManger.basketService.GetBasketAsync(id);
            return Ok(result);
        }
        [HttpPost] // POST /api/
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var result = await serviceManger.basketService.UpdateBasketAsync(basketDto);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await serviceManger.basketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
