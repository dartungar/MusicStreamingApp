using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
    class Playlists
    {


        // playlist methods
        public List<Playlist> GetPlaylists()
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Playlists.ToList();
        }

        public List<Playlist> GetPlaylists(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Playlists.Where(p => p.Name == query).ToList();
        }
        // TODO: get playlists by collection
        // TODO: get playlists by user

        public void UpdatePlaylist(Playlist playlist)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist foundPlaylist = db.Playlists.Find(playlist.Id);
            if (foundPlaylist != null) foundPlaylist = playlist;
            db.SaveChanges();
        }

        public Playlist AddNewPlaylist(string name, Guid userId)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist existingPlaylist = db.Playlists.FirstOrDefault(p => p.Name == name && p.AuthorUserId == userId);
            if (existingPlaylist != null)
                return existingPlaylist;
            Guid playlistId = Guid.NewGuid();
            Playlist newPlaylist = new Playlist
            {
                Id = playlistId,
                Name = name,
                AuthorUserId = userId
            };
            db.Playlists.Add(newPlaylist);
            db.SaveChanges();
            AddPlaylistToUser(playlistId, userId);
            return newPlaylist;

        }

        public void AddPlaylistToUser(Guid playlistId, Guid userId)
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

        public void RemovePLaylist(Playlist playlist)
        {
            using ApplicationContext db = new ApplicationContext();
            db.Playlists.Remove(playlist);
            db.SaveChanges();
        }

        public void RemovePLaylist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist playlist = db.Playlists.Find(id);
            if (playlist != null) db.Playlists.Remove(playlist);
            db.SaveChanges();
        }
    }
}
