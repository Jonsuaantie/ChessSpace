using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessSpace.Migrations
{
    /// <inheritdoc />
    public partial class playerjoined : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Player1Joined",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Player2Joined",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Player1Joined",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Player2Joined",
                table: "Games");
        }
    }
}
