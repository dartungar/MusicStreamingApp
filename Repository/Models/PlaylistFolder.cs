using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class PlaylistFolder
    {
        public Guid Id { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid FolderId { get; set; }

        public virtual UserFolder Folder { get; set; }
        public virtual Playlist Playlist { get; set; }
    }
}
