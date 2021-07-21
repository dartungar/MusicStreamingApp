using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class TrackUserReaction
    {
        public Guid Id { get; set; }
        public Guid TrackId { get; set; }
        public Guid UserId { get; set; }
        public bool? IsLike { get; set; }

        public virtual Track Track { get; set; }
        public virtual User User { get; set; }
    }
}
