using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class Add_ShortDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProShortDescription",
                table: "Products",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProShortDescription",
                table: "Products");
        }
    }
}
