using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OLS.Migrations
{
    /// <inheritdoc />
    public partial class FixedBIDIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BorrowedBooks_BookID",
                table: "BorrowedBooks");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedBooks_BookID",
                table: "BorrowedBooks",
                column: "BookID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BorrowedBooks_BookID",
                table: "BorrowedBooks");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedBooks_BookID",
                table: "BorrowedBooks",
                column: "BookID",
                unique: true);
        }
    }
}
