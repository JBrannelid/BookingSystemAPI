using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Costumers_CustomerId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Costumers",
                table: "Costumers");

            migrationBuilder.RenameTable(
                name: "Costumers",
                newName: "Customers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Customers_CustomerId",
                table: "Bookings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Customers_CustomerId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Costumers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Costumers",
                table: "Costumers",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Costumers_CustomerId",
                table: "Bookings",
                column: "CustomerId",
                principalTable: "Costumers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
