using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.DTO;
using Repository.Models;
using AutoMapper;

namespace Service
{
    public abstract class BaseService<TEntity, TDto> : IService<TEntity, TDto>
    {
        protected readonly UnitOfWork _unitOfWork;
        public IMapper Mapper { get; set; }


        public BaseService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            var mapConfig = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TDto>());
            Mapper = mapConfig.CreateMapper();
        }

        public abstract TDto GetById(Guid id);

        public abstract List<TDto> Get();

        public abstract void Add(TDto dto);

        public abstract void Update(TDto dto);

        public abstract void Delete(Guid id);

        public virtual TDto ToDto(TEntity entity)
        {
            return Mapper.Map<TDto>(entity);
        }

        public virtual TEntity FromDto(TDto dto)
        {
            return Mapper.Map<TEntity>(dto);
        }
    }
}
