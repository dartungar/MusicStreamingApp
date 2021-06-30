using System;
using System.Collections.Generic;
using Repository;
using AutoMapper;

namespace Service
{
    public interface IService<TEntity, TDto>
    {
        public IMapper MapperToDto { get; set; }
        public IMapper MapperFromDto { get; set; }

        public TDto GetById(Guid id);

        
        public List<TDto> Get();

        //public List<TDto> Get(int offset, int limit);
        //public List<TDto> Get(string query);
        //public List<TDto> Get(int offset, int limit, string query);

        public void Add(TDto dto);

        public void Update(TDto dto);

        public void Delete(Guid id);

        public TDto ToDto(TEntity entity);

        public TEntity FromDto(TDto dto);
    }
}
