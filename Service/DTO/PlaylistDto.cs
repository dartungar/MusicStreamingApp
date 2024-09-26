using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class PlaylistDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public List<Guid> TracksIds { get; set; }
    }
}
