using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class _008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostID",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "Tags",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_PostID",
                table: "Tags",
                newName: "IX_Tags_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Tags",
                newName: "PostID");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_PostId",
                table: "Tags",
                newName: "IX_Tags_PostID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_PostID",
                table: "Tags",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
