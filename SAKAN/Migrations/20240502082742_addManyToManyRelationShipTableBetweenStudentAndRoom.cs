using Microsoft.EntityFrameworkCore.Migrations;

namespace SAKAN.Migrations
{
    public partial class addManyToManyRelationShipTableBetweenStudentAndRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Room_RoomId",
                table: "User");

            migrationBuilder.CreateTable(
                name: "StudentInRoom",
                columns: table => new
                {
                    StuedntId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInRoom", x => new { x.StuedntId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_StudentInRoom_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentInRoom_User_StuedntId",
                        column: x => x.StuedntId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentInRoom_RoomId",
                table: "StudentInRoom",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Room_RoomId",
                table: "User",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Room_RoomId",
                table: "User");

            migrationBuilder.DropTable(
                name: "StudentInRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Room_RoomId",
                table: "User",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
