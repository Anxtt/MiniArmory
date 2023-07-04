using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniArmory.Data.Migrations
{
    public partial class ImageDataTableContentTypeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Images",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Images");
        }
    }
}
