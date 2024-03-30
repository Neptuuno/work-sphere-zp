using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedPostTitleLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastChatId",
                table: "AspNetUsers",
                newName: "LastOpenedChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastOpenedChatId",
                table: "AspNetUsers",
                newName: "LastChatId");
        }
    }
}
