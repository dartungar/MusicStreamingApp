using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Setting
    {
        public Setting()
        {
            UserSettings = new HashSet<UserSetting>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ValueType { get; set; }

        public virtual ICollection<UserSetting> UserSettings { get; set; }
    }
}
