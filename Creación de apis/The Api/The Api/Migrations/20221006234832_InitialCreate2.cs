using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace The_Api.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TodoItems",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "TodoItems",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TodoItems",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "TodoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
