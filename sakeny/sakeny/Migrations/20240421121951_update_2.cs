using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sakeny.Migrations
{
    public partial class update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PostsTblPostId",
                table: "POST_FEATURES_TBL",
                type: "numeric(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "POST_ID",
                table: "FEATURES_TBL",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_POST_FEATURES_TBL_PostsTblPostId",
                table: "POST_FEATURES_TBL",
                column: "PostsTblPostId");

            migrationBuilder.CreateIndex(
                name: "IX_FEATURES_TBL_POST_ID",
                table: "FEATURES_TBL",
                column: "POST_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FEATURES_TBL_POSTS_TBL_POST_ID",
                table: "FEATURES_TBL",
                column: "POST_ID",
                principalTable: "POSTS_TBL",
                principalColumn: "POST_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_POST_FEATURES_TBL_POSTS_TBL_PostsTblPostId",
                table: "POST_FEATURES_TBL",
                column: "PostsTblPostId",
                principalTable: "POSTS_TBL",
                principalColumn: "POST_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FEATURES_TBL_POSTS_TBL_POST_ID",
                table: "FEATURES_TBL");

            migrationBuilder.DropForeignKey(
                name: "FK_POST_FEATURES_TBL_POSTS_TBL_PostsTblPostId",
                table: "POST_FEATURES_TBL");

            migrationBuilder.DropIndex(
                name: "IX_POST_FEATURES_TBL_PostsTblPostId",
                table: "POST_FEATURES_TBL");

            migrationBuilder.DropIndex(
                name: "IX_FEATURES_TBL_POST_ID",
                table: "FEATURES_TBL");

            migrationBuilder.DropColumn(
                name: "PostsTblPostId",
                table: "POST_FEATURES_TBL");

            migrationBuilder.DropColumn(
                name: "POST_ID",
                table: "FEATURES_TBL");
        }
    }
}
