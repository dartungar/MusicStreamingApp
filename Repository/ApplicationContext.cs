using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Repository.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

#nullable disable

namespace Repository
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressElement> AddressElements { get; set; }
        public virtual DbSet<AddressElementType> AddressElementTypes { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<AlbumType> AlbumTypes { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<ArtistImage> ArtistImages { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<PlaylistCollection> PlaylistCollections { get; set; }
        public virtual DbSet<PlaylistFolder> PlaylistFolders { get; set; }
        public virtual DbSet<PlaylistType> PlaylistTypes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<TrackArtist> TrackArtists { get; set; }
        public virtual DbSet<TrackPlayback> TrackPlaybacks { get; set; }
        public virtual DbSet<TrackUserReaction> TrackUserReactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserFolder> UserFolders { get; set; }
        public virtual DbSet<UserFollowing> UserFollowings { get; set; }
        public virtual DbSet<UserMediaLibrary> UserMediaLibraries { get; set; }
        public virtual DbSet<UserPlaylist> UserPlaylists { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

                optionsBuilder.LogTo(System.Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Error);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.HasIndex(e => new { e.CountryId, e.RegionId, e.CityId, e.StreetId, e.House }, "IX_Address")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.House)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.StreetId).HasColumnName("StreetID");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.AddressCities)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_AddressElement_City");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.AddressCountries)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_AddressElement_Country");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.AddressRegions)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_Address_AddressElement_Region");

                entity.HasOne(d => d.Street)
                    .WithMany(p => p.AddressStreets)
                    .HasForeignKey(d => d.StreetId)
                    .HasConstraintName("FK_Address_AddressElement_Street");
            });

            modelBuilder.Entity<AddressElement>(entity =>
            {
                entity.ToTable("AddressElement");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AddressElementTypeId).HasColumnName("AddressElementTypeID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.AddressElementType)
                    .WithMany(p => p.AddressElements)
                    .HasForeignKey(d => d.AddressElementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AddressElement_AddressElementType");
            });

            modelBuilder.Entity<AddressElementType>(entity =>
            {
                entity.ToTable("AddressElementType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasData(
                    new AddressElementType { Id = Guid.NewGuid(), Name = "Country" },
                    new AddressElementType { Id = Guid.NewGuid(), Name = "Region" },
                    new AddressElementType { Id = Guid.NewGuid(), Name = "City" },
                    new AddressElementType { Id = Guid.NewGuid(), Name = "Street" }
                    );
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("Album");

                entity.HasIndex(e => e.Name, "IX_Album_Name");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AlbumTypeId).HasColumnName("AlbumTypeID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.AlbumType)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.AlbumTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Album_AlbumType");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Album_Image");
            });

            modelBuilder.Entity<AlbumType>(entity =>
            {
                entity.ToTable("AlbumType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasData(
                    new AlbumType { Id = Guid.NewGuid(), Name = "Album" },
                    new AlbumType { Id = Guid.NewGuid(), Name = "EP" },
                    new AlbumType { Id = Guid.NewGuid(), Name = "Single" });
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artist");

                entity.HasIndex(e => e.Name, "IX_Artist_Name");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.FacebookLink).HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ArtistImage>(entity =>
            {
                entity.ToTable("ArtistImage");

                entity.HasIndex(e => new { e.ArtistId, e.ImageId }, "IX_ArtistImage_Artist")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistImages)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArtistImage_Artist");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ArtistImages)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArtistImage_Image");
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("Collection");

                entity.HasIndex(e => e.Name, "IX_Collection_Name");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Collection_Image");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.ToTable("Playlist");

                entity.HasIndex(e => new { e.Name, e.AuthorUserId }, "IX_Playlist")
                    .IsUnique();

                entity.HasIndex(e => e.AuthorUserId, "IX_Playlist_Author");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AuthorUserId).HasColumnName("AuthorUserID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PlaylistTypeId).HasColumnName("PlaylistTypeID");

                entity.HasOne(d => d.AuthorUser)
                    .WithMany(p => p.Playlists)
                    .HasForeignKey(d => d.AuthorUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Playlist_AuthorUser");

                entity.HasOne(d => d.Image)
                    .WithOne(p => p.Playlist)
                    .HasForeignKey<Playlist>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Playlist_Image");

                entity.HasOne(d => d.PlaylistType)
                    .WithMany(p => p.Playlists)
                    .HasForeignKey(d => d.PlaylistTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Playlist_PlaylistType");
            });

            modelBuilder.Entity<PlaylistCollection>(entity =>
            {
                entity.ToTable("PlaylistCollection");

                entity.HasIndex(e => new { e.CollectionId, e.PlaylistId }, "IX_PlaylistCollection")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CollectionId).HasColumnName("CollectionID");

                entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.PlaylistCollections)
                    .HasForeignKey(d => d.CollectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlaylistCollection_Collection");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistCollections)
                    .HasForeignKey(d => d.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlaylistCollection_Playlist");
            });

            modelBuilder.Entity<PlaylistFolder>(entity =>
            {
                entity.ToTable("PlaylistFolder");

                entity.HasIndex(e => new { e.FolderId, e.PlaylistId }, "IX_PlaylistFolder")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.PlaylistFolders)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlaylistFolder_UserFolder");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistFolders)
                    .HasForeignKey(d => d.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlaylistFolder_Playlist");
            });

            modelBuilder.Entity<PlaylistType>(entity =>
            {
                entity.ToTable("PlaylistType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.ToTable("Track");

                entity.HasIndex(e => new { e.AlbumId, e.Name }, "IX_Track")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "IX_Track_Name");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AlbumId).HasColumnName("AlbumID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Track_Album");
            });

            modelBuilder.Entity<TrackArtist>(entity =>
            {
                entity.ToTable("TrackArtist");

                entity.HasIndex(e => new { e.ArtistId, e.TrackId }, "IX_TrackArtist")
                    .IsUnique();

                entity.HasIndex(e => e.TrackId, "IX_TrackArtist_Track");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

                entity.Property(e => e.TrackId).HasColumnName("TrackID");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.TrackArtists)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrackArtist_Artist");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.TrackArtists)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrackArtist_Track");
            });

            modelBuilder.Entity<TrackPlayback>(entity =>
            {
                entity.ToTable("TrackPlayback");

                entity.HasIndex(e => new { e.UserId, e.PlayedAt }, "IX_TrackPlayback")
                    .IsUnique();

                entity.HasIndex(e => e.TrackId, "IX_TrackPlayback_Track");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.PlayedAt).HasColumnType("datetime");

                entity.Property(e => e.TrackId).HasColumnName("TrackID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.TrackPlaybacks)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrackPlayback_Track");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TrackPlaybacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrackPlayback_User");
            });

            modelBuilder.Entity<TrackUserReaction>(entity =>
            {
                entity.ToTable("TrackUserReaction");

                entity.HasIndex(e => new { e.UserId, e.IsLike }, "IX_TrackUserReaction_IsLike");

                entity.HasIndex(e => new { e.UserId, e.TrackId }, "IX_TrackUserReaction_UserTrack")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.IsLike)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TrackId).HasColumnName("TrackID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.TrackUserReactions)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrackUserReaction_Track");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TrackUserReactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrackUserReaction_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Login, "IX_User")
                    .IsUnique();

                entity.HasIndex(e => new { e.Email, e.Name }, "IX_User_EmailName");

                entity.HasIndex(e => e.Login, "UQ__User__5E55825B69D58426")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Address");
            });

            modelBuilder.Entity<UserFolder>(entity =>
            {
                entity.ToTable("UserFolder");

                entity.HasIndex(e => e.UserId, "IX_UserFolder_User");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFolders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserFolder_User");
            });

            modelBuilder.Entity<UserFollowing>(entity =>
            {
                entity.ToTable("UserFollowing");

                entity.HasIndex(e => new { e.UserId, e.ArtistId }, "IX_UserFollowing")
                    .IsUnique();

                entity.HasIndex(e => e.ArtistId, "IX_UserFollowing_Artist");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.UserFollowings)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserFollowing_Artist");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFollowings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserFollowing_User");
            });

            modelBuilder.Entity<UserMediaLibrary>(entity =>
            {
                entity.ToTable("UserMediaLibrary");

                entity.HasIndex(e => new { e.UserId, e.AlbumId }, "IX_UserMediaLibrary")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AlbumId).HasColumnName("AlbumID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.UserMediaLibraries)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMediaLibrary_Album");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserMediaLibraries)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMediaLibrary_User");
            });

            modelBuilder.Entity<UserPlaylist>(entity =>
            {
                entity.ToTable("UserPlaylist");

                entity.HasIndex(e => new { e.UserId, e.PlaylistId }, "IX_UserPlaylist")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.UserPlaylists)
                    .HasForeignKey(d => d.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPlaylist_Playlist");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPlaylists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPlaylist_User");
            });

            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.ToTable("UserSetting");

                entity.HasIndex(e => new { e.UserId, e.SettingId }, "IX_UserSetting")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.SettingId).HasColumnName("SettingID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSetting_Setting");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSetting_User");
            });

            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.ToTable("UserSubscription");

                entity.HasIndex(e => new { e.UserId, e.DateBegin, e.DateEnd }, "IX_UserSubscription")
                    .IsUnique();

                entity.HasIndex(e => new { e.DateBegin, e.DateEnd }, "IX_UserSubscription_Dates");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DateBegin).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubscriptions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSubscription_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
