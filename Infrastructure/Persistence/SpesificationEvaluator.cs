using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class SpesificationEvaluator  
    {
        public static IQueryable<TEntity> GetQuery<TEntity,TKey>(
            IQueryable<TEntity> inputQuery,
            ISpecifications<TEntity,TKey> specifications
            )
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            if ( specifications.Criteria != null ) 
                query =  query.Where(specifications.Criteria);
            if (specifications.OrderBy != null )
                query = query.OrderBy(specifications.OrderBy);
            else if (specifications.OrderByDesc != null )
                query = query.OrderByDescending(specifications.OrderByDesc);
            if (specifications.IsPagination)
                query = query.Skip(specifications.Skip).Take(specifications.Take);
           query =  specifications.IncludeExpressions.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
