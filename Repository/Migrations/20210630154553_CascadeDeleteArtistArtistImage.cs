using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CascadeDeleteArtistArtistImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistImage_Artist",
                table: "ArtistImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistImage_Image",
                table: "ArtistImage");

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
                    { new Guid("41b86ffd-c60b-4cda-8903-76bb4e1f1592"), "Country" },
                    { new Guid("3958a68b-9239-4d6b-ab00-6c6689062c43"), "Region" },
                    { new Guid("a888f760-8449-4c06-9f44-08a858122e8f"), "City" },
                    { new Guid("734467fc-5996-4f04-a975-33c1382395f9"), "Street" }
                });

            migrationBuilder.InsertData(
                table: "AlbumType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("17b77bdf-45f5-43ee-b181-9b5db9d5fa8d"), "Album" },
                    { new Guid("d0d07794-4e7c-4b3b-8edc-383fa164c4d8"), "EP" },
                    { new Guid("e0b201c2-3bc7-4b7e-ad16-765d52c32589"), "Single" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistImage_Artist",
                table: "ArtistImage",
                column: "ArtistID",
                principalTable: "Artist",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistImage_Image",
                table: "ArtistImage",
                column: "ImageID",
                principalTable: "Image",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistImage_Artist",
                table: "ArtistImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistImage_Image",
                table: "ArtistImage");

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("3958a68b-9239-4d6b-ab00-6c6689062c43"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("41b86ffd-c60b-4cda-8903-76bb4e1f1592"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("734467fc-5996-4f04-a975-33c1382395f9"));

            migrationBuilder.DeleteData(
                table: "AddressElementType",
                keyColumn: "ID",
                keyValue: new Guid("a888f760-8449-4c06-9f44-08a858122e8f"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("17b77bdf-45f5-43ee-b181-9b5db9d5fa8d"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("d0d07794-4e7c-4b3b-8edc-383fa164c4d8"));

            migrationBuilder.DeleteData(
                table: "AlbumType",
                keyColumn: "ID",
                keyValue: new Guid("e0b201c2-3bc7-4b7e-ad16-765d52c32589"));

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
                name: "FK_ArtistImage_Artist",
                table: "ArtistImage",
                column: "ArtistID",
                principalTable: "Artist",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistImage_Image",
                table: "ArtistImage",
                column: "ImageID",
                principalTable: "Image",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
