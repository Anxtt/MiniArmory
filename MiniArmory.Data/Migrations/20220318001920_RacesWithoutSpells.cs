using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniArmory.Data.Migrations
{
    public partial class RacesWithoutSpells : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Spells_RaceId",
                table: "Spells");

            migrationBuilder.AlterColumn<int>(
                name: "RaceId",
                table: "Spells",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_RaceId",
                table: "Spells",
                column: "RaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Spells_RaceId",
                table: "Spells");

            migrationBuilder.AlterColumn<int>(
                name: "RaceId",
                table: "Spells",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_RaceId",
                table: "Spells",
                column: "RaceId",
                unique: true);
        }
    }
}
