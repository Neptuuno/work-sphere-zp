using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    /// <inheritdoc />
    public partial class AddedManyToManyChatUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatModel_AspNetUsers_User1Id",
                table: "ChatModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatModel_AspNetUsers_User2Id",
                table: "ChatModel");

            migrationBuilder.DropIndex(
                name: "IX_ChatModel_User1Id",
                table: "ChatModel");

            migrationBuilder.DropIndex(
                name: "IX_ChatModel_User2Id",
                table: "ChatModel");

            migrationBuilder.DropColumn(
                name: "User1Id",
                table: "ChatModel");

            migrationBuilder.DropColumn(
                name: "User2Id",
                table: "ChatModel");

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ChatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.UserId, x.ChatId });
                    table.ForeignKey(
                        name: "FK_ChatUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_ChatModel_ChatId",
                        column: x => x.ChatId,
                        principalTable: "ChatModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_ChatId",
                table: "ChatUser",
                column: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.AddColumn<string>(
                name: "User1Id",
                table: "ChatModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User2Id",
                table: "ChatModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatModel_User1Id",
                table: "ChatModel",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatModel_User2Id",
                table: "ChatModel",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatModel_AspNetUsers_User1Id",
                table: "ChatModel",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatModel_AspNetUsers_User2Id",
                table: "ChatModel",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
