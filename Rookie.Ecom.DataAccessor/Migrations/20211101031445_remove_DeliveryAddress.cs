using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rookie.Ecom.DataAccessor.Migrations
{
    public partial class remove_DeliveryAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Order");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 1, 10, 14, 45, 103, DateTimeKind.Local).AddTicks(4455),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 10, 30, 0, 12, 28, 21, DateTimeKind.Local).AddTicks(861));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 30, 0, 12, 28, 21, DateTimeKind.Local).AddTicks(861),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 11, 1, 10, 14, 45, 103, DateTimeKind.Local).AddTicks(4455));

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Order",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
