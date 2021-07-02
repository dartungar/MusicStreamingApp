using System;
using System.ComponentModel.DataAnnotations;

namespace Service.DTO
{
    public class AddressDto
    {
        public Guid? Id { get; set; }
        public AddressElementDto Country { get; set; }
        public AddressElementDto Region { get; set; }
        public AddressElementDto City { get; set; }
        public AddressElementDto Street { get; set; }
        public string House { get; set; }
    }
}
