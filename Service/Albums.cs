using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using Repository.DTO;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class Albums
    {
        public static AlbumDto GetAlbum(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return AlbumToDto(db.Albums.AsNoTracking().Where(a => a.Id == id).FirstOrDefault());
        }

        public static List<AlbumDto> SearchAlbums(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Albums.AsNoTracking().Where(a => a.Name.Contains(query)).Select(a => AlbumToDto(a)).ToList();
        }

        public static List<AlbumDto> GetAlbumsByArtist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            List<Guid> albumIds = (from track in db.Tracks
                                   join trackArtist in db.TrackArtists on track.Id equals trackArtist.TrackId
                                   where trackArtist.ArtistId == id
                                   select track.AlbumId).ToList();
            List<AlbumDto> albums = db.Albums.AsNoTracking().Where(
                a => albumIds.Any(aid => aid == a.Id))
                .Select(a => AlbumToDto(a)).ToList();
            return albums;
        }


        internal static List<Guid> GetAlbumsArtistsIds(Guid albumId)
        {
            using ApplicationContext db = new ApplicationContext();
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

        /// <summary>
        /// Добавить альбом и треки в нём.
        /// Создает алтбом, треки и отношение.
        /// При неудаче откатывает все созданные сущности (пока - грубым удалением уже созданного)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="dateReleased"></param>
        /// <param name="tracksData">Список треков</param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static AlbumDto AddAlbumWithTracks(string name, string type, DateTime dateReleased, List<TrackDto> tracksData, string imageUrl) {
            using ApplicationContext db = new ApplicationContext(); // в вызываемых функциях свои контексты; это плохо?
            
            // проверка на то, существует ли альбом с таким же именем и набором исполнителей
            // это единственный более-менее надежный способ проверить "уникальность" альбома без связки "альбом-исполнитель"

            // 1. Собираем набор исполнителей из треков, которые собираемся добавить в альбом
            List<Guid> tracksDataArtists = tracksData.SelectMany(trackData => trackData.ArtistsIds).ToList();

            // 2. ищем альбомы с таким же именем
            List<Album> albumsWithSameName = db.Albums.AsNoTracking().Where(a => a.Name == name).ToList();

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

            // создаем и сохраняем в БД треки
            // можно экономичнее (сначала добавить в контекст, потом сохранить всё разом)
            // но т.к операция нечастая и структура не элементарная (см. Track & TrackArtist)
            // пока делаем так
            try
            {
                foreach (TrackDto trackData in tracksData)
                {
                    trackData.AlbumId = album.Id;
                    Tracks.AddTrack(trackData, true);
                }                
            }
            catch (Exception ex)
            {
                foreach (TrackDto trackData in tracksData)
                {
                    Track trackToDelete = db.Tracks.Find(trackData.Id);
                    if (trackToDelete != null) db.Tracks.Remove(trackToDelete);
                }
                RemoveAlbum(album.Id);
                throw new Exception($"Ошибка при добавлении треков в альбом: {ex.InnerException.Message}");
            } 
            finally
            {
                db.SaveChanges();
            }


            return new AlbumDto { Id = album.Id, Name = album.Name};
        }


        private static Album AddAlbum(string name, string type, DateTime dateReleased, string imageUrl, bool saveChanges)
        {
            using ApplicationContext db = new ApplicationContext();
            // проверяем тип альбома
            // TODO: Enum?
            AlbumType albumType = db.AlbumTypes.AsNoTracking().FirstOrDefault(at => at.Name == type);
            if (albumType == null)
                throw new Exception("Неправильный тип альбома");

            // TODO: более надежная проверка
            Album existingAlbum = db.Albums.AsNoTracking().FirstOrDefault(a => a.Name == name && a.Date == dateReleased && a.AlbumType.Name == type);
            if (existingAlbum != null)
                throw new Exception($"Альбом с названием {name}, вышедший {dateReleased.ToString()}, уже существует");

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
        public void UpdateAlbumImage(Guid albumId, string imageUrl)
        {
            using ApplicationContext db = new ApplicationContext();
            Album album = db.Albums.Find(albumId);

            if (album != null)
                if (album.Image != null)
                    album.Image = new Image { Id = Guid.NewGuid(), Url = imageUrl };
                else
                    album.Image.Url = imageUrl;
            else throw new Exception($"Альбом с ID {albumId} не найден");
            db.SaveChanges();
        }

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
