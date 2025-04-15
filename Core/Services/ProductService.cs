using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork , IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
           var brand =  await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brand);
            return result;
        }

        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParamters specParams)
        {
            var spec = new ProductWithBrandsSpecifications(specParams);
            // Get All Products Througt unitOfWork
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);

            var specCount = new ProductWithCountSpecifications(specParams);
            var count = await unitOfWork.GetRepository<Product,int>().CountAsync(specCount  );
            // Mapping IEnumrable<Product> To IEnumrable<ProductResultDto>
            var result =mapper.Map<IEnumerable<ProductResultDto>>(products);
            return new PaginationResponse<ProductResultDto>(specParams.PageIndex,specParams.PageSize,count,result);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }

        public async Task<ProductResultDto?> GetProductbyIdAsync(int id)
        {
            var spec = new ProductWithBrandsSpecifications(id);  
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null)return null;

            var result= mapper.Map<ProductResultDto>(product);
            return result;

        }
    }
}
