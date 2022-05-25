using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using DAL.EF;
using Domain.Models;
using Service.DTO;

namespace Service
{
    public class TrackService: BaseService<Track, TrackDto>
    {
        private readonly TrackRepository _trackRepository;

        public TrackService() : this(new UnitOfWork()) { }

        public TrackService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _trackRepository = new TrackRepository(unitOfWork.Context);
            
            var configFromDto = new MapperConfiguration(cfg =>
            {
                // TO DO: продумать создание / изменение треков
                cfg.CreateMap<TrackDto, Track>()
                  // если передан пустой ID, то генерируем новый
                  .ForMember(
                    track => track.Id, opts => opts.MapFrom(
                        src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id));

            });
            MapperFromDto = configFromDto.CreateMapper();

            var configToDto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Track, TrackDto>()

                 .ForMember(trackDto => trackDto.ArtistsIds,
                    opt => opt.MapFrom(
                        track => track.TrackArtists.Select(tra => tra.ArtistId).ToList()))
                 .ForMember(trackDto => trackDto.ArtistsNames,
                    opt => opt.MapFrom(
                        track => track.TrackArtists.Select(tra => tra.Artist.Name))) // каждый раз много запросов. надо придумать, как оптимизировать
                 .ForMember(trackDto => trackDto.AlbumName,
                    opt => opt.MapFrom(
                        track => track.Album.Name));

            });
            MapperToDto = configToDto.CreateMapper();
        }

        public override TrackDto GetById(Guid id)
        {
            Track track = _trackRepository.GetById(id);
            if (track == null) throw new Exception("Трек не найден");
/*            track.TrackArtists = GetTrackArtists(track.Id);
            track.Album = GetAlbum(track.AlbumId);*/
            return ToDto(track);
        }

        public override List<TrackDto> Get(Expression<Func<Track, bool>> filter = null)
        {
            var tracks = _trackRepository.Get(filter).ToList();
            return tracks.Select(ToDto).ToList();
        }

        public override void Add(TrackDto TrackDto)
        {
            Track Track = FromDto(TrackDto);
            _trackRepository.Insert(Track);
            _unitOfWork.Save();
        }

        public override void Add(Track Track)
        {
            _trackRepository.Insert(Track);
            _unitOfWork.Save();
        }

        public override void Update(TrackDto TrackDto)
        {
            if (TrackDto.Id == Guid.Empty)
                throw new Exception("Для изменения данных трека необходимо указать его ID");

            // кастим Guid? -> Guid, т.к провели проверку на NULL
            // наверное есть более каноничный способ?..
            Track oldTrack = _trackRepository.GetById((Guid)TrackDto.Id);
            if (oldTrack == null)
                throw new Exception("Исполнитель не найден");

            Track newTrack = FromDto(TrackDto);

            _trackRepository.Update(oldTrack, newTrack);
            _unitOfWork.Save();
        }


        public void Delete(TrackDto TrackDto)
        {
            if (TrackDto.Id != Guid.Empty)
                Delete((Guid)TrackDto.Id);
            throw new Exception("Для удаления трека необходимо указать его ID");
        }

        public override void Delete(Guid id)
        {
            Track Track = _trackRepository.GetById(id);
            if (Track == null)
                throw new Exception("Трек не найден");
            _trackRepository.Delete(Track);
            _unitOfWork.Save();
        }
    }
}
