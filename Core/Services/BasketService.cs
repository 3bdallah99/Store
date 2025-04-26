using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BasketService(IBasketRepository basketRepository , IMapper mapper) : IBasketService
    {
        public async Task<BasketDto>? GetBasketAsync(string id)
        {
            var basket =await basketRepository.GetBasketAsync(id);
            if (basket == null) throw new BasketNotFoundException(id);
            var result = mapper.Map<BasketDto>(basket);
            return result;
        }
        public async Task<BasketDto>? UpdateBasketAsync(BasketDto basketDto)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basketDto);
            var basket = await basketRepository.UpdateBasketAsync(customerBasket);
            if (basket == null) throw new BasketAddOrUpdateBadRequestException();
            var result = mapper.Map<BasketDto>(basket);
            return  result;
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            var flag = await basketRepository.DeleteBasketAsync(id);
            if (!flag) throw new BasketDeleteBadRequest();
            return flag;
        }

        
    }
}
