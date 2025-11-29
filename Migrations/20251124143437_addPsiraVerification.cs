using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Secu.Migrations
{
    /// <inheritdoc />
    public partial class addPsiraVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPsiraVerified",
                table: "SecurityOfficers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PsiraCertificateExpiry",
                table: "SecurityOfficers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PsiraNumber",
                table: "SecurityOfficers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PsiraStatus",
                table: "SecurityOfficers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PsiraVerifiedAt",
                table: "SecurityOfficers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPsiraVerified",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PsiraCertificateExpiry",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PsiraNumber",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PsiraStatus",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PsiraVerifiedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPsiraVerified",
                table: "SecurityOfficers");

            migrationBuilder.DropColumn(
                name: "PsiraCertificateExpiry",
                table: "SecurityOfficers");

            migrationBuilder.DropColumn(
                name: "PsiraNumber",
                table: "SecurityOfficers");

            migrationBuilder.DropColumn(
                name: "PsiraStatus",
                table: "SecurityOfficers");

            migrationBuilder.DropColumn(
                name: "PsiraVerifiedAt",
                table: "SecurityOfficers");

            migrationBuilder.DropColumn(
                name: "IsPsiraVerified",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PsiraCertificateExpiry",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PsiraNumber",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PsiraStatus",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PsiraVerifiedAt",
                table: "Companies");
        }
    }
}
