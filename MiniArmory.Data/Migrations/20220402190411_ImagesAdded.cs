using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniArmory.Data.Migrations
{
    public partial class ImagesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialisation",
                table: "Classes",
                newName: "SpecialisationName");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Races",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClassImage",
                table: "Classes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Characters",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Achievements",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "ClassImage",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Achievements");

            migrationBuilder.RenameColumn(
                name: "SpecialisationName",
                table: "Classes",
                newName: "Specialisation");
        }
    }
}
