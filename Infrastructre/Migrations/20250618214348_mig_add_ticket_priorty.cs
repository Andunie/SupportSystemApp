using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportSystemApp.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_ticket_priorty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Priorty",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priorty",
                table: "Tickets");
        }
    }
}
