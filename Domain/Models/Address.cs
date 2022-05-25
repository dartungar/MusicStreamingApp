using System;
using System.Collections.Generic;
using Domain.Models;

#nullable disable

namespace Domain.Models
{
    public partial class Address
    {
        public Address()
        {
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid? RegionId { get; set; }
        public Guid CityId { get; set; }
        public Guid? StreetId { get; set; }
        public string House { get; set; }

        public virtual AddressElement City { get; set; }
        public virtual AddressElement Country { get; set; }
        public virtual AddressElement Region { get; set; }
        public virtual AddressElement Street { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
