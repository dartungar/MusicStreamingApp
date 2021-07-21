using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class UserFollowing
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ArtistId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual User User { get; set; }
    }
}
