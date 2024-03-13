using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sakeny.Migrations
{
    public partial class Post_fav_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "POST_FAV_TBL",
                columns: table => new
                {
                    POST_FAV_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POST_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    USER_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POST_FAV_TBL", x => x.POST_FAV_ID);
                    table.ForeignKey(
                        name: "FK_POST_FAV_TBL_POSTS_TBL_POST_ID",
                        column: x => x.POST_ID,
                        principalTable: "POSTS_TBL",
                        principalColumn: "POST_ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_POST_FAV_TBL_USERS_TBL_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS_TBL",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.NoAction);
                });
            migrationBuilder.CreateIndex(
               name: "IX_POST_FAV_TBL_POST_ID",
               table: "POST_FAV_TBL",
               column: "POST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_POST_FAV_TBL_USER_ID",
                table: "POST_FAV_TBL",
                column: "USER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POST_FAV_TBL");
        }
    }
}
