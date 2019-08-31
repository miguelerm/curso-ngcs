using Microsoft.EntityFrameworkCore.Migrations;

namespace Abs.BooksCatalog.Service.Migrations
{
    public partial class Covers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cover",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    BookId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cover", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Cover_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cover_BookId",
                table: "Cover",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cover");
        }
    }
}
