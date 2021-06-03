using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
    class Tracks
    {


        // track methods
        public List<Track> GetTracks()
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Tracks.ToList();
        }

        public List<Track> GetTracks(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Tracks.Where(t => t.Name == query).ToList();
        }
        // TODO: get tracks by artist
        // TODO: get tracks by album

        public void UpdateTrack(Track track)
        {
            using ApplicationContext db = new ApplicationContext();
            Track foundTrack = db.Tracks.Find(track.Id);
            if (foundTrack != null) foundTrack = track;
            db.SaveChanges();
        }


        protected internal Track AddTrack(ITrackData data, bool saveChanges)
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

            foreach (Guid artistId in data.ArtistIds)
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

        public void RemoveTrack(Track track)
        {
            using ApplicationContext db = new ApplicationContext();
            db.Tracks.Remove(track);
            db.SaveChanges();
        }

        public void RemoveTrack(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Track track = db.Tracks.Find(id);
            if (track != null) db.Tracks.Remove(track);
            db.SaveChanges();
        }
    }
}
