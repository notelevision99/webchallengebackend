using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNongNghiep.Migrations
{
    public partial class addUrlSeoBlogDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlSeoCategoryBlog",
                table: "CategoryBlogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlSeoBlog",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlSeoCategoryBlog",
                table: "CategoryBlogs");

            migrationBuilder.DropColumn(
                name: "UrlSeoBlog",
                table: "Blogs");
        }
    }
}
