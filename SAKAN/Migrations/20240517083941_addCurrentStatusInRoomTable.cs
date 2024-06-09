using Microsoft.EntityFrameworkCore.Migrations;

namespace SAKAN.Migrations
{
    public partial class addCurrentStatusInRoomTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CurrentState",
                table: "Room",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "Room");
        }
    }
}
