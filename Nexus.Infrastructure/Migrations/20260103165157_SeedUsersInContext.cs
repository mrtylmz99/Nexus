using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nexus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsersInContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "ProfilePictureUrl", "ResetCode", "ResetCodeExpires", "Role", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@nexus.com", "System Administrator", true, "$2a$11$FKX1W0VBTO5t.Z01QOsNWePbSsEQttXNrfsoG3k4WHSRdphlHc9SO", null, null, null, 2, null, "admin" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "john@nexus.com", "John Doe", true, "$2a$11$ksou699iZPqNAAjTI8MfeOuaCI/l4DP2ci5p.Fb54MfLaVe/FUkYG", null, null, null, 0, null, "jdoe" },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "alice@nexus.com", "Alice Smith", true, "$2a$11$3689O5gFXRxx3nCYFWwJz.UMPjhSjQmJfYuoHAbHhvNlrxlQNE/p.", null, null, null, 1, null, "asmith" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
