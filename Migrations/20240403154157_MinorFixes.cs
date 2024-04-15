using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    /// <inheritdoc />
    public partial class MinorFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserPostModel",
                columns: table => new
                {
                    LikedByUsersId = table.Column<string>(type: "TEXT", nullable: false),
                    LikedPostsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserPostModel", x => new { x.LikedByUsersId, x.LikedPostsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserPostModel_AspNetUsers_LikedByUsersId",
                        column: x => x.LikedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserPostModel_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserPostModel_LikedPostsId",
                table: "ApplicationUserPostModel",
                column: "LikedPostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserPostModel");
            
            
        }
    }
}
