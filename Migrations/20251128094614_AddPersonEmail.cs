using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseEFApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "person",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "person");
        }
    }
}
