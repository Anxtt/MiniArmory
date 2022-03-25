using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniArmory.Data.Migrations
{
    public partial class IsRacialColumnChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRacial",
                table: "Spells");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Spells",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Spells");

            migrationBuilder.AddColumn<bool>(
                name: "IsRacial",
                table: "Spells",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
