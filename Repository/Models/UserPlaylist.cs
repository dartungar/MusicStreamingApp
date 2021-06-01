using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class UserPlaylist
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PlaylistId { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual User User { get; set; }
    }
}
