using System;
using System.Collections.Generic;
using System.Linq;
using Repository;
using Repository.Models;
using Service.DTO;

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
            var artists = (List<Artist>)_unitOfWork.ArtistRepository.Get();
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
            if (artistDto.Id == null)
                throw new Exception("Для изменения данных исполнителя необходимо указать его ID");

            // кастим Guid? -> Guid, т.к провели проверку на NULL
            // наверное есть более каноничный способ?..
            Artist oldArtist = _unitOfWork.ArtistRepository.GetById((Guid)artistDto.Id);
            if (oldArtist == null)
                throw new Exception("Исполнитель не найден");

            Artist newArtist = FromDto(artistDto);

            _unitOfWork.ArtistRepository.Update(oldArtist, newArtist);
            _unitOfWork.Save();
        }


        public void Delete(ArtistDto artistDto)
        {
            if (artistDto.Id != null)
                Delete((Guid)artistDto.Id);
            throw new Exception("Для удаления исполнителя необходимо указать его ID");
           
            
        }

        public override void Delete(Guid id)
        {
            Artist artist = _unitOfWork.ArtistRepository.GetById(id);
            if (artist == null)
                throw new Exception("Исполнитель не найден");
            _unitOfWork.ArtistRepository.Delete(artist);
            _unitOfWork.Save();
        }

        public override Artist FromDto(ArtistDto artistDto)
        {
            try
            {
                Artist artist = MapperFromDto.Map<Artist>(artistDto);
                // ArtistDto.Guid? == null бывает в случаях, когда, например, приходят данные для создания нового исполнителя
                artist.Id = artistDto.Id == null ? Guid.NewGuid() : (Guid)artistDto.Id;
                return artist;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
