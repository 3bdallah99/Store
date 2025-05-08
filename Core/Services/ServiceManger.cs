using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManger(
                IUnitOfWork unitOfWork,
                IMapper mapper, 
                IBasketRepository basketRepository,
                ICashRepository cashRepository,
                UserManager<AppUser> userManager,
                IOptions<JwtOptions> options,
                IConfiguration configuration
                ) : IServiceManger
    {

        public IProductService productService { get; } =new ProductService(unitOfWork , mapper);
        public IBasketService basketService { get; } = new BasketService(basketRepository, mapper);
        public ICashService cashService { get; } = new CashService(cashRepository);

        public IAuthenticationService authenticationService { get; } = new AuthenticationService(userManager, options , mapper);

        public IOrderService orderService { get; } = new OrderService(mapper,basketRepository,unitOfWork);

		public IPaymentService paymentService { get; } = new PaymentService(basketRepository,unitOfWork,mapper,configuration);
	}
}
