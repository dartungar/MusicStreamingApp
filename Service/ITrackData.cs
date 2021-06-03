using System;
using System.Collections.Generic;

namespace Service
{
    public interface ITrackData
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public List<Guid> ArtistIds { get; set; }
        public Guid AlbumId { get; set; }


    }
}
