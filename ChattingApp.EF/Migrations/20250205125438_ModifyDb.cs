using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApp.EF.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMember_Chat_ChatId",
                table: "ChatMember");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_User_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageStatusForChatMember_Messages_MemberId",
                table: "MessageStatusForChatMember");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageStatusForChatMember_MessageId",
                table: "MessageStatusForChatMember",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MemberId",
                table: "Messages",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMember_Chat_ChatId",
                table: "ChatMember",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatMember_MemberId",
                table: "Messages",
                column: "MemberId",
                principalTable: "ChatMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageStatusForChatMember_Messages_MessageId",
                table: "MessageStatusForChatMember",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMember_Chat_ChatId",
                table: "ChatMember");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatMember_MemberId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageStatusForChatMember_Messages_MessageId",
                table: "MessageStatusForChatMember");

            migrationBuilder.DropIndex(
                name: "IX_MessageStatusForChatMember_MessageId",
                table: "MessageStatusForChatMember");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MemberId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMember_Chat_ChatId",
                table: "ChatMember",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_User_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageStatusForChatMember_Messages_MemberId",
                table: "MessageStatusForChatMember",
                column: "MemberId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
