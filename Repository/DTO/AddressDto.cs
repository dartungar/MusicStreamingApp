using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public class AddressDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public AddressElementDto Country { get; set; }
        public AddressElementDto Region { get; set; }
        public AddressElementDto City { get; set; }
        public AddressElementDto Street { get; set; }
        public string House { get; set; }
    }
}
