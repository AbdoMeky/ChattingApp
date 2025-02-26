using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApp.EF.Migrations
{
    /// <inheritdoc />
    public partial class ModifyChatsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwoSomeChatUSer");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Messages",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Messages",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TwoSomeChatUSer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TwosomeChatID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoSomeChatUSer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwoSomeChatUSer_Chat_TwosomeChatID",
                        column: x => x.TwosomeChatID,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwoSomeChatUSer_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwoSomeChatUSer_ApplicationUserId",
                table: "TwoSomeChatUSer",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TwoSomeChatUSer_TwosomeChatID",
                table: "TwoSomeChatUSer",
                column: "TwosomeChatID");
        }
    }
}
