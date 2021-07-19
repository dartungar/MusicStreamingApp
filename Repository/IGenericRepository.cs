using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null);

        public TEntity GetById(Guid id);

        public void Insert(TEntity entity);

        public void Update(TEntity oldEntity, TEntity newEntity);

        public void Delete(Guid id);

        public void Delete(TEntity entity);
    }
}
