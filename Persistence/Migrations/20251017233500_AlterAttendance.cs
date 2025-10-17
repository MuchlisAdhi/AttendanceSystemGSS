using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "tbtattendance");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "tbtattendance");

            migrationBuilder.AddColumn<double>(
                name: "CheckInLatitude",
                table: "tbtattendance",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckInLongitude",
                table: "tbtattendance",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLatitude",
                table: "tbtattendance",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLongitude",
                table: "tbtattendance",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "tbtattendance",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "tbtattendance",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInLatitude",
                table: "tbtattendance");

            migrationBuilder.DropColumn(
                name: "CheckInLongitude",
                table: "tbtattendance");

            migrationBuilder.DropColumn(
                name: "CheckOutLatitude",
                table: "tbtattendance");

            migrationBuilder.DropColumn(
                name: "CheckOutLongitude",
                table: "tbtattendance");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "tbtattendance");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tbtattendance");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "tbtattendance",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "tbtattendance",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
