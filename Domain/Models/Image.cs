using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Image
    {
        public Image()
        {
            Albums = new HashSet<Album>();
            ArtistImages = new HashSet<ArtistImage>();
            Collections = new HashSet<Collection>();
        }

        public Guid Id { get; set; }
        public string Url { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<ArtistImage> ArtistImages { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
    }
}
