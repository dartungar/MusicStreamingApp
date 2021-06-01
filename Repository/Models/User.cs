using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class User
    {
        public User()
        {
            Playlists = new HashSet<Playlist>();
            TrackPlaybacks = new HashSet<TrackPlayback>();
            TrackUserReactions = new HashSet<TrackUserReaction>();
            UserFolders = new HashSet<UserFolder>();
            UserFollowings = new HashSet<UserFollowing>();
            UserMediaLibraries = new HashSet<UserMediaLibrary>();
            UserPlaylists = new HashSet<UserPlaylist>();
            UserSettings = new HashSet<UserSetting>();
            UserSubscriptions = new HashSet<UserSubscription>();
        }

        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<TrackPlayback> TrackPlaybacks { get; set; }
        public virtual ICollection<TrackUserReaction> TrackUserReactions { get; set; }
        public virtual ICollection<UserFolder> UserFolders { get; set; }
        public virtual ICollection<UserFollowing> UserFollowings { get; set; }
        public virtual ICollection<UserMediaLibrary> UserMediaLibraries { get; set; }
        public virtual ICollection<UserPlaylist> UserPlaylists { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
