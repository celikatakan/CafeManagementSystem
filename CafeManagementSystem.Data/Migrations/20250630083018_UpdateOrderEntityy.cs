using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderEntityy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecialRequest",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialRequest",
                table: "Orders");
        }
    }
}
