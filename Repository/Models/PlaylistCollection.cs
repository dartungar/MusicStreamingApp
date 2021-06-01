using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class PlaylistCollection
    {
        public Guid Id { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid CollectionId { get; set; }

        public virtual Collection Collection { get; set; }
        public virtual Playlist Playlist { get; set; }
    }
}
