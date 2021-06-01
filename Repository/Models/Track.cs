using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class Track
    {
        public Track()
        {
            TrackArtists = new HashSet<TrackArtist>();
            TrackPlaybacks = new HashSet<TrackPlayback>();
            TrackUserReactions = new HashSet<TrackUserReaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public Guid AlbumId { get; set; }

        public virtual Album Album { get; set; }
        public virtual ICollection<TrackArtist> TrackArtists { get; set; }
        public virtual ICollection<TrackPlayback> TrackPlaybacks { get; set; }
        public virtual ICollection<TrackUserReaction> TrackUserReactions { get; set; }
    }
}
