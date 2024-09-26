using System;
using System.ComponentModel.DataAnnotations;

namespace Service.DTO
{
    public class AddressDto
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid CountryId { get; set; } = Guid.Empty;
        public AddressElementDto Country { get; set; }

        public Guid RegionId { get; set; } = Guid.Empty;
        public AddressElementDto Region { get; set; }

        public Guid CityId { get; set; } = Guid.Empty;
        public AddressElementDto City { get; set; }

        public Guid StreetId { get; set; } = Guid.Empty;
        public AddressElementDto Street { get; set; }

        public string House { get; set; }
    }
}
