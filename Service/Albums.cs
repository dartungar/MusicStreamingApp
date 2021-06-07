using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using Repository.DTO;

namespace Service
{
    public class Albums
    {
        public static AlbumDto GetAlbum(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return AlbumToDto(db.Albums.Find(id));
        }

        public static List<AlbumDto> GetAlbums(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Albums.Where(a => a.Name == query).Select(a => AlbumToDto(a)).ToList();
        }

        public static List<AlbumDto> GetAlbumsByArtist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            List<Guid> albumIds = (from track in db.Tracks
                                   join trackArtist in db.TrackArtists on track.Id equals trackArtist.TrackId
                                   where trackArtist.ArtistId == id
                                   select track.AlbumId).ToList();
            List<AlbumDto> albums = db.Albums.Where(
                a => albumIds.Any(aid => aid == a.Id))
                .Select(a => AlbumToDto(a)).ToList();
            return albums;
        }


        internal static List<Guid> GetAlbumsArtistsIds(Guid albumId)
        {
            using ApplicationContext db = new ApplicationContext();
            Album album = db.Albums.Find(albumId);
            List<Guid> artistsIds = (from track in db.Tracks
                                     join trackArtist in db.TrackArtists on track.Id equals trackArtist.TrackId
                                     where track.AlbumId == albumId
                                     select trackArtist.ArtistId).ToList();
            return artistsIds;
        }

        public static void UpdateAlbum(Guid id, string newName)
        {
            using ApplicationContext db = new ApplicationContext();
            Album foundAlbum = db.Albums.Find(id);
            if (foundAlbum != null) foundAlbum.Name = newName;
            db.SaveChanges();
        }


        public static AlbumDto AddAlbumWithTracks(string name, string type, DateTime dateReleased, List<TrackDto> tracksData, string imageUrl) {
            using ApplicationContext db = new ApplicationContext(); // в вызываемых функциях свои контексты; это плохо?
            
            // проверка на то, существует ли альбом с таким именем и таким набором исполнителей
            // это единственный более-менее надежный способ проверить "уникальность" альбома без связки "альбом-исполнитель"

            // 1. Собираем набор исполнителей из треков, которые собираемся добавить в альбом
            List<Guid> tracksDataArtists = tracksData.SelectMany(trackData => trackData.ArtistsIds).ToList();

            // 2. ищем альбомы с таким же именем
            List<Album> albumsWithSameName = db.Albums.Where(a => a.Name == name).ToList();

            // 3. проверяем, совпадает ли набор исполнителей с треков этого альбома
            // с набором исполнителей из треков, которые мы хотим добавить 
            if (albumsWithSameName.Count > 0)
            {
                foreach (Album albumWithSameName in albumsWithSameName)
                {
                    List<Guid> albumWithSameNameArtists = GetAlbumsArtistsIds(albumWithSameName.Id);
                    if (albumWithSameNameArtists.Count > 0 && albumWithSameNameArtists.Intersect(tracksDataArtists).Count() == albumWithSameNameArtists.Count)
                        throw new Exception($"Альбом с названием {name} и исполнителем(-ями) уже существует"); // TODO: хотя бы перечислять GUIDы
                }
            }

            // продолжаем создание альбома.

            // создаем и сохраняем в БД альбом
            Album album = AddAlbum(name, type, dateReleased, imageUrl, true);
            List<Track> createdTracks = new List<Track>();
            // создаем и сохраняем в БД треки
            // можно было бы экономичнее (сначала добавить в контекст, потом сохранить всё разом)
            // но т.к операция нечастая и структура не элементарная (см. Track & TrackArtist)
            // пока делаем так
            foreach (TrackDto trackData in tracksData)
            {
                trackData.AlbumId = album.Id;
                createdTracks.Add(Tracks.AddTrack(trackData, true));
            }

            return new AlbumDto { Id = album.Id, };
        }


        private static Album AddAlbum(string name, string type, DateTime dateReleased, string imageUrl, bool saveChanges)
        {
            using ApplicationContext db = new ApplicationContext();
            // проверяем тип альбома
            // нужен Enum?
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

        // TODO: add album image

        public static void RemoveAlbum(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Album album = db.Albums.Find(id);
            if (album != null) db.Albums.Remove(album);
            db.SaveChanges();
        }

        // TODO: снизить число запросов к БД
        private static AlbumDto AlbumToDto(Album album)
        {
            return new AlbumDto
            {
                Id = album.Id,
                Name = album.Name,
                Image = new ImageDto { Id = album.Image.Id, Url = album.Image.Url },
                TracksList = album.Tracks.Select(t => new TrackDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Length = t.Length,
                    ArtistsIds = t.TrackArtists.Select(ta => ta.ArtistId).ToList(),
                    AlbumId = album.Id
                }).ToList(),
                Artists = GetAlbumsArtistsIds(album.Id)
            };
        }
    }
}
