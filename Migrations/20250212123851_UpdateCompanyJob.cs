using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalJob.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SKilLevel",
                table: "CompanyJobs",
                newName: "SkillLevel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkillLevel",
                table: "CompanyJobs",
                newName: "SKilLevel");
        }
    }
}
