using Microsoft.EntityFrameworkCore.Migrations;

namespace SAKAN.Migrations
{
    public partial class OwnerBuildingRelationShipOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Building",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Building_OwnerId",
                table: "Building",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Building_User_OwnerId",
                table: "Building",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Building_User_OwnerId",
                table: "Building");

            migrationBuilder.DropIndex(
                name: "IX_Building_OwnerId",
                table: "Building");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Building");
        }
    }
}
