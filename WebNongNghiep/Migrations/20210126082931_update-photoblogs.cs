using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNongNghiep.Migrations
{
    public partial class updatephotoblogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoBlog_Blogs_BlogId",
                table: "PhotoBlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhotoBlog",
                table: "PhotoBlog");

            migrationBuilder.RenameTable(
                name: "PhotoBlog",
                newName: "PhotoBlogs");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoBlog_BlogId",
                table: "PhotoBlogs",
                newName: "IX_PhotoBlogs_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhotoBlogs",
                table: "PhotoBlogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoBlogs_Blogs_BlogId",
                table: "PhotoBlogs",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoBlogs_Blogs_BlogId",
                table: "PhotoBlogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhotoBlogs",
                table: "PhotoBlogs");

            migrationBuilder.RenameTable(
                name: "PhotoBlogs",
                newName: "PhotoBlog");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoBlogs_BlogId",
                table: "PhotoBlog",
                newName: "IX_PhotoBlog_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhotoBlog",
                table: "PhotoBlog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoBlog_Blogs_BlogId",
                table: "PhotoBlog",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
