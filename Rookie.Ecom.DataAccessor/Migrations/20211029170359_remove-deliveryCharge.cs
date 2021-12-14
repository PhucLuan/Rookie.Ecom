using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rookie.Ecom.DataAccessor.Migrations
{
    public partial class removedeliveryCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCharge",
                table: "Order");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 30, 0, 3, 58, 734, DateTimeKind.Local).AddTicks(9287),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 10, 26, 15, 44, 47, 512, DateTimeKind.Local).AddTicks(6016));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 26, 15, 44, 47, 512, DateTimeKind.Local).AddTicks(6016),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 10, 30, 0, 3, 58, 734, DateTimeKind.Local).AddTicks(9287));

            migrationBuilder.AddColumn<double>(
                name: "DeliveryCharge",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
