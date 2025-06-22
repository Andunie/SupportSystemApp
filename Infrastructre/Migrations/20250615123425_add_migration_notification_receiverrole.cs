using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportSystemApp.Migrations
{
    /// <inheritdoc />
    public partial class add_migration_notification_receiverrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverRole",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverRole",
                table: "Notifications");
        }
    }
}
