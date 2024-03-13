using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    /// <inheritdoc />
    public partial class AddedChatModelRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatModel_AspNetUsers_ReceiverId",
                table: "ChatModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatModel_AspNetUsers_SenderId",
                table: "ChatModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatModel_Messages_MessageId",
                table: "ChatModel");

            migrationBuilder.DropIndex(
                name: "IX_ChatModel_MessageId",
                table: "ChatModel");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "ChatModel");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "ChatModel",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "ChatModel",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatModel_SenderId",
                table: "ChatModel",
                newName: "IX_ChatModel_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatModel_ReceiverId",
                table: "ChatModel",
                newName: "IX_ChatModel_User1Id");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatModel_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "ChatModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatModel_AspNetUsers_User1Id",
                table: "ChatModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatModel_AspNetUsers_User2Id",
                table: "ChatModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatModel_ChatId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "ChatModel",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "ChatModel",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatModel_User2Id",
                table: "ChatModel",
                newName: "IX_ChatModel_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatModel_User1Id",
                table: "ChatModel",
                newName: "IX_ChatModel_ReceiverId");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "ChatModel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatModel_MessageId",
                table: "ChatModel",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatModel_AspNetUsers_ReceiverId",
                table: "ChatModel",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatModel_AspNetUsers_SenderId",
                table: "ChatModel",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatModel_Messages_MessageId",
                table: "ChatModel",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
