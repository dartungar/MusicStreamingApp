using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Repository;
using AutoMapper;

namespace Service
{
    public abstract class BaseService<TEntity, TDto> : IService<TEntity, TDto>
    {
        protected readonly UnitOfWork _unitOfWork;
        public IMapper MapperToDto { get; set; }
        public IMapper MapperFromDto { get; set; }


        public BaseService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var mapConfigToDto = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TDto>());
            MapperToDto = mapConfigToDto.CreateMapper();
            var mapConfigFromDto = new MapperConfiguration(cfg => cfg.CreateMap<TDto, TEntity>());
            MapperFromDto = mapConfigFromDto.CreateMapper();
        }

        public abstract TDto GetById(Guid id);

        public abstract List<TDto> Get(Expression<Func<TEntity, bool>> filter = null);

        public abstract void Add(TDto dto);

        public abstract void Add(TEntity entity);

        public abstract void Update(TDto dto);

        public abstract void Delete(Guid id);

        public virtual TDto ToDto(TEntity entity)
        {
            return MapperToDto.Map<TDto>(entity);
        }

        public virtual TEntity FromDto(TDto dto)
        {
            return MapperFromDto.Map<TEntity>(dto);
        }
    }
}
