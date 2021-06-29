using System;
using System.Collections.Generic;
using System.Linq;
using Repository;
using Repository.Models;
using Repository.DTO;

namespace Service
{
    public class ArtistService : BaseService<Artist, ArtistDto>
    {

        public ArtistService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override ArtistDto GetById(Guid id)
        {
            Artist artist = _unitOfWork.ArtistRepository.GetById(id);
            if (artist == null) throw new Exception("Исполнитель не найден");
            return ToDto(artist);
        }

        public override List<ArtistDto> Get()
        {
            var artists = (List<Artist>)_unitOfWork.AddressRepository.Get();
            return artists.Select(a => ToDto(a)).ToList();
        }

        public override void Add(ArtistDto artistDto)
        {
            Artist artist = FromDto(artistDto);
            _unitOfWork.ArtistRepository.Insert(artist);
            _unitOfWork.Save();
        }

        public override void Update(ArtistDto artistDto)
        {
            if (artistDto.Id == Guid.Empty)
                throw new Exception("Для изменения данных исполнителя необходимо указать его ID");
            if (_unitOfWork.ArtistRepository.GetById(artistDto.Id) == null)
                throw new Exception("Исполнитель не найден");

            _unitOfWork.ArtistRepository.Update(FromDto(artistDto));
            _unitOfWork.Save();
        }


        public void Delete(ArtistDto artistDto)
        {
            if (artistDto.Id == Guid.Empty)
                throw new Exception("Для удаления исполнителя необходимо указать его ID");
            Delete(artistDto.Id);
        }

        public override void Delete(Guid id)
        {
            Artist artist = _unitOfWork.ArtistRepository.GetById(id);
            if (artist == null)
                throw new Exception("Исполнитель не найден");
            _unitOfWork.ArtistRepository.Delete(artist);
        }

        public override ArtistDto ToDto(Artist artist)
        {
            return Mapper.Map<ArtistDto>(artist);
        }

        // TODO: проверить маппинг
        // особенно поведение с ID (что если Guid.Empty? Guid.Empty - ошибка проектирования DTO?)
        public override Artist FromDto(ArtistDto artistDto)
        {
            return Mapper.Map<Artist>(artistDto);
        }
    }
}
