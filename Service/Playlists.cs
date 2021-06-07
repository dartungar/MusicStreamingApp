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
    public class Playlists
    {

        public static PlaylistDto GetPlaylist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return PlaylistToDto(db.Playlists.Find(id));
        }

        public static List<PlaylistDto> GetPlaylists(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Playlists.Where(pl => pl.Name == query).Select(pl => PlaylistToDto(pl)).ToList();
        }


        public static List<PlaylistDto> GetUserPlaylists(Guid userId)
        {
            using ApplicationContext db = new ApplicationContext();
            List<Playlist> playlists = (from userPlaylist in db.UserPlaylists
                                       join playlist in db.Playlists on userPlaylist.PlaylistId equals playlist.Id
                                        where userPlaylist.UserId == userId
                                        select playlist).ToList();
                                       
            return playlists.Select(pl => PlaylistToDto(pl)).ToList();
        }

        public static List<PlaylistDto> GetPLaylistsByAuthor(Guid userId)
        {
            using ApplicationContext db = new ApplicationContext();
            List<Playlist> playlists = (from userPlaylist in db.UserPlaylists
                                        join playlist in db.Playlists on userPlaylist.PlaylistId equals playlist.Id
                                        select playlist).ToList();

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

        public static PlaylistDto AddOrGetPlaylist(string name, Guid userId, Guid playlistTypeId, string imageUrl)
        {
            Playlist newPlaylist;
            
            using ApplicationContext db = new ApplicationContext();
            Playlist existingPlaylist = db.Playlists.FirstOrDefault(pl => pl.Name == name && pl.AuthorUserId == userId);
            if (existingPlaylist != null)
                newPlaylist = existingPlaylist;
            Guid playlistId = Guid.NewGuid();
            newPlaylist = new Playlist
            {
                Id = playlistId,
                Name = name,
                AuthorUserId = userId,
                PlaylistTypeId = playlistTypeId,
                Image = new Image { Id = Guid.NewGuid(), Url = imageUrl}
            };
            db.Playlists.Add(newPlaylist);
            db.SaveChanges();
            AddPlaylistToUser(playlistId, userId);

            return PlaylistToDto(newPlaylist);

        }

        public static void AddPlaylistToUser(Guid playlistId, Guid userId)
        {
            using ApplicationContext db = new ApplicationContext();
            UserPlaylist existingUserPlaylist = db.UserPlaylists.FirstOrDefault(upl => upl.PlaylistId == playlistId && upl.UserId == userId);
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

        // playlist types methods

        public static List<PlaylistTypeDto> GetPlaylistTypes() {
            using ApplicationContext db = new ApplicationContext();
            return db.PlaylistTypes.Select(pt => new PlaylistTypeDto { Id = pt.Id, Name = pt.Name }).ToList();
        }
    }
}
