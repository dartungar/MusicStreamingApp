using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistCollections = new HashSet<PlaylistCollection>();
            PlaylistFolders = new HashSet<PlaylistFolder>();
            UserPlaylists = new HashSet<UserPlaylist>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid AuthorUserId { get; set; }
        public Guid? ImageId { get; set; }
        public Guid PlaylistTypeId { get; set; }

        public virtual User AuthorUser { get; set; }
        public virtual Image IdNavigation { get; set; }
        public virtual PlaylistType PlaylistType { get; set; }
        public virtual ICollection<PlaylistCollection> PlaylistCollections { get; set; }
        public virtual ICollection<PlaylistFolder> PlaylistFolders { get; set; }
        public virtual ICollection<UserPlaylist> UserPlaylists { get; set; }
    }
}
