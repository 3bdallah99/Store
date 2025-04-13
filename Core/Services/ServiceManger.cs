using AutoMapper;
using Domain.Contracts;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManger(IUnitOfWork unitOfWork,IMapper mapper) : IServiceManger
    {
        public IProductService productService { get; } =new ProductService(unitOfWork , mapper);

    }
}
