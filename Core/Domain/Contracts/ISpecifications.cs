using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity,bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; }
        public Expression<Func<TEntity,object>>? OrderBy { get; set; }
        public Expression<Func<TEntity,object>>? OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }


    }
}
