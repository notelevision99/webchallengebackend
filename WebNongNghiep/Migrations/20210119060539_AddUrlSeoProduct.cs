using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNongNghiep.Migrations
{
    public partial class AddUrlSeoProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlSeo",
                table: "Products",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlSeo",
                table: "Products");
        }
    }
}
