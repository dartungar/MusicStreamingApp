using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class UserSubscription
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }

        public virtual User User { get; set; }
    }
}
