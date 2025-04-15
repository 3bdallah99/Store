using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
             await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return trackChanges ? 
                 await  _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync() as IEnumerable<TEntity>
               : await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }
                return trackChanges? 
                  await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }


        public async Task<TEntity?> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).FirstOrDefaultAsync(P => P.Id == id as int?) as TEntity;

            }
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec, bool trackChanges = false)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<TEntity?> GetAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity,TKey> spec)
        {
            return SpesificationEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
    }
}
