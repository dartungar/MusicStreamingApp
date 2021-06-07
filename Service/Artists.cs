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
    public class Artists
    {
        // artist methods
        public static ArtistDto GetArtists(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return ArtistToDto(db.Artists.Find(id));
        }

        public static List<ArtistDto> GetArtists(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Artists.Where(a => a.Name == query).Select(a => ArtistToDto(a)).ToList();
        }

        public static void UpdateArtist(Guid id, string name, string description, string facebookLink)
        {
            using ApplicationContext db = new ApplicationContext();
            Artist foundArtist = db.Artists.Find(id);
            if (foundArtist != null)
            {
                foundArtist.Name = name;
                foundArtist.Description = description;
                foundArtist.FacebookLink = facebookLink;
            }
            db.SaveChanges();
        }

        public static ArtistDto AddArtist(string name, string facebookLink, string description, bool isVerified, string imageUrl)
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
            return ArtistToDto(newArtist);
        }

        public static void RemoveArtist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Artist artist = db.Artists.Find(id);
            if (artist != null) db.Artists.Remove(artist);
            db.SaveChanges();
        }

        // TODO: update artist description
        // TODO: CRUD artist images

        private static ArtistDto ArtistToDto(Artist artist)
        {
            return new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Description = artist.Description,
                FacebookLink = artist.FacebookLink,
                ImagesIds = artist.ArtistImages.Select(ai => ai.ImageId).ToList()
            };
        }
    }
}
