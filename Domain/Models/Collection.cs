using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Collection
    {
        public Collection()
        {
            PlaylistCollections = new HashSet<PlaylistCollection>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual ICollection<PlaylistCollection> PlaylistCollections { get; set; }
    }
}
