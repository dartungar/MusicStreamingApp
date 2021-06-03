using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
    public class Albums
    {

        // album methods
        public List<Album> GetAlbums()
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Albums.ToList();
        }

        public List<Album> GetAlbums(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Albums.Where(a => a.Name == query).ToList();
        }
        // TODO: get albums by artist

        public void UpdateAlbum(Album album)
        {
            using ApplicationContext db = new ApplicationContext();
            Album foundAlbum = db.Albums.Find(album.Id);
            if (foundAlbum != null) foundAlbum = album;
            db.SaveChanges();
        }



        // TODO
        public Album AddAlbumWithTracks(string name, string type, DateTime dateReleased, List<ITrackData> tracksData, string imageUrl) {
            using ApplicationContext db = new ApplicationContext(); // в вызываемых функциях свои контексты; это плохо?
            
            // проверка на то, существует ли альбом с таким именем и таким набором исполнителей
            // это единственный более-менее надежный способ проверить "уникальность" альбома
            // 1. Собираем набор исполнителей из треков, которые собираемся добавить в альбом
            List<Guid> tracksDataArtists = tracksData.SelectMany(trackData => trackData.ArtistIds).ToList();
            // 2. ищем альбомы с таким же именем
            List<Album> albumsWithSameName = db.Albums.Where(a => a.Name == name).ToList();
            // 3. проверяем, совпадает ли набор исполнителей с треков этого альбома
            // с набором исполнителей из треков, которые мы хотим добавить 
            if (albumsWithSameName.Count > 0)
            {
                foreach (Album albumWithSameName in albumsWithSameName)
                {
                    List<Guid> albumWithSameNameArtists = (from track in db.Tracks
                                                           join trackArtist in db.TrackArtists on track.Id equals trackArtist.TrackId
                                                           where track.AlbumId == albumWithSameName.Id
                                                           select trackArtist.ArtistId).ToList();
                    if (albumWithSameNameArtists.Count > 0 && albumWithSameNameArtists.Intersect(tracksDataArtists).Count() == albumWithSameNameArtists.Count)
                        throw new Exception($"Альбом с названием {name} и исполнителем(-ями) {albumWithSameNameArtists.Count} уже существует");
                }
            }


            // пронесло! продолжаем создание альбома.

            // создаем и сохраняем в БД альбом
            Album album = AddAlbum(name, type, dateReleased, imageUrl, true);
            Tracks trackService = new Tracks();
            List<Track> createdTracks = new List<Track>();
            // создаем и сохраняем в БД треки
            // можно было бы экономичнее (сначала добавить в контекст, потом сохранить всё разом)
            // но т.к операция нечастая и структура не элементарная (см. Track & TrackArtist)
            // пока делаем так
            foreach (ITrackData trackData in tracksData)
            {
                trackData.AlbumId = album.Id;
                createdTracks.Add(trackService.AddTrack(trackData, true));
            }

            return album;
        }

        // TODO: понять, как обеспечить уникальность альбомов в рамках исполнителя
        // с учетом того что исполнители альбома связаны с альбомом только через треки
        // а треки нельзя создать, не создав альбом
        // возможно, всё-таки нужна таблица AlbumArtist
        private Album AddAlbum(string name, string type, DateTime dateReleased, string imageUrl, bool saveChanges)
        {
            using ApplicationContext db = new ApplicationContext();
            // проверяем тип альбома
            // нужен Enum
            AlbumType albumType = db.AlbumTypes.FirstOrDefault(at => at.Name == type);
            if (albumType == null)
                throw new Exception("Неправильный тип альбома");
            // TODO: надежная проверка
            Album existingAlbum = db.Albums.FirstOrDefault(a => a.Name == name && a.Date == dateReleased);
            Album newAlbum = new Album
            {
                Id = Guid.NewGuid(),
                Name = name,
                Date = dateReleased,
                AlbumTypeId = albumType.Id,
                Image = new Image { Id = Guid.NewGuid(), Url = imageUrl}
            };
            db.Albums.Add(newAlbum);
            if (saveChanges)
                db.SaveChanges();
            return newAlbum;

        }
        // TODO: add album with artist name
        // TODO: add album image

        public void RemoveAlbum(Album album)
        {
            using ApplicationContext db = new ApplicationContext();
            db.Albums.Remove(album);
            db.SaveChanges();
        }

        public void RemoveAlbum(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Album album = db.Albums.Find(id);
            if (album != null) db.Albums.Remove(album);
            db.SaveChanges();
        }
    }
}
