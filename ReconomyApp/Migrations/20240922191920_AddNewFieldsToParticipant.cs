using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReconomyApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Participants",
                newName: "ID");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishDate",
                table: "Participants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Participants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WorkCommitment",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "WorkCommitment",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Participants",
                newName: "Id");
        }
    }
}
