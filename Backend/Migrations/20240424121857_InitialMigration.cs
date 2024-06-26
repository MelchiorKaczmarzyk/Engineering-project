using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    System = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    MaxNumberOfPlayers = table.Column<int>(type: "INTEGER", nullable: false),
                    GmId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Gms_GmId",
                        column: x => x.GmId,
                        principalTable: "Gms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSession",
                columns: table => new
                {
                    PlayersId = table.Column<string>(type: "TEXT", nullable: false),
                    SessionsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSession", x => new { x.PlayersId, x.SessionsId });
                    table.ForeignKey(
                        name: "FK_PlayerSession_Players_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSession_Sessions_SessionsId",
                        column: x => x.SessionsId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Gms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "1", "Gm1" },
                    { "2", "Gm2" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "1", "Player1" },
                    { "2", "Player2" },
                    { "3", "Player3" },
                    { "4", "Player4" }
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "Description", "GmId", "MaxNumberOfPlayers", "System", "Tags", "Title" },
                values: new object[,]
                {
                    { "1", "Description", "1", 4, "d&d", "tag1, tag2, tag3", "title1" },
                    { "2", "Description", "1", 4, "d&d", "tag1, tag2, tag3", "title2" },
                    { "3", "Description", "2", 4, "d&d", "tag1, tag2, tag3", "title3" },
                    { "4", "Description", "2", 6, "Veasen", "tag2, tag3", "title4" }
                });

            migrationBuilder.InsertData(
                table: "PlayerSession",
                columns: new[] { "PlayersId", "SessionsId" },
                values: new object[,]
                {
                    { "1", "1" },
                    { "1", "4" },
                    { "2", "1" },
                    { "2", "2" },
                    { "2", "4" },
                    { "3", "2" },
                    { "3", "3" },
                    { "3", "4" },
                    { "4", "3" },
                    { "4", "4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSession_SessionsId",
                table: "PlayerSession",
                column: "SessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_GmId",
                table: "Sessions",
                column: "GmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerSession");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Gms");
        }
    }
}
