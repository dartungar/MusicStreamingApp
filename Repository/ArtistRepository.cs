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
    public class ArtistRepository
    {
        // artist methods
        public static ArtistDto GetArtist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return ArtistToDto(db.Artists.AsNoTracking().Where(a => a.Id ==id).FirstOrDefault());
        }

        public static List<ArtistDto> GetArtistsByName(string name)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Artists.AsNoTracking().Where(a => a.Name == name).Select(a => ArtistToDto(a)).ToList();
        }

        public static List<ArtistDto> SearchArtists(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Artists.AsNoTracking().Where(a => a.Name.Contains(query)).Select(a => ArtistToDto(a)).ToList();
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
                Name = name,
                Description = description,
                FacebookLink = facebookLink,
                IsVerified = isVerified,
            };
            newArtist.ArtistImages.Add(
                new ArtistImage { 
                    Image = new Image { 
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

        public static void AddArtistImage(Guid artistId, string imageUrl)
        {
            using ApplicationContext db = new ApplicationContext();
            Artist artist = db.Artists.Find(artistId);
            if (artist == null) throw new Exception($"Исполнителя с ID {artistId} не найдено");
            Image imageWithSameUrl = db.Images.AsNoTracking().Where(i => i.Url == imageUrl).FirstOrDefault();
            if (imageWithSameUrl != null) throw new Exception($"У исполнителя {artist.Name} уже существует изображение с URL {imageWithSameUrl.Url}");
            Image newImage = new Image { Url = imageUrl };
            ArtistImage artistImage = new ArtistImage { ArtistId = artist.Id, ImageId = newImage.Id };
            db.Images.Add(newImage);
            db.ArtistImages.Add(artistImage);
            db.SaveChanges();

        }

        public static void RemoveArtistImage(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Image image = db.Images.Find(id);
            if (image != null) 
            {
                db.Images.Remove(image);
            } 
            db.SaveChanges();
        }

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
