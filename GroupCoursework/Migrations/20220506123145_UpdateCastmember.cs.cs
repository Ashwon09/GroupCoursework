using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupCoursework.Migrations
{
    public partial class UpdateCastmembercs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers",
                columns: new[] { "DVDNumber", "ActorNumber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers",
                column: "DVDNumber");
        }
    }
}
