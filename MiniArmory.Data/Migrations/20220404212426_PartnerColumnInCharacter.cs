using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniArmory.Data.Migrations
{
    public partial class PartnerColumnInCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "Characters",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_PartnerId",
                table: "Characters",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Characters_PartnerId",
                table: "Characters",
                column: "PartnerId",
                principalTable: "Characters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Characters_PartnerId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_PartnerId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Characters");
        }
    }
}
