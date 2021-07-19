using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    /// <summary>
    /// Generic repository with CRUD operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        // поиск с опциональным фильтром
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null) 
        {
            IQueryable<TEntity> result = dbSet;

            if (filter != null)
            {
                result = result.Where(filter);
            }

            return result.ToList();

        }

        public virtual TEntity GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity) {
            dbSet.Add(entity);
        }

        public virtual void Update(TEntity oldEntity, TEntity newEntity) {
            // устанавливаем сущности измененные атрибуты
            context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        public virtual void Delete(Guid id) {
            TEntity entity = dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete (TEntity entity)
        {
            dbSet.Remove(entity);
        }
    }
}
