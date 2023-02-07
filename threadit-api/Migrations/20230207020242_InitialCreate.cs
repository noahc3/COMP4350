using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreaditAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateCreated", "Email", "PasswordHash", "Username" },
                values: new object[] { "9bb973d3-290f-46dc-b72c-219403a3985d", new DateTime(2023, 2, 7, 2, 2, 42, 508, DateTimeKind.Utc).AddTicks(7665), "test@gmail.com", "testPassword", "testAccount" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "9bb973d3-290f-46dc-b72c-219403a3985d");
        }
    }
}
