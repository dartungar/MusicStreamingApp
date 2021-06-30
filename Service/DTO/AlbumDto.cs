using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class AlbumDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TrackDto> TracksList { get; set; }
        public List<Guid> Artists { get; set; }
        public ImageDto Image { get; set; }
    }
}
