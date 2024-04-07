using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    /// <inheritdoc />
    public partial class ManuallyAddedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageImages_Contents_ContentId",
                table: "MessageImages");

            migrationBuilder.AlterColumn<int>(
                name: "ContentId",
                table: "MessageImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageImages_Contents_ContentId",
                table: "MessageImages",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageImages_Contents_ContentId",
                table: "MessageImages");

            migrationBuilder.AlterColumn<int>(
                name: "ContentId",
                table: "MessageImages",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageImages_Contents_ContentId",
                table: "MessageImages",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id");
        }
    }
}
