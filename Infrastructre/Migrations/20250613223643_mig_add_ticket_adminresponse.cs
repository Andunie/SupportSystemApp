using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportSystemApp.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_ticket_adminresponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminResponse",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminResponse",
                table: "Tickets");
        }
    }
}
