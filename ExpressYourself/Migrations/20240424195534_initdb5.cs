using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressYourself.Migrations
{
    /// <inheritdoc />
    public partial class initdb5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IPAddresses_CountryId",
                table: "IPAddresses",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IPAddresses_Countries_CountryId",
                table: "IPAddresses",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IPAddresses_Countries_CountryId",
                table: "IPAddresses");

            migrationBuilder.DropIndex(
                name: "IX_IPAddresses_CountryId",
                table: "IPAddresses");
        }
    }
}
