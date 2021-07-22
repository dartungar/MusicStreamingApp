using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class ArtistImage
    {
        public Guid Id { get; set; }
        public Guid ArtistId { get; set; }
        public Guid ImageId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Image Image { get; set; }
    }
}
