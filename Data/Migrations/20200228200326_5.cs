using Microsoft.EntityFrameworkCore.Migrations;

namespace CatCar.Data.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CarNo",
                table: "Car",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CarNo",
                table: "Car",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
