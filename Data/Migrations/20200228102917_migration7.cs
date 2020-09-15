using Microsoft.EntityFrameworkCore.Migrations;

namespace CatCar.Data.Migrations
{
    public partial class migration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Car_CarId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_CarId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Car");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Car",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarId",
                table: "Car",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Car_CarId",
                table: "Car",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
