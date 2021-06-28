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
    public class GenericRepository<TEntity> where TEntity : class
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
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                {
                    query = query.Where(filter);
                }

            return query.ToList();

        }

        public virtual TEntity GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity) {
            dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity) {
            // добавляем измененную сущность в контекст
            dbSet.Attach(entity);
            // указываем, что она изменилась
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(Guid id) {
            TEntity entity = dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete (TEntity entity)
        {
            // проверяем, добавлена ли сущность в контекст; если нет - добавляем
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }
    }
}
