using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class UserSetting
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SettingId { get; set; }
        public string Value { get; set; }

        public virtual Setting Setting { get; set; }
        public virtual User User { get; set; }
    }
}
