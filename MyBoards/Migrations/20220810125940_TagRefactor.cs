using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards.Migrations
{
    public partial class TagRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
