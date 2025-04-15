using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithCountSpecifications : BaseSpecification<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecificationsParamters specParams) 
            : base(
                  P =>
                  (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search.ToLower())) &&
                        (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId) &&
                        (!specParams.TypeId.HasValue || P.TypeId == specParams.TypeId)
                  )
        {
        }
    }
}
