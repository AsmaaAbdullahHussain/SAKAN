using Microsoft.EntityFrameworkCore.Migrations;
using SAKAN.Constants;
using System;

namespace SAKAN.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), Role.Student, Role.Student.ToUpper(), Guid.NewGuid().ToString() }

                );
            migrationBuilder.InsertData(
               table: "Role",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), Role.Owner, Role.Owner.ToUpper(), Guid.NewGuid().ToString() }

               );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete * from Role");

        }
    }
}
