using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReconomyApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameFinishDateToEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinishDate",
                table: "Participants",
                newName: "EndDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Participants",
                newName: "FinishDate");
        }
    }
}
