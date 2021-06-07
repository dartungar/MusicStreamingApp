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
    public class Tracks
    {

        // track methods
        public static TrackDto GetTrack(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return TrackToDto(db.Tracks.Find(id));
        }

        public static List<TrackDto> GetTracks(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Tracks.Where(t => t.Name == query).Select(t => TrackToDto(t)).ToList();
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


        public static Track AddTrack(TrackDto data, bool saveChanges)
        {
            using ApplicationContext db = new ApplicationContext();

            Track existingTrack = db.Tracks.FirstOrDefault(t => t.AlbumId == data.AlbumId && t.Name == data.Name);
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
                Artist existingArtist = db.Artists.Find(artistId);
                if (existingArtist == null)
                    throw new Exception($"Ошибка при добавлении трека: исполнитель с ID {artistId} не найден");

                TrackArtist trackArtist = new TrackArtist
                {
                    Id = Guid.NewGuid(),
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
            Track track = db.Tracks.Find(id);
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
