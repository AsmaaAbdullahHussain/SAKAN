using Microsoft.EntityFrameworkCore.Migrations;

namespace SAKAN.Migrations
{
    public partial class RelationshipsBetweenBuildingAndFlatAndRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlatId",
                table: "Room",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Flat",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room_FlatId",
                table: "Room",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_Flat_BuildingId",
                table: "Flat",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flat_Building_BuildingId",
                table: "Flat",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Flat_FlatId",
                table: "Room",
                column: "FlatId",
                principalTable: "Flat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flat_Building_BuildingId",
                table: "Flat");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Flat_FlatId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_FlatId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Flat_BuildingId",
                table: "Flat");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Flat");
        }
    }
}
