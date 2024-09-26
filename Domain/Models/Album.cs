using System;
using System.Collections.Generic;
using Domain.Models;

#nullable disable

namespace Domain.Models
{
    public partial class Album
    {
        public Album()
        {
            Tracks = new HashSet<Track>();
            UserMediaLibraries = new HashSet<UserMediaLibrary>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AlbumTypeId { get; set; }
        public DateTime Date { get; set; }
        public Guid ImageId { get; set; }

        public virtual AlbumType AlbumType { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public virtual ICollection<UserMediaLibrary> UserMediaLibraries { get; set; }
    }
}
