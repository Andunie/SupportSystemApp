using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportSystemApp.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_notification_senderuserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SenderUserID",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderUserID",
                table: "Notifications");
        }
    }
}
