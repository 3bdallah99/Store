﻿using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get  ; set; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>>? OrderBy { get ; set ; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get ; set ; }
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPagination { get ; set ; }

        public BaseSpecification(Expression<Func<TEntity, bool>>? expression)
        {
             Criteria = expression;
        }
        protected void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> expression)
        {
            OrderByDesc = expression;
        }
        protected void ApplyPagination(int pageIndex , int pageSize)
        {
            IsPagination = true;
            Take = pageSize; 
            Skip = (pageIndex -1 ) * pageSize;

        }
    }
}
