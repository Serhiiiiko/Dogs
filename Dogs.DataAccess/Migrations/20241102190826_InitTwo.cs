using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dogs.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_Dogs",
                table: "Dogs",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Dogs",
                table: "Dogs");
        }
    }
}
