using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class AddressElementType
    {
        public AddressElementType()
        {
            AddressElements = new HashSet<AddressElement>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AddressElement> AddressElements { get; set; }
    }
}
