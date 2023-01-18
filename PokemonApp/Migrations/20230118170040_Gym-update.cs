using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonApp.Migrations
{
    public partial class Gymupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Addresses_AddressId",
                table: "Gyms");

            migrationBuilder.DropIndex(
                name: "IX_Gyms_AddressId",
                table: "Gyms");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Gyms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Gyms_AddressId",
                table: "Gyms",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Addresses_AddressId",
                table: "Gyms",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Addresses_AddressId",
                table: "Gyms");

            migrationBuilder.DropIndex(
                name: "IX_Gyms_AddressId",
                table: "Gyms");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Gyms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gyms_AddressId",
                table: "Gyms",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Addresses_AddressId",
                table: "Gyms",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
