using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class AlbumType
    {
        public AlbumType()
        {
            Albums = new HashSet<Album>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
