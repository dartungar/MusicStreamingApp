using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using Repository.DTO;
using Microsoft.EntityFrameworkCore;

// Теперь есть GenericRepository. Этот - удалить, после того как перенесу нужные методы в соответствующий Service

namespace Service
{
    public class TrackRepository
    {

        // track methods
        public static TrackDto GetTrack(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return TrackToDto(db.Tracks.AsNoTracking().Where(t => t.Id == id).FirstOrDefault());
        }

        public static List<TrackDto> SearchTracks(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Tracks.AsNoTracking().Where(t => t.Name.Contains(query)).Select(t => TrackToDto(t)).ToList();
        }
        // TODO: get tracks by artist
        // TODO: get tracks by album

        public void UpdateTrack(Guid id, string name, int length)
        {
            using ApplicationContext db = new ApplicationContext();
            Track foundTrack = db.Tracks.Find(id);
            if (foundTrack != null)
            {
                foundTrack.Name = name;
                foundTrack.Length = length;
            }
            db.SaveChanges();
        }

        public void UpdateTrack(TrackDto trackData)
        {
            using ApplicationContext db = new ApplicationContext();
            Track foundTrack = db.Tracks.Find(trackData.Id);
            if (foundTrack != null)
            {
                foundTrack.Name = trackData.Name;
                foundTrack.Length = trackData.Length;
                foundTrack.TrackArtists.Clear();
                List<Artist> newArtists = db.Artists.Where(a => trackData.ArtistsIds.Any(aid => aid == a.Id)).ToList();
                foreach (Artist artist in newArtists)
                {
                    TrackArtist trackArtist = new TrackArtist { Track = foundTrack, Artist = artist };
                    db.TrackArtists.Add(trackArtist);
                }
                db.SaveChanges();
            }
        }


        public static Track AddTrack(TrackDto data, bool saveChanges)
        {
            using ApplicationContext db = new ApplicationContext();

            Track existingTrack = db.Tracks.AsNoTracking().FirstOrDefault(t => t.AlbumId == data.AlbumId && t.Name == data.Name);
            if (existingTrack != null)
                return existingTrack;

            Guid trackId = Guid.NewGuid();

            Track newTrack = new Track
            {
                Id = trackId,
                Name = data.Name,
                Length = data.Length,
                AlbumId = data.AlbumId
            };

            db.Tracks.Add(newTrack);

            foreach (Guid artistId in data.ArtistsIds)
            {
                Artist existingArtist = db.Artists.AsNoTracking().Where(t => t.Id == artistId).FirstOrDefault();
                if (existingArtist == null)
                    throw new Exception($"Ошибка при добавлении трека: исполнитель с ID {artistId} не найден");

                TrackArtist trackArtist = new TrackArtist
                {
                    TrackId = trackId,
                    ArtistId = artistId
                };
                db.TrackArtists.Add(trackArtist);
            }
            if (saveChanges)
                db.SaveChanges();
            return newTrack;
        }

        public static void RemoveTrack(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Track track = db.Tracks.AsNoTracking().Where(t => t.Id == id).FirstOrDefault();
            if (track != null) db.Tracks.Remove(track);
            db.SaveChanges();
        }

        private static TrackDto TrackToDto(Track track)
        {
            return new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Length = track.Length,
                AlbumId = track.AlbumId,
                ArtistsIds = track.TrackArtists.Select(ta => ta.ArtistId).ToList()
            };
        }
    }
}
