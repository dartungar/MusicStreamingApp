using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
    public class Artists
    {
        // artist methods
        public List<Artist> GetArtists()
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Artists.ToList();
        }

        public List<Artist> GetArtists(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Artists.Where(a => a.Name == query).ToList();
        }

        public void UpdateArtist(Artist artist)
        {
            using ApplicationContext db = new ApplicationContext();
            Artist foundArtist = db.Artists.Find(artist.Id);
            if (foundArtist != null) foundArtist = artist;
            db.SaveChanges();
        }

        public Artist AddArtist(string name, string facebookLink, string description, bool isVerified, string imageUrl)
        {
            using ApplicationContext db = new ApplicationContext();
            Artist newArtist = new Artist
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                IsVerified = isVerified,
            };
            newArtist.ArtistImages.Add(
                new ArtistImage { 
                    Id = Guid.NewGuid(), 
                    Image = new Image { 
                        Id = Guid.NewGuid(), 
                        Url = imageUrl } });
            db.Artists.Add(newArtist);
            db.SaveChanges();
            return newArtist;

        }

        public void RemoveArtist(Artist artist)
        {
            using ApplicationContext db = new ApplicationContext();
            db.Artists.Remove(artist);
            db.SaveChanges();
        }

        public void RemoveArtist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Artist artist = db.Artists.Find(id);
            if (artist != null) db.Artists.Remove(artist);
            db.SaveChanges();
        }
        // TODO: update artist description
        // TODO: CRUD artist images
    }
}
