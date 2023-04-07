using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoirManagement.Membership.WebService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalData_PESEL_Number = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PersonalData_Name_FirstName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PersonalData_Name_MiddleName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PersonalData_Name_LastName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PersonalData_Address_Street = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PersonalData_Address_HouseNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PersonalData_Address_ApartmentNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    PersonalData_Address_PostalCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    PersonalData_Address_City = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PersonalData_Address_Commune = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PersonalData_Address_County = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PersonalData_Address_Voivodeship = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PersonalData_BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonalData_PhoneNumber_Value = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PersonalData_Email_Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsAnonymised = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
