using System;
using System.Collections.Generic;
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
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Threads_ThreadId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_OwnerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Spools_UserSettings_UserSettingsId",
                table: "Spools");

            migrationBuilder.DropForeignKey(
                name: "FK_Spools_Users_OwnerId",
                table: "Spools");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Spools_SpoolId",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Users_OwnerId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "ModeratorProfileSpool");

            migrationBuilder.DropTable(
                name: "Moderators");

            migrationBuilder.DropIndex(
                name: "IX_Threads_OwnerId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_SpoolId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Spools_OwnerId",
                table: "Spools");

            migrationBuilder.DropIndex(
                name: "IX_Spools_UserSettingsId",
                table: "Spools");

            migrationBuilder.DropIndex(
                name: "IX_Comments_OwnerId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ThreadId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserSettingsId",
                table: "Spools");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Spins",
                table: "UserSettings",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Interests",
                table: "UserSettings",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<List<string>>(
                name: "SpoolsJoined",
                table: "UserSettings",
                type: "text[]",
                nullable: false);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Stitches",
                table: "Threads",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SpoolId",
                table: "Threads",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Rips",
                table: "Threads",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<List<string>>(
                name: "Moderators",
                table: "Spools",
                type: "text[]",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "ThreadId",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ParentCommentId",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpoolsJoined",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "Moderators",
                table: "Spools");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Spins",
                table: "UserSettings",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "Interests",
                table: "UserSettings",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "Stitches",
                table: "Threads",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "SpoolId",
                table: "Threads",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Rips",
                table: "Threads",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AddColumn<string>(
                name: "UserSettingsId",
                table: "Spools",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ThreadId",
                table: "Comments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Moderators",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeratorProfileSpool",
                columns: table => new
                {
                    CreatedSpoolsId = table.Column<string>(type: "text", nullable: false),
                    ModeratorsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeratorProfileSpool", x => new { x.CreatedSpoolsId, x.ModeratorsId });
                    table.ForeignKey(
                        name: "FK_ModeratorProfileSpool_Moderators_ModeratorsId",
                        column: x => x.ModeratorsId,
                        principalTable: "Moderators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeratorProfileSpool_Spools_CreatedSpoolsId",
                        column: x => x.CreatedSpoolsId,
                        principalTable: "Spools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Threads_OwnerId",
                table: "Threads",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_SpoolId",
                table: "Threads",
                column: "SpoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Spools_OwnerId",
                table: "Spools",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Spools_UserSettingsId",
                table: "Spools",
                column: "UserSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OwnerId",
                table: "Comments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ThreadId",
                table: "Comments",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorProfileSpool_ModeratorsId",
                table: "ModeratorProfileSpool",
                column: "ModeratorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Threads_ThreadId",
                table: "Comments",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_OwnerId",
                table: "Comments",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Spools_UserSettings_UserSettingsId",
                table: "Spools",
                column: "UserSettingsId",
                principalTable: "UserSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Spools_Users_OwnerId",
                table: "Spools",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Spools_SpoolId",
                table: "Threads",
                column: "SpoolId",
                principalTable: "Spools",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Users_OwnerId",
                table: "Threads",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
