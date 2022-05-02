using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupCoursework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActorNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorSurname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorNumber);
                });

            migrationBuilder.CreateTable(
                name: "DVDCategorys",
                columns: table => new
                {
                    CategoryNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AgeRestricted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DVDCategorys", x => x.CategoryNumber);
                });

            migrationBuilder.CreateTable(
                name: "LoanTypes",
                columns: table => new
                {
                    LoanTypeNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Loan_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanDuration = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanTypes", x => x.LoanTypeNumber);
                });

            migrationBuilder.CreateTable(
                name: "MembershipCategorys",
                columns: table => new
                {
                    MembershipCategoryNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipCategoryDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MembershipCategoryTotalLoans = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipCategorys", x => x.MembershipCategoryNumber);
                });

            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    ProducerNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProducerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producers", x => x.ProducerNumber);
                });

            migrationBuilder.CreateTable(
                name: "Studios",
                columns: table => new
                {
                    StudioNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudioName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studios", x => x.StudioNumber);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserNumber);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MembershipNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipCategoryNumber = table.Column<int>(type: "int", nullable: false),
                    MembershipLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MembershipFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MembershipAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberDOB = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MembershipNumber);
                    table.ForeignKey(
                        name: "FK_Members_MembershipCategorys_MembershipCategoryNumber",
                        column: x => x.MembershipCategoryNumber,
                        principalTable: "MembershipCategorys",
                        principalColumn: "MembershipCategoryNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DVDTitles",
                columns: table => new
                {
                    DVDNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DvdTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProducerNumber = table.Column<int>(type: "int", nullable: false),
                    CategoryNumber = table.Column<int>(type: "int", nullable: false),
                    StudioNumber = table.Column<int>(type: "int", nullable: false),
                    DateReleased = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StandardCharge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PenaltyCharge = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DVDTitles", x => x.DVDNumber);
                    table.ForeignKey(
                        name: "FK_DVDTitles_DVDCategorys_CategoryNumber",
                        column: x => x.CategoryNumber,
                        principalTable: "DVDCategorys",
                        principalColumn: "CategoryNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DVDTitles_Producers_ProducerNumber",
                        column: x => x.ProducerNumber,
                        principalTable: "Producers",
                        principalColumn: "ProducerNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DVDTitles_Studios_StudioNumber",
                        column: x => x.StudioNumber,
                        principalTable: "Studios",
                        principalColumn: "StudioNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CastMembers",
                columns: table => new
                {
                    DVDNumber = table.Column<int>(type: "int", nullable: false),
                    ActorNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastMembers", x => new { x.DVDNumber, x.ActorNumber });
                    table.ForeignKey(
                        name: "FK_CastMembers_Actors_ActorNumber",
                        column: x => x.ActorNumber,
                        principalTable: "Actors",
                        principalColumn: "ActorNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CastMembers_DVDTitles_DVDNumber",
                        column: x => x.DVDNumber,
                        principalTable: "DVDTitles",
                        principalColumn: "DVDNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DVDCopys",
                columns: table => new
                {
                    CopyNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DVDNumber = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    DatePurchased = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DVDCopys", x => x.CopyNumber);
                    table.ForeignKey(
                        name: "FK_DVDCopys_DVDTitles_DVDNumber",
                        column: x => x.DVDNumber,
                        principalTable: "DVDTitles",
                        principalColumn: "DVDNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanTypeNumber = table.Column<int>(type: "int", nullable: false),
                    CopyNumber = table.Column<int>(type: "int", nullable: false),
                    MemberNumber = table.Column<int>(type: "int", nullable: false),
                    DateOut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateReturned = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanNumber);
                    table.ForeignKey(
                        name: "FK_Loans_DVDCopys_CopyNumber",
                        column: x => x.CopyNumber,
                        principalTable: "DVDCopys",
                        principalColumn: "CopyNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_LoanTypes_LoanTypeNumber",
                        column: x => x.LoanTypeNumber,
                        principalTable: "LoanTypes",
                        principalColumn: "LoanTypeNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Members_MemberNumber",
                        column: x => x.MemberNumber,
                        principalTable: "Members",
                        principalColumn: "MembershipNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CastMembers_ActorNumber",
                table: "CastMembers",
                column: "ActorNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DVDCopys_DVDNumber",
                table: "DVDCopys",
                column: "DVDNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DVDTitles_CategoryNumber",
                table: "DVDTitles",
                column: "CategoryNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DVDTitles_ProducerNumber",
                table: "DVDTitles",
                column: "ProducerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DVDTitles_StudioNumber",
                table: "DVDTitles",
                column: "StudioNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_CopyNumber",
                table: "Loans",
                column: "CopyNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanTypeNumber",
                table: "Loans",
                column: "LoanTypeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MemberNumber",
                table: "Loans",
                column: "MemberNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Members_MembershipCategoryNumber",
                table: "Members",
                column: "MembershipCategoryNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CastMembers");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "DVDCopys");

            migrationBuilder.DropTable(
                name: "LoanTypes");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "DVDTitles");

            migrationBuilder.DropTable(
                name: "MembershipCategorys");

            migrationBuilder.DropTable(
                name: "DVDCategorys");

            migrationBuilder.DropTable(
                name: "Producers");

            migrationBuilder.DropTable(
                name: "Studios");
        }
    }
}
