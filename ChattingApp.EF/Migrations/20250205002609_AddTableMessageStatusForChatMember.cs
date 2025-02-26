using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApp.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddTableMessageStatusForChatMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMember_Chat_TwosomeChatId",
                table: "ChatMember");

            migrationBuilder.DropIndex(
                name: "IX_ChatMember_TwosomeChatId",
                table: "ChatMember");

            migrationBuilder.DropColumn(
                name: "TwosomeChatId",
                table: "ChatMember");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeletedForSender",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MessageStatusForChatMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    IsRecieve = table.Column<bool>(type: "bit", nullable: false),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageStatusForChatMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageStatusForChatMember_ChatMember_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ChatMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageStatusForChatMember_Messages_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageStatusForChatMember_MemberId",
                table: "MessageStatusForChatMember",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageStatusForChatMember");

            migrationBuilder.DropColumn(
                name: "IsDeletedForSender",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "TwosomeChatId",
                table: "ChatMember",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMember_TwosomeChatId",
                table: "ChatMember",
                column: "TwosomeChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMember_Chat_TwosomeChatId",
                table: "ChatMember",
                column: "TwosomeChatId",
                principalTable: "Chat",
                principalColumn: "Id");
        }
    }
}
