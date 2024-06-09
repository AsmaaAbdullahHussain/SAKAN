using Microsoft.EntityFrameworkCore.Migrations;

namespace SAKAN.Migrations
{
    public partial class AddBuildingFlatAndRoomTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserGender = table.Column<int>(type: "int", nullable: false),
                    Gas = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discreption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfFloor = table.Column<short>(type: "smallint", nullable: false),
                    NumberOfRooms = table.Column<short>(type: "smallint", nullable: false),
                    ThereIsWasher = table.Column<bool>(type: "bit", nullable: false),
                    WasherType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ThereIsHeater = table.Column<bool>(type: "bit", nullable: false),
                    HeaterType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TV = table.Column<bool>(type: "bit", nullable: false),
                    Internet = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfBathroom = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirCondition = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfBeds = table.Column<short>(type: "smallint", nullable: false),
                    NumberOfDisks = table.Column<short>(type: "smallint", nullable: false),
                    NumberOfChairs = table.Column<short>(type: "smallint", nullable: false),
                    NumberOfCupboards = table.Column<short>(type: "smallint", nullable: false),
                    WindowType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ServicesPrice = table.Column<short>(type: "smallint", nullable: false),
                    InsurancePrice = table.Column<short>(type: "smallint", nullable: false),
                    MonthPrice = table.Column<short>(type: "smallint", nullable: false),
                    DayPrice = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropTable(
                name: "Flat");

            migrationBuilder.DropTable(
                name: "Room");
        }
    }
}
