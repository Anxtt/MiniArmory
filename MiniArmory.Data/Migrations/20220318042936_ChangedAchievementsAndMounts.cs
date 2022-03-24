using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniArmory.Data.Migrations
{
    public partial class ChangedAchievementsAndMounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanFly",
                table: "Mounts");

            migrationBuilder.AlterColumn<short>(
                name: "GroundSpeed",
                table: "Mounts",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "FlyingSpeed",
                table: "Mounts",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Mounts",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Achievements",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Mounts");

            migrationBuilder.AlterColumn<int>(
                name: "GroundSpeed",
                table: "Mounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "FlyingSpeed",
                table: "Mounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<bool>(
                name: "CanFly",
                table: "Mounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
