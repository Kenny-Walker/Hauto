using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hauto.Migrations
{
    /// <inheritdoc />
    public partial class HautoV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_Devices_DeviceId",
                table: "Hubs");

            migrationBuilder.DropIndex(
                name: "IX_Hubs_DeviceId",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Hubs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "Hubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_DeviceId",
                table: "Hubs",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_Devices_DeviceId",
                table: "Hubs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
