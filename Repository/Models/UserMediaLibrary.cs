using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class UserMediaLibrary
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AlbumId { get; set; }

        public virtual Album Album { get; set; }
        public virtual User User { get; set; }
    }
}
