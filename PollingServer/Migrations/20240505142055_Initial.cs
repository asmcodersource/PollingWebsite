using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollingServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectFieldQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectFieldQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextFieldQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldPlaceholder = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFieldQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    RegistrationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Polls_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectFieldResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectFieldQuestionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnswerTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectFieldResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectFieldResponse_SelectFieldQuestion_SelectFieldQuestionId",
                        column: x => x.SelectFieldQuestionId,
                        principalTable: "SelectFieldQuestion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SelectFieldResponse_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextFieldResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextFieldQuestionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnswerTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFieldResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextFieldResponse_TextFieldQuestion_TextFieldQuestionId",
                        column: x => x.TextFieldQuestionId,
                        principalTable: "TextFieldQuestion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TextFieldResponse_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBan_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PollAllowedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PollId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollAllowedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollAllowedUsers_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PollAllowedUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    OrderRate = table.Column<int>(type: "int", nullable: false),
                    SelectQuestionId = table.Column<int>(type: "int", nullable: true),
                    TextQuestionId = table.Column<int>(type: "int", nullable: true),
                    PollId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollQuestion_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PollQuestion_SelectFieldQuestion_SelectQuestionId",
                        column: x => x.SelectQuestionId,
                        principalTable: "SelectFieldQuestion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PollQuestion_TextFieldQuestion_TextQuestionId",
                        column: x => x.TextQuestionId,
                        principalTable: "TextFieldQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollAllowedUsers_PollId",
                table: "PollAllowedUsers",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_PollAllowedUsers_UserId",
                table: "PollAllowedUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestion_PollId",
                table: "PollQuestion",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestion_SelectQuestionId",
                table: "PollQuestion",
                column: "SelectQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestion_TextQuestionId",
                table: "PollQuestion",
                column: "TextQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_OwnerId",
                table: "Polls",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectFieldResponse_SelectFieldQuestionId",
                table: "SelectFieldResponse",
                column: "SelectFieldQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectFieldResponse_UserId",
                table: "SelectFieldResponse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TextFieldResponse_TextFieldQuestionId",
                table: "TextFieldResponse",
                column: "TextFieldQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TextFieldResponse_UserId",
                table: "TextFieldResponse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBan_UserId",
                table: "UserBan",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PollAllowedUsers");

            migrationBuilder.DropTable(
                name: "PollQuestion");

            migrationBuilder.DropTable(
                name: "SelectFieldResponse");

            migrationBuilder.DropTable(
                name: "TextFieldResponse");

            migrationBuilder.DropTable(
                name: "UserBan");

            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "SelectFieldQuestion");

            migrationBuilder.DropTable(
                name: "TextFieldQuestion");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
