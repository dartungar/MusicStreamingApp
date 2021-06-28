using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public class AddressElementDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Value { get; set; }
    }
}
