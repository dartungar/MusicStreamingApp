using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class TrackPlayback
    {
        public Guid Id { get; set; }
        public Guid TrackId { get; set; }
        public Guid UserId { get; set; }
        public DateTime PlayedAt { get; set; }

        public virtual Track Track { get; set; }
        public virtual User User { get; set; }
    }
}
