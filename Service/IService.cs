using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Repository;
using AutoMapper;

namespace Service
{
    public interface IService<TEntity, TDto>
    {
        public IMapper MapperToDto { get; set; }
        public IMapper MapperFromDto { get; set; }

        public TDto GetById(Guid id);

        
        public List<TDto> Get(Expression<Func<TEntity, bool>> filter = null);
        //public List<TDto> Get(int offset, int limit);

        public void Add(TDto dto);

        public void Add(TEntity entity);

        public void Update(TDto dto);

        public void Delete(Guid id);

        public TDto ToDto(TEntity entity);

        public TEntity FromDto(TDto dto);
    }
}
