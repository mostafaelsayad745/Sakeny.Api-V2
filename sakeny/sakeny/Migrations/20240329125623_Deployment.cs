using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sakeny.Migrations
{
    public partial class Deployment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FEATURES_TBL",
                columns: table => new
                {
                    FEATURES_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FEATURES_NAME = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FEATURES_TBL", x => x.FEATURES_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_BAN_TBL",
                columns: table => new
                {
                    USER_BAN_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_BAN_NAT_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_BAN_TBL", x => x.USER_BAN_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_CHAT_TBL",
                columns: table => new
                {
                    USER_CHAT_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_CHAT_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    USER_CHAT_TIME = table.Column<TimeSpan>(type: "time", nullable: true),
                    USER_CHAT_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_CHAT_TEXT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USER_CHAT_IMAGE = table.Column<byte[]>(type: "image", nullable: true),
                    USER_CHAT_FROM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_CHAT_TO = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_CHAT_TBL", x => x.USER_CHAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_FEEDBACK_TBL",
                columns: table => new
                {
                    FEEDBACK_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FEEDBACK_TEXT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FEEDBACK_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    FEEDBACK_TIME = table.Column<TimeSpan>(type: "time", nullable: true),
                    FEEDBACK_FROM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FEEDBACK_TO = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FEEDBACKS_TBL", x => x.FEEDBACK_ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS_TBL",
                columns: table => new
                {
                    USER_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USER_PASSWORD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USER_FULL_NAME = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    USER_EMAIL = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    USER_NAT_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USER_GENDER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USER_AGE = table.Column<int>(type: "int", nullable: false),
                    USER_INFO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USER_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USER_ACCOUNT_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS_TBL", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NOTIFICATION_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    MESSAGE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TIMESTAMP = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NOTIFICATION_ID);
                    table.ForeignKey(
                        name: "FK_Notifications_USERS_TBL_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS_TBL",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POSTS_TBL",
                columns: table => new
                {
                    POST_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POST_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    POST_TIME = table.Column<TimeSpan>(type: "time", nullable: true),
                    POST_CATEGORY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_TITLE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_BODY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_AREA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    POST_KITCHENS = table.Column<int>(type: "int", nullable: true),
                    POST_BEDROOMS = table.Column<int>(type: "int", nullable: true),
                    POST_BATHROOMS = table.Column<int>(type: "int", nullable: true),
                    POST_LOOK_SEA = table.Column<bool>(type: "bit", nullable: true),
                    POST_PETS_ALLOW = table.Column<bool>(type: "bit", nullable: true),
                    POST_CURRENCY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_PRICE_AI = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    POST_PRICE_DISPLAY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    POST_PRICE_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_CITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_STATE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_FLOOR = table.Column<int>(type: "int", nullable: true),
                    POST_LATITUDE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_LONGITUDE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_STATUE = table.Column<bool>(type: "bit", nullable: true),
                    PostUserId = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSTS_TBL", x => x.POST_ID);
                    table.ForeignKey(
                        name: "FK_POSTS_TBL_USERS_TBL_PostUserId",
                        column: x => x.PostUserId,
                        principalTable: "USERS_TBL",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "POST_FEATURES_TBL",
                columns: table => new
                {
                    FEATURES_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    POST_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POST_FEATURES_TBL", x => new { x.FEATURES_ID, x.POST_ID });
                    table.ForeignKey(
                        name: "FK_POST_FEATURES_TBL_FEATURES_TBL",
                        column: x => x.FEATURES_ID,
                        principalTable: "FEATURES_TBL",
                        principalColumn: "FEATURES_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_POST_FEATURES_TBL_POSTS_TBL",
                        column: x => x.POST_ID,
                        principalTable: "POSTS_TBL",
                        principalColumn: "POST_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POST_FEEDBACK_TBL",
                columns: table => new
                {
                    POST_FEED_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POST_FEED_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    POST_FEED_TIME = table.Column<TimeSpan>(type: "time", nullable: true),
                    POST_FEED_TEXT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POST_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    USER_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POST_FEED_TBL", x => x.POST_FEED_ID);
                    table.ForeignKey(
                        name: "FK_POST_FEEDBACK_TBL_POSTS_TBL",
                        column: x => x.POST_ID,
                        principalTable: "POSTS_TBL",
                        principalColumn: "POST_ID");
                    table.ForeignKey(
                        name: "FK_POST_FEEDBACK_TBL_USERS_TBL",
                        column: x => x.USER_ID,
                        principalTable: "USERS_TBL",
                        principalColumn: "USER_ID");
                });

            migrationBuilder.CreateTable(
                name: "POST_PIC_TBL",
                columns: table => new
                {
                    POST_PIC_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POST_ID = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    PICTURE = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POST_PIC_TBL", x => x.POST_PIC_ID);
                    table.ForeignKey(
                        name: "FK_POST_PIC_TBL_POSTS_TBL",
                        column: x => x.POST_ID,
                        principalTable: "POSTS_TBL",
                        principalColumn: "POST_ID");
                });

            migrationBuilder.CreateIndex(
                name: "FEATURES_INDEX",
                table: "FEATURES_TBL",
                column: "FEATURES_NAME",
                unique: true,
                filter: "[FEATURES_NAME] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_USER_ID",
                table: "Notifications",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_POST_FAV_TBL_POST_ID",
                table: "POST_FAV_TBL",
                column: "POST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_POST_FAV_TBL_USER_ID",
                table: "POST_FAV_TBL",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_POST_FEATURES_TBL_POST_ID",
                table: "POST_FEATURES_TBL",
                column: "POST_ID");

            migrationBuilder.CreateIndex(
                name: "POST_FEATURES_INDEX",
                table: "POST_FEATURES_TBL",
                columns: new[] { "FEATURES_ID", "POST_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POST_FEEDBACK_TBL_POST_ID",
                table: "POST_FEEDBACK_TBL",
                column: "POST_ID");

            migrationBuilder.CreateIndex(
                name: "POST_FEED_INDEX",
                table: "POST_FEEDBACK_TBL",
                columns: new[] { "USER_ID", "POST_ID" },
                unique: true,
                filter: "[USER_ID] IS NOT NULL AND [POST_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_POST_PIC_TBL_POST_ID",
                table: "POST_PIC_TBL",
                column: "POST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_POSTS_TBL_PostUserId",
                table: "POSTS_TBL",
                column: "PostUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "POST_FAV_TBL");

            migrationBuilder.DropTable(
                name: "POST_FEATURES_TBL");

            migrationBuilder.DropTable(
                name: "POST_FEEDBACK_TBL");

            migrationBuilder.DropTable(
                name: "POST_PIC_TBL");

            migrationBuilder.DropTable(
                name: "USER_BAN_TBL");

            migrationBuilder.DropTable(
                name: "USER_CHAT_TBL");

            migrationBuilder.DropTable(
                name: "USER_FEEDBACK_TBL");

            migrationBuilder.DropTable(
                name: "FEATURES_TBL");

            migrationBuilder.DropTable(
                name: "POSTS_TBL");

            migrationBuilder.DropTable(
                name: "USERS_TBL");
        }
    }
}
