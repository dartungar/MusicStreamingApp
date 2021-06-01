using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class UserFolder
    {
        public UserFolder()
        {
            PlaylistFolders = new HashSet<PlaylistFolder>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PlaylistFolder> PlaylistFolders { get; set; }
    }
}
