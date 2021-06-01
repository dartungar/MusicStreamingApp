using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class TrackArtist
    {
        public Guid Id { get; set; }
        public Guid TrackId { get; set; }
        public Guid ArtistId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Track Track { get; set; }
    }
}
