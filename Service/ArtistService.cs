using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.EF;
using Domain;
using Domain.Models;
using Service.DTO;
using AutoMapper;

namespace Service
{
    public class ArtistService : BaseService<Artist, ArtistDto>
    {
        private readonly IGenericRepository<Artist> _artistRepository;
        public ArtistService(IUnitOfWork unitOfWork, IGenericRepository<Artist> artistRepo) : base(unitOfWork)
        {
            _artistRepository = artistRepo;
            
            var configFromDto = new MapperConfiguration(cfg =>
                cfg.CreateMap<ArtistDto, Artist>()
                // если передан пустой ID, то генерируем новый
                .ForMember(artist => artist.Id, opts => opts.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
            );
            MapperFromDto = configFromDto.CreateMapper();
        }

        public override ArtistDto GetById(Guid id)
        {
            Artist artist = _artistRepository.GetById(id);
            if (artist == null) throw new Exception("Исполнитель не найден");
            return ToDto(artist);
        }

        public override List<ArtistDto> Get(Expression<Func<Artist, bool>> filter = null)
        {
            var artists = (List<Artist>)_artistRepository.Get(filter);
            return artists.Select(a => ToDto(a)).ToList();
        }

        public override void Add(ArtistDto artistDto)
        {
            
            Artist artist = FromDto(artistDto);
            _artistRepository.Insert(artist);
            _unitOfWork.Save();
        }

        public override void Add(Artist artist)
        {
            _artistRepository.Insert(artist);
            _unitOfWork.Save();
        }

        public override void Update(ArtistDto artistDto)
        {
            if (artistDto.Id == Guid.Empty)
                throw new Exception("Для изменения данных исполнителя необходимо указать его ID");

            // кастим Guid? -> Guid, т.к провели проверку на NULL
            // наверное есть более каноничный способ?..
            Artist oldArtist = _artistRepository.GetById(artistDto.Id);
            if (oldArtist == null)
                throw new Exception("Исполнитель не найден");

            Artist newArtist = FromDto(artistDto);

            _artistRepository.Update(oldArtist, newArtist);
            _unitOfWork.Save();
        }


        public void Delete(ArtistDto artistDto)
        {
            if (artistDto.Id != Guid.Empty)
                Delete((Guid)artistDto.Id);
            throw new Exception("Для удаления исполнителя необходимо указать его ID");
           
            
        }

        public override void Delete(Guid id)
        {
            Artist artist = _artistRepository.GetById(id);
            if (artist == null)
                throw new Exception("Исполнитель не найден");
            _artistRepository.Delete(artist);
            _unitOfWork.Save();
        }

    }
}
