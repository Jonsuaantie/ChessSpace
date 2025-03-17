using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessSpace.Migrations
{
    /// <inheritdoc />
    public partial class addedboardstateandcurrenplayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoardState",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentPlayer",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardState",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CurrentPlayer",
                table: "Games");
        }
    }
}
