using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class TrackDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public int Length { get; set; }
        public List<Guid> ArtistsIds { get; set; }
        public List<string> ArtistsNames { get; set; }
        public Guid AlbumId { get; set; } = Guid.Empty;
        public string AlbumName { get; set; }
    }
}
