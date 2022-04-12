using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniArmory.Data.Migrations
{
    public partial class RemovedFactionIsCollectedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faction",
                table: "Mounts");

            migrationBuilder.DropColumn(
                name: "IsCollected",
                table: "Mounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Faction",
                table: "Mounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCollected",
                table: "Mounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
