using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalChat.Migrations
{
    public partial class aggiuntoRoomIdaChatTexts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "ChatTexts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "ChatTexts");
        }
    }
}
