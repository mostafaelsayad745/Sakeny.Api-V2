using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sakeny.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "FEATURES_INDEX",
                table: "FEATURES_TBL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "FEATURES_INDEX",
                table: "FEATURES_TBL",
                column: "FEATURES_NAME",
                unique: true,
                filter: "[FEATURES_NAME] IS NOT NULL");
        }
    }
}
