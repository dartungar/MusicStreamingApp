using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class AddressElement
    {
        public AddressElement()
        {
            AddressCities = new HashSet<Address>();
            AddressCountries = new HashSet<Address>();
            AddressRegions = new HashSet<Address>();
            AddressStreets = new HashSet<Address>();
        }

        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid AddressElementTypeId { get; set; }

        public virtual AddressElementType AddressElementType { get; set; }
        public virtual ICollection<Address> AddressCities { get; set; }
        public virtual ICollection<Address> AddressCountries { get; set; }
        public virtual ICollection<Address> AddressRegions { get; set; }
        public virtual ICollection<Address> AddressStreets { get; set; }
    }
}
