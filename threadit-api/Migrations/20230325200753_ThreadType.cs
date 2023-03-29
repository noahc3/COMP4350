using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreaditAPI.Migrations
{
	/// <inheritdoc />
	public partial class ThreadType : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "ThreadType",
				table: "Threads",
				type: "text",
				nullable: false,
				defaultValue: Constants.ThreadTypes.TEXT);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ThreadType",
				table: "Threads");
		}
	}
}
