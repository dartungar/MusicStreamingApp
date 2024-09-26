using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class ImageDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Url { get; set; }

    }
}
