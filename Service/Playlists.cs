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
    public class Playlists
    {

        public static PlaylistDto GetPlaylist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return PlaylistToDto(db.Playlists.AsNoTracking().Where(pl => pl.Id == id).FirstOrDefault());
        }

        public static List<PlaylistDto> SearchPlaylists(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Playlists.AsNoTracking().Where(pl => pl.Name.Contains(query)).Select(pl => PlaylistToDto(pl)).ToList();
        }


        public static List<PlaylistDto> GetUserPlaylists(Guid userId)
        {
            using ApplicationContext db = new ApplicationContext();
            List<Playlist> playlists = (from userPlaylist in db.UserPlaylists
                                       join playlist in db.Playlists on userPlaylist.PlaylistId equals playlist.Id
                                        where userPlaylist.UserId == userId
                                        select playlist).AsNoTracking().ToList();
                                       
            return playlists.Select(pl => PlaylistToDto(pl)).ToList();
        }

        public static List<PlaylistDto> GetPLaylistsByAuthor(Guid userId)
        {
            using ApplicationContext db = new ApplicationContext();
            List<Playlist> playlists = (from userPlaylist in db.UserPlaylists
                                        join playlist in db.Playlists on userPlaylist.PlaylistId equals playlist.Id
                                        select playlist).AsNoTracking().ToList();

            return playlists.Where(pl => pl.AuthorUserId == userId).Select(pl => PlaylistToDto(pl)).ToList();
        }


        public static void UpdatePlaylist(Guid id, string name)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist foundPlaylist = db.Playlists.Find(id);
            if (foundPlaylist != null)
            {
                foundPlaylist.Name = name;
            }
            db.SaveChanges();
        }

        public static PlaylistDto AddOrGetPlaylist(string name, Guid authorId, Guid playlistTypeId, string imageUrl)
        {
            Playlist newPlaylist;
            
            using ApplicationContext db = new ApplicationContext();
            Playlist existingPlaylist = db.Playlists.AsNoTracking().FirstOrDefault(pl => pl.Name == name && pl.AuthorUserId == authorId);
            if (existingPlaylist != null)
                newPlaylist = existingPlaylist;
            Guid playlistId = Guid.NewGuid();
            newPlaylist = new Playlist
            {
                Id = playlistId,
                Name = name,
                AuthorUserId = authorId,
                PlaylistTypeId = playlistTypeId,
                Image = new Image { Id = Guid.NewGuid(), Url = imageUrl }
            };
            db.Playlists.Add(newPlaylist);
            db.SaveChanges();
            AddPlaylistToUser(playlistId, authorId);

            return PlaylistToDto(newPlaylist);

        }

        public static PlaylistDto AddOrGetPlaylist(string name, string description, Guid authorId, Guid playlistTypeId, string imageUrl)
        {
            Playlist newPlaylist;

            using ApplicationContext db = new ApplicationContext();
            Playlist existingPlaylist = db.Playlists.AsNoTracking().FirstOrDefault(pl => pl.Name == name && pl.AuthorUserId == authorId);
            if (existingPlaylist != null)
                newPlaylist = existingPlaylist;
            Guid playlistId = Guid.NewGuid();
            newPlaylist = new Playlist
            {
                Id = playlistId,
                Name = name,
                Description = description,
                AuthorUserId = authorId,
                PlaylistTypeId = playlistTypeId,
                Image = new Image { Id = Guid.NewGuid(), Url = imageUrl }
            };
            db.Playlists.Add(newPlaylist);
            db.SaveChanges();
            AddPlaylistToUser(playlistId, authorId);

            return PlaylistToDto(newPlaylist);

        }

        public static PlaylistDto AddOrGetPlaylist(string name, string description, Guid authorId, string imageUrl)
        {
            Playlist newPlaylist;

            using ApplicationContext db = new ApplicationContext();
            Playlist existingPlaylist = db.Playlists.AsNoTracking().FirstOrDefault(pl => pl.Name == name && pl.AuthorUserId == authorId);
            if (existingPlaylist != null)
                newPlaylist = existingPlaylist;
            Guid playlistId = Guid.NewGuid();
            newPlaylist = new Playlist
            {
                Id = playlistId,
                Name = name,
                Description = description,
                AuthorUserId = authorId,
                PlaylistTypeId = db.PlaylistTypes.Where(pt => pt.Name == "Default").FirstOrDefault().Id,
                Image = new Image { Id = Guid.NewGuid(), Url = imageUrl }
            };
            db.Playlists.Add(newPlaylist);
            db.SaveChanges();
            AddPlaylistToUser(playlistId, authorId);

            return PlaylistToDto(newPlaylist);

        }

        public static void AddPlaylistToUser(Guid playlistId, Guid userId)
        {
            using ApplicationContext db = new ApplicationContext();
            UserPlaylist existingUserPlaylist = db.UserPlaylists.AsNoTracking().FirstOrDefault(upl => upl.PlaylistId == playlistId && upl.UserId == userId);
            if (existingUserPlaylist == null)
            {
                UserPlaylist newUserPlaylist = new UserPlaylist
                {
                    Id = Guid.NewGuid(),
                    PlaylistId = playlistId,
                    UserId = userId
                };
                db.SaveChanges();
            }
        }


        public static void RemovePLaylist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist playlist = db.Playlists.Find(id);
            if (playlist != null) db.Playlists.Remove(playlist);
            db.SaveChanges();
        }

        public static PlaylistDto PlaylistToDto(Playlist playlist)
        {
            using ApplicationContext db = new ApplicationContext();
            return new PlaylistDto
            {
                Id = playlist.Id,
                Name = playlist.Name
            };
        }

        // playlist-track methods
        public static void AddTrackToPlaylist(Guid trackId, Guid playlistId)
        {
            using ApplicationContext db = new ApplicationContext();
            
            Playlist playlist = db.Playlists.Find(playlistId);
            if (playlist == null)
                throw new Exception($"Плейлист с ID {playlistId} не найден");
            
            Track track = db.Tracks.Find(trackId);
            if (track == null)
                throw new Exception($"Трек с ID {trackId} не найден");

            playlist.Tracks.Add(track);
            db.SaveChanges();
        }

        public static void RemoveTrackFromPlaylist(Guid trackId, Guid playlistId)
        {
            using ApplicationContext db = new ApplicationContext();

            Playlist playlist = db.Playlists.Find(playlistId);
            if (playlist == null)
                throw new Exception($"Плейлист с ID {playlistId} не найден");

            Track track = db.Tracks.Find(trackId);
            if (track == null)
                throw new Exception($"Трек с ID {trackId} не найден");

            playlist.Tracks.Remove(track);
            db.SaveChanges();
        }

        // playlist images methods
        public static void UpdatePlaylistImage(Guid playlistId, string imageUrl)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist playlist = db.Playlists.Find(playlistId);
            if (playlist != null)
            {
                Image imageWithSameUrl = db.Images.AsNoTracking().Where(i => i.Url == imageUrl).FirstOrDefault();
                if (imageWithSameUrl == null)
                {
                    Image newImage = new Image { Id = Guid.NewGuid(), Url = imageUrl };
                    db.Images.Add(newImage);
                    playlist.ImageId = newImage.Id;
                    db.SaveChanges();
                }
                else throw new Exception($"У плейлиста {playlist.Name} уже существует изображение с URL {imageWithSameUrl.Url}");
            }
            else throw new Exception($"Плейлиста с ID {playlistId} не найдено");
        }

        // playlist types methods

        public static List<PlaylistTypeDto> GetPlaylistTypes() {
            using ApplicationContext db = new ApplicationContext();
            return db.PlaylistTypes.AsNoTracking().Select(pt => new PlaylistTypeDto { Id = pt.Id, Name = pt.Name }).ToList();
        }
    }
}
