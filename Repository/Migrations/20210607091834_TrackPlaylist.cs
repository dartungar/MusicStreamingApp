using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class TrackPlaylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // расплата за отсутствие изначальной миграции
            /*
            migrationBuilder.CreateTable(
                name: "AddressElementType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressElementType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AlbumType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FacebookLink = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValueType = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AddressElement",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressElementTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressElement", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AddressElement_AddressElementType",
                        column: x => x.AddressElementTypeID,
                        principalTable: "AddressElementType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlbumTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Album_AlbumType",
                        column: x => x.AlbumTypeID,
                        principalTable: "AlbumType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Album_Image",
                        column: x => x.ImageID,
                        principalTable: "Image",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistImage",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistImage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArtistImage_Artist",
                        column: x => x.ArtistID,
                        principalTable: "Artist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArtistImage_Image",
                        column: x => x.ImageID,
                        principalTable: "Image",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Collection_Image",
                        column: x => x.ImageID,
                        principalTable: "Image",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreetID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    House = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Address_AddressElement_City",
                        column: x => x.CityID,
                        principalTable: "AddressElement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Address_AddressElement_Country",
                        column: x => x.CountryID,
                        principalTable: "AddressElement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Address_AddressElement_Region",
                        column: x => x.RegionID,
                        principalTable: "AddressElement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Address_AddressElement_Street",
                        column: x => x.StreetID,
                        principalTable: "AddressElement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    AlbumID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Track", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Track_Album",
                        column: x => x.AlbumID,
                        principalTable: "Album",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Address",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackArtist",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackArtist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TrackArtist_Artist",
                        column: x => x.ArtistID,
                        principalTable: "Artist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackArtist_Track",
                        column: x => x.TrackID,
                        principalTable: "Track",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AuthorUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlaylistTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Playlist_AuthorUser",
                        column: x => x.AuthorUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playlist_Image",
                        column: x => x.ID,
                        principalTable: "Image",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playlist_PlaylistType",
                        column: x => x.PlaylistTypeID,
                        principalTable: "PlaylistType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackPlayback",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackPlayback", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TrackPlayback_Track",
                        column: x => x.TrackID,
                        principalTable: "Track",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackPlayback_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackUserReaction",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackUserReaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TrackUserReaction_Track",
                        column: x => x.TrackID,
                        principalTable: "Track",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackUserReaction_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFolder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFolder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserFolder_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFollowing",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowing", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserFollowing_Artist",
                        column: x => x.ArtistID,
                        principalTable: "Artist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFollowing_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMediaLibrary",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMediaLibrary", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserMediaLibrary_Album",
                        column: x => x.AlbumID,
                        principalTable: "Album",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMediaLibrary_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserSetting_Setting",
                        column: x => x.SettingID,
                        principalTable: "Setting",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSetting_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscription",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateBegin = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscription", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserSubscription_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistCollection",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaylistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistCollection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlaylistCollection_Collection",
                        column: x => x.CollectionID,
                        principalTable: "Collection",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistCollection_Playlist",
                        column: x => x.PlaylistID,
                        principalTable: "Playlist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });
            */
            migrationBuilder.CreateTable(
                name: "PlaylistTrack",
                columns: table => new
                {
                    PlaylistsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TracksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistTrack", x => new { x.PlaylistsId, x.TracksId });
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_Playlist_PlaylistsId",
                        column: x => x.PlaylistsId,
                        principalTable: "Playlist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_Track_TracksId",
                        column: x => x.TracksId,
                        principalTable: "Track",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
            /*
            migrationBuilder.CreateTable(
                name: "UserPlaylist",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaylistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlaylist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserPlaylist_Playlist",
                        column: x => x.PlaylistID,
                        principalTable: "Playlist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPlaylist_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistFolder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaylistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FolderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistFolder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlaylistFolder_Playlist",
                        column: x => x.PlaylistID,
                        principalTable: "Playlist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistFolder_UserFolder",
                        column: x => x.FolderID,
                        principalTable: "UserFolder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AddressElementType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("f0f630c5-e9a7-4b87-a319-7aee721c925a"), "Country" },
                    { new Guid("83c9a757-acaf-46c6-9df9-fadf831525b3"), "Region" },
                    { new Guid("6a18014c-e204-468d-b428-4697ee7a19f1"), "City" },
                    { new Guid("866a0883-913b-435c-8242-e22db5cc90dc"), "Street" }
                });

            migrationBuilder.InsertData(
                table: "AlbumType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("cc05a77b-ab2d-4560-9dae-ffd5171f8b9c"), "Album" },
                    { new Guid("9464a8d7-4a4b-4b04-a3ef-9f752ee2d75c"), "EP" },
                    { new Guid("11e83471-6f24-47a0-8888-6b71ca29ad1e"), "Single" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address",
                table: "Address",
                columns: new[] { "CountryID", "RegionID", "CityID", "StreetID", "House" },
                unique: true,
                filter: "[RegionID] IS NOT NULL AND [StreetID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Address_CityID",
                table: "Address",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_RegionID",
                table: "Address",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_StreetID",
                table: "Address",
                column: "StreetID");

            migrationBuilder.CreateIndex(
                name: "IX_AddressElement_AddressElementTypeID",
                table: "AddressElement",
                column: "AddressElementTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Album_AlbumTypeID",
                table: "Album",
                column: "AlbumTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Album_ImageID",
                table: "Album",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Name",
                table: "Album",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Artist_Name",
                table: "Artist",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistImage_Artist",
                table: "ArtistImage",
                columns: new[] { "ArtistID", "ImageID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistImage_ImageID",
                table: "ArtistImage",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_ImageID",
                table: "Collection",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_Name",
                table: "Collection",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist",
                table: "Playlist",
                columns: new[] { "Name", "AuthorUserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_Author",
                table: "Playlist",
                column: "AuthorUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_PlaylistTypeID",
                table: "Playlist",
                column: "PlaylistTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCollection",
                table: "PlaylistCollection",
                columns: new[] { "CollectionID", "PlaylistID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCollection_PlaylistID",
                table: "PlaylistCollection",
                column: "PlaylistID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistFolder",
                table: "PlaylistFolder",
                columns: new[] { "FolderID", "PlaylistID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistFolder_PlaylistID",
                table: "PlaylistFolder",
                column: "PlaylistID");
            */
            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTrack_TracksId",
                table: "PlaylistTrack",
                column: "TracksId");
            /*
            migrationBuilder.CreateIndex(
                name: "IX_Track",
                table: "Track",
                columns: new[] { "AlbumID", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Track_Name",
                table: "Track",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtist",
                table: "TrackArtist",
                columns: new[] { "ArtistID", "TrackID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtist_Track",
                table: "TrackArtist",
                column: "TrackID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackPlayback",
                table: "TrackPlayback",
                columns: new[] { "UserID", "PlayedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackPlayback_Track",
                table: "TrackPlayback",
                column: "TrackID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUserReaction_IsLike",
                table: "TrackUserReaction",
                columns: new[] { "UserID", "IsLike" });

            migrationBuilder.CreateIndex(
                name: "IX_TrackUserReaction_TrackID",
                table: "TrackUserReaction",
                column: "TrackID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUserReaction_UserTrack",
                table: "TrackUserReaction",
                columns: new[] { "UserID", "TrackID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User",
                table: "User",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_AddressID",
                table: "User",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_User_EmailName",
                table: "User",
                columns: new[] { "Email", "Name" });

            migrationBuilder.CreateIndex(
                name: "UQ__User__5E55825B69D58426",
                table: "User",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFolder_User",
                table: "UserFolder",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowing",
                table: "UserFollowing",
                columns: new[] { "UserID", "ArtistID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowing_Artist",
                table: "UserFollowing",
                column: "ArtistID");

            migrationBuilder.CreateIndex(
                name: "IX_UserMediaLibrary",
                table: "UserMediaLibrary",
                columns: new[] { "UserID", "AlbumID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMediaLibrary_AlbumID",
                table: "UserMediaLibrary",
                column: "AlbumID");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaylist",
                table: "UserPlaylist",
                columns: new[] { "UserID", "PlaylistID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaylist_PlaylistID",
                table: "UserPlaylist",
                column: "PlaylistID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSetting",
                table: "UserSetting",
                columns: new[] { "UserID", "SettingID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSetting_SettingID",
                table: "UserSetting",
                column: "SettingID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription",
                table: "UserSubscription",
                columns: new[] { "UserID", "DateBegin", "DateEnd" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_Dates",
                table: "UserSubscription",
                columns: new[] { "DateBegin", "DateEnd" });
        */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropTable(
                name: "ArtistImage");

            migrationBuilder.DropTable(
                name: "PlaylistCollection");

            migrationBuilder.DropTable(
                name: "PlaylistFolder");
            */
            migrationBuilder.DropTable(
                name: "PlaylistTrack");
            /*
            migrationBuilder.DropTable(
                name: "TrackArtist");

            migrationBuilder.DropTable(
                name: "TrackPlayback");

            migrationBuilder.DropTable(
                name: "TrackUserReaction");

            migrationBuilder.DropTable(
                name: "UserFollowing");

            migrationBuilder.DropTable(
                name: "UserMediaLibrary");

            migrationBuilder.DropTable(
                name: "UserPlaylist");

            migrationBuilder.DropTable(
                name: "UserSetting");

            migrationBuilder.DropTable(
                name: "UserSubscription");

            migrationBuilder.DropTable(
                name: "Collection");

            migrationBuilder.DropTable(
                name: "UserFolder");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PlaylistType");

            migrationBuilder.DropTable(
                name: "AlbumType");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "AddressElement");

            migrationBuilder.DropTable(
                name: "AddressElementType");
            */
        }
    }
}
