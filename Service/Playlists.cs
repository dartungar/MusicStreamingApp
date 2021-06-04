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
    class Playlists
    {


        // playlist methods
        public PlaylistDto GetPlaylist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            return PlaylistToDto(db.Playlists.Find(id));
        }

        public List<PlaylistDto> GetPlaylists(string query)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Playlists.Where(p => p.Name == query).Select(pl => PlaylistToDto(pl)).ToList();
        }

        // TODO: get playlists by collection
        // TODO: get playlists by user

        public void UpdatePlaylist(Guid id, string name)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist foundPlaylist = db.Playlists.Find(id);
            if (foundPlaylist != null)
            {
                foundPlaylist.Name = name;
            }
            db.SaveChanges();
        }

        public PlaylistDto AddNewPlaylist(string name, Guid userId)
        {
            Playlist newPlaylist;
            
            using ApplicationContext db = new ApplicationContext();
            Playlist existingPlaylist = db.Playlists.FirstOrDefault(p => p.Name == name && p.AuthorUserId == userId);
            if (existingPlaylist != null)
                newPlaylist = existingPlaylist;
            Guid playlistId = Guid.NewGuid();
            newPlaylist = new Playlist
            {
                Id = playlistId,
                Name = name,
                AuthorUserId = userId
            };
            db.Playlists.Add(newPlaylist);
            db.SaveChanges();
            AddPlaylistToUser(playlistId, userId);

            return PlaylistToDto(newPlaylist);

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


        public void RemovePLaylist(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Playlist playlist = db.Playlists.Find(id);
            if (playlist != null) db.Playlists.Remove(playlist);
            db.SaveChanges();
        }

        public PlaylistDto PlaylistToDto(Playlist playlist)
        {
            using ApplicationContext db = new ApplicationContext();
            return new PlaylistDto
            {
                Id = playlist.Id,
                Name = playlist.Name
            };
        }
    }
}
