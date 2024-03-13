using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    /// <inheritdoc />
    public partial class FixedNullIssuesWithChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_AspNetUsers_UserId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_ChatModel_ChatId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatModel_ChatId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatModel",
                table: "ChatModel");

            migrationBuilder.RenameTable(
                name: "ChatUser",
                newName: "ChatUsers");

            migrationBuilder.RenameTable(
                name: "ChatModel",
                newName: "Chats");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUser_ChatId",
                table: "ChatUsers",
                newName: "IX_ChatUsers_ChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUsers",
                table: "ChatUsers",
                columns: new[] { "UserId", "ChatId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUsers_AspNetUsers_UserId",
                table: "ChatUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUsers_Chats_ChatId",
                table: "ChatUsers",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUsers_AspNetUsers_UserId",
                table: "ChatUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUsers_Chats_ChatId",
                table: "ChatUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUsers",
                table: "ChatUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "ChatUsers",
                newName: "ChatUser");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "ChatModel");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUsers_ChatId",
                table: "ChatUser",
                newName: "IX_ChatUser_ChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                columns: new[] { "UserId", "ChatId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatModel",
                table: "ChatModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_AspNetUsers_UserId",
                table: "ChatUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_ChatModel_ChatId",
                table: "ChatUser",
                column: "ChatId",
                principalTable: "ChatModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatModel_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "ChatModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
