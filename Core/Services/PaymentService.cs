using Domain.Contracts;
using Domain.Exceptions;
using productM= Domain.Models.Product;
using Domain.Models.OrderModels;
using Services.Abstractions;
using Shared;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Services
{
	public class PaymentService(IBasketRepository basketRepository ,
								IUnitOfWork unitOfWork
								,IMapper mapper
								,IConfiguration configuration) : IPaymentService
	{
		public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
		{
			var basket = await basketRepository.GetBasketAsync(basketId)
				?? throw new Domain.Exceptions.BasketNotFoundException(basketId);
            foreach (var item in basket.Items)
            {
				var product = await unitOfWork.GetRepository<productM, int>().GetAsync(item.Id)
					?? throw new ProductNotFoundException(item.Id);
				item.Price = product.Price;

            }
			if (!basket.DeliveryMethodId.HasValue) throw new Exception("Invalid Delivery Mthod Id !!");
			var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value)
				?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
			basket.ShippingPrice = deliveryMethod.Cost;
				
			var amount =(long) (basket.Items.Sum(I => I.Quantity * I.Price) +basket.ShippingPrice) *100 ;
			StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
			var service = new PaymentIntentService();
			
			if (string.IsNullOrEmpty(basket.PaymentIntentId))
			{
				// Create
				var cerateOptions = new PaymentIntentCreateOptions()
				{
					Amount = amount,
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card"}
				};
				var paymentIntent = await service.CreateAsync(cerateOptions);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else
			{
				//Update
				var updateOptions = new PaymentIntentUpdateOptions()
				{
					Amount = amount,
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }
				};
				var paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,updateOptions);
			}
			await basketRepository.UpdateBasketAsync(basket);
			var result = mapper.Map<BasketDto>(basket);
			return result;
		}
	}
}
