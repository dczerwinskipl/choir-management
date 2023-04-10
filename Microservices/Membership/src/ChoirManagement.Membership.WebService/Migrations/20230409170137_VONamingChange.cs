using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoirManagement.Membership.WebService.Migrations
{
    /// <inheritdoc />
    public partial class VONamingChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalData_PhoneNumber_Value",
                table: "Members",
                newName: "PersonalData_PhoneNumber_Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalData_PhoneNumber_Number",
                table: "Members",
                newName: "PersonalData_PhoneNumber_Value");
        }
    }
}
