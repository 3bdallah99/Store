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
    public class ProductWithBrandsSpecifications : BaseSpecification<Product, int>
    {
        public ProductWithBrandsSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        public ProductWithBrandsSpecifications(ProductSpecificationsParamters specParams) 
            :base(P => 
                        (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search.ToLower()))&&
                        (! specParams.BrandId.HasValue || P.BrandId == specParams.BrandId) &&
                        (!specParams.TypeId.HasValue || P.TypeId == specParams.TypeId)
                        )
        {
            ApplyIncludes();
            ApplySorting(specParams.Sort);
            ApplyPagination(specParams.PageIndex, specParams.PageSize);
        }
        private void ApplyIncludes()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
        private void ApplySorting(string? sort)
        {
            if (string.IsNullOrEmpty(sort))
            {
                switch (sort?.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(P => P.Name);
                        break;
                    case "namedesc":
                        AddOrderByDesc(P => P.Name);
                        break;
                    case "Priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "Pricedesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }
    }
}
