using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CascadeDeleteArtistArtistTracks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("6a18014c-e204-468d-b428-4697ee7a19f1"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("83c9a757-acaf-46c6-9df9-fadf831525b3"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("866a0883-913b-435c-8242-e22db5cc90dc"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("f0f630c5-e9a7-4b87-a319-7aee721c925a"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("11e83471-6f24-47a0-8888-6b71ca29ad1e"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("9464a8d7-4a4b-4b04-a3ef-9f752ee2d75c"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("cc05a77b-ab2d-4560-9dae-ffd5171f8b9c"));

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
