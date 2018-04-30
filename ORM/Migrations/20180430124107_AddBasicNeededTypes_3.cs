using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ORM.Migrations
{
    public partial class AddBasicNeededTypes_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCharacteristics");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Items",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Items",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Items",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "ItemCharacteristics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<string>(maxLength: 25, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ItemId = table.Column<Guid>(nullable: false),
                    Model = table.Column<string>(maxLength: 25, nullable: true),
                    Size = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCharacteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCharacteristics_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCharacteristics_ItemId",
                table: "ItemCharacteristics",
                column: "ItemId",
                unique: true);
        }
    }
}
