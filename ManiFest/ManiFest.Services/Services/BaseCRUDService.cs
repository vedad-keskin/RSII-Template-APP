using ManiFest.Services.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ManiFest.Model.Responses;
using ManiFest.Model.Requests;
using ManiFest.Model.SearchObjects;
using System.Linq;
using MapsterMapper;
using ManiFest.Services.Interfaces;

namespace ManiFest.Services.Services
{
    public abstract class BaseCRUDService<T, TSearch, TEntity, TInsert, TUpdate>
    : BaseService<T, TSearch, TEntity>, ICRUDService<T, TSearch, TInsert, TUpdate>
    where T : class where TSearch : BaseSearchObject where TEntity : class, new() where TInsert : class where TUpdate : class
    {
        protected readonly ManiFestDbContext _context;

        public BaseCRUDService(ManiFestDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }


        public virtual async Task<T> CreateAsync(TInsert request)
        {
            var entity = new TEntity();
            MapInsertToEntity(entity, request);
            _context.Set<TEntity>().Add(entity);

            await BeforeInsert(entity, request);

            await _context.SaveChangesAsync();
            return MapToResponse(entity);
        }

        protected virtual async Task BeforeInsert(TEntity entity, TInsert request)
        {

        }


        protected virtual TEntity MapInsertToEntity(TEntity entity, TInsert request)
        {
            return _mapper.Map(request, entity);
        }

        public virtual async Task<T?> UpdateAsync(int id, TUpdate request)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return null;

            await BeforeUpdate(entity, request);

            MapUpdateToEntity(entity, request);

            await _context.SaveChangesAsync();
            return MapToResponse(entity);
        }

        protected virtual async Task BeforeUpdate(TEntity entity, TUpdate request)
        {

        }

        protected virtual void MapUpdateToEntity(TEntity entity, TUpdate request)
        {
            _mapper.Map(request, entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return false;

            await BeforeDelete(entity);

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        protected virtual async Task BeforeDelete(TEntity entity)
        {

        }

    }
}