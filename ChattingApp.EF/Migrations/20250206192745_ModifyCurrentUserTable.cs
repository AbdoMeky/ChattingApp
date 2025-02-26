using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChattingApp.EF.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCurrentUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentOnlineUsers",
                table: "CurrentOnlineUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentOnlineUsers",
                table: "CurrentOnlineUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentOnlineUsers_UserID",
                table: "CurrentOnlineUsers",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentOnlineUsers",
                table: "CurrentOnlineUsers");

            migrationBuilder.DropIndex(
                name: "IX_CurrentOnlineUsers_UserID",
                table: "CurrentOnlineUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentOnlineUsers",
                table: "CurrentOnlineUsers",
                columns: new[] { "UserID", "Id" });
        }
    }
}
