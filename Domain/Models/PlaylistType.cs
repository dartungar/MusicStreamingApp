using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class PlaylistType
    {
        public PlaylistType()
        {
            Playlists = new HashSet<Playlist>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
