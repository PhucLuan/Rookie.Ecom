using Microsoft.EntityFrameworkCore.Migrations;

namespace Rookie.Ecom.IdentityServer.Migrations
{
    public partial class rename_Published_column_remove_DeliveryAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pubished",
                table: "Addresses",
                newName: "Published");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Published",
                table: "Addresses",
                newName: "Pubished");
        }
    }
}
