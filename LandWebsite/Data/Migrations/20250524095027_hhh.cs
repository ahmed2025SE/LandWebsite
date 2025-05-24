using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Hhh : Migration 
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    ownerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ownerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.ownerId);
                });

            migrationBuilder.CreateTable(
                name: "Lands",
                columns: table => new
                {
                    landId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    area = table.Column<double>(type: "float", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lands", x => x.landId);
                    table.ForeignKey(
                        name: "FK_Lands_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "ownerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_OwnerId",
                table: "Lands",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lands");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
