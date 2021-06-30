using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CascadeDeleteArtistArtistTracks2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackArtist_Artist",
                table: "TrackArtist");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackArtist_Track",
                table: "TrackArtist");

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("46d75256-4e54-4287-8557-961475b792b6"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("c916d09a-c9e6-4a12-b05e-0c8e7fdb6799"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("d35b1120-a076-4634-bce4-65f3f69699ab"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("da6019dc-caac-4dad-862b-b4e8e2b11e52"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("2c2228f6-fc67-43a0-93e3-d85c60e128ee"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("3fa2b648-a426-4140-9ed4-3229c638a89f"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("b60a288a-b223-4ce8-804d-d6c86c6d62af"));

            migrationBuilder.InsertData(
                table: "AddressElementType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("293a544b-79e3-4365-b12d-019fc7fb476d"), "Country" },
                    { new Guid("65efc804-d6cf-430a-ac0e-9974678b7780"), "Region" },
                    { new Guid("1c99e9f8-dc90-4038-b1af-2d293f5e0d4e"), "City" },
                    { new Guid("8e504ef3-6276-48d5-8eeb-0cfbd75679f3"), "Street" }
                });

            migrationBuilder.InsertData(
                table: "AlbumType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("c85c9b7f-67bc-4c9e-93cc-0b365b13576b"), "Album" },
                    { new Guid("15bd3823-d3d7-4ce1-97f6-e696e7a58a54"), "EP" },
                    { new Guid("2442ed30-a4b5-48fd-be84-675e27487623"), "Single" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TrackArtist_Artist",
                table: "TrackArtist",
                column: "ArtistID",
                principalTable: "Artist",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackArtist_Track",
                table: "TrackArtist",
                column: "TrackID",
                principalTable: "Track",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackArtist_Artist",
                table: "TrackArtist");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackArtist_Track",
                table: "TrackArtist");

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("1c99e9f8-dc90-4038-b1af-2d293f5e0d4e"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("293a544b-79e3-4365-b12d-019fc7fb476d"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("65efc804-d6cf-430a-ac0e-9974678b7780"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("8e504ef3-6276-48d5-8eeb-0cfbd75679f3"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("15bd3823-d3d7-4ce1-97f6-e696e7a58a54"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("2442ed30-a4b5-48fd-be84-675e27487623"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("c85c9b7f-67bc-4c9e-93cc-0b365b13576b"));

            migrationBuilder.InsertData(
                table: "AddressElementType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("46d75256-4e54-4287-8557-961475b792b6"), "Country" },
                    { new Guid("da6019dc-caac-4dad-862b-b4e8e2b11e52"), "Region" },
                    { new Guid("d35b1120-a076-4634-bce4-65f3f69699ab"), "City" },
                    { new Guid("c916d09a-c9e6-4a12-b05e-0c8e7fdb6799"), "Street" }
                });

            migrationBuilder.InsertData(
                table: "AlbumType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("3fa2b648-a426-4140-9ed4-3229c638a89f"), "Album" },
                    { new Guid("2c2228f6-fc67-43a0-93e3-d85c60e128ee"), "EP" },
                    { new Guid("b60a288a-b223-4ce8-804d-d6c86c6d62af"), "Single" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TrackArtist_Artist",
                table: "TrackArtist",
                column: "ArtistID",
                principalTable: "Artist",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackArtist_Track",
                table: "TrackArtist",
                column: "TrackID",
                principalTable: "Track",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
