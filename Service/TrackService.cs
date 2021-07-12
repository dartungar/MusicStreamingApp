using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Repository;
using Repository.Models;
using Service.DTO;

namespace Service
{
    public class TrackService: BaseService<Track, TrackDto>
    {
        public TrackService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            var configFromDto = new MapperConfiguration(cfg =>
            {
                // TO DO: продумать создание / изменение треков
                cfg.CreateMap<TrackDto, Track>();

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
            Track track = _unitOfWork.TrackRepository.GetById(id);
            if (track == null) throw new Exception("Трек не найден");
            track.TrackArtists = GetTrackArtists(track.Id);
            track.Album = GetAlbum(track.AlbumId);
            return ToDto(track);
        }

        public override List<TrackDto> Get(Expression<Func<Track, bool>> filter = null)
        {
            var tracks = (List<Track>)_unitOfWork.TrackRepository.Get(filter);
            foreach (var track in tracks)
            {
                track.TrackArtists = GetTrackArtists(track.Id);
                foreach(TrackArtist ta in track.TrackArtists)
                {
                    ta.Artist = GetArtist(ta.ArtistId);
                }
                track.Album = GetAlbum(track.AlbumId);
            }
            return tracks.Select(a => ToDto(a)).ToList();
        }

        // доп. запросы нужны, т.к пока не понял, как совместить Eager loading и generic-репозиторий
        private Album GetAlbum(Guid albumId)
        {
            return _unitOfWork.AlbumRepository.Get(alb => alb.Id == albumId).FirstOrDefault();
        }

        private List<TrackArtist> GetTrackArtists(Guid trackId)
        {
            return _unitOfWork.TrackArtistRepository.Get(tra => tra.TrackId == trackId).ToList();
        }

        private Artist GetArtist(Guid id)
        {
            return _unitOfWork.ArtistRepository.Get(a => a.Id == id).FirstOrDefault();

        }

        public override void Add(TrackDto TrackDto)
        {
            Track Track = FromDto(TrackDto);
            _unitOfWork.TrackRepository.Insert(Track);
            _unitOfWork.Save();
        }

        public override void Add(Track Track)
        {
            _unitOfWork.TrackRepository.Insert(Track);
            _unitOfWork.Save();
        }

        public override void Update(TrackDto TrackDto)
        {
            if (TrackDto.Id == null)
                throw new Exception("Для изменения данных трека необходимо указать его ID");

            // кастим Guid? -> Guid, т.к провели проверку на NULL
            // наверное есть более каноничный способ?..
            Track oldTrack = _unitOfWork.TrackRepository.GetById((Guid)TrackDto.Id);
            if (oldTrack == null)
                throw new Exception("Исполнитель не найден");

            Track newTrack = FromDto(TrackDto);

            _unitOfWork.TrackRepository.Update(oldTrack, newTrack);
            _unitOfWork.Save();
        }


        public void Delete(TrackDto TrackDto)
        {
            if (TrackDto.Id != null)
                Delete((Guid)TrackDto.Id);
            throw new Exception("Для удаления трека необходимо указать его ID");
        }

        public override void Delete(Guid id)
        {
            Track Track = _unitOfWork.TrackRepository.GetById(id);
            if (Track == null)
                throw new Exception("Трек не найден");
            _unitOfWork.TrackRepository.Delete(Track);
            _unitOfWork.Save();
        }

        public override Track FromDto(TrackDto TrackDto)
        {
            try
            {
                Track Track = MapperFromDto.Map<Track>(TrackDto);
                // TrackDto.Guid? == null бывает в случаях, когда, например, приходят данные для создания нового исполнителя
                Track.Id = TrackDto.Id ?? Guid.NewGuid();
                return Track;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
