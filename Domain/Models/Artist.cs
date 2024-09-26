using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Artist
    {
        public Artist()
        {
            ArtistImages = new HashSet<ArtistImage>();
            TrackArtists = new HashSet<TrackArtist>();
            UserFollowings = new HashSet<UserFollowing>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FacebookLink { get; set; }
        public bool IsVerified { get; set; }

        public virtual ICollection<ArtistImage> ArtistImages { get; set; }
        public virtual ICollection<TrackArtist> TrackArtists { get; set; }
        public virtual ICollection<UserFollowing> UserFollowings { get; set; }
    }
}
