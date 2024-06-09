using Microsoft.EntityFrameworkCore.Migrations;

namespace SAKAN.Migrations
{
    public partial class StudentLivesInRoomRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RoomId",
                table: "User",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Room_RoomId",
                table: "User",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Room_RoomId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RoomId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "User");
        }
    }
}
