using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedTagsTriggersVtts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "1", "4" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "2", "1" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "2", "4" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "3", "2" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "3", "3" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "3", "4" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "4", "3" });

            migrationBuilder.DeleteData(
                table: "PlayerSession",
                keyColumns: new[] { "PlayersId", "SessionsId" },
                keyValues: new object[] { "4", "4" });

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Gms",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Gms",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Systems",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Systems",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Systems",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Systems",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Triggers_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vtts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vtts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vtts_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_SessionId",
                table: "Tags",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_SessionId",
                table: "Triggers",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vtts_SessionId",
                table: "Vtts",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Triggers");

            migrationBuilder.DropTable(
                name: "Vtts");

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
                table: "Systems",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "1", "d&d 5e" },
                    { "2", "Veasen" },
                    { "3", "Things from the Flood" },
                    { "4", "DC20" }
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "Description", "GmId", "MaxNumberOfPlayers", "PicturePath", "SystemId", "Tags", "Title" },
                values: new object[,]
                {
                    { "1", "Description", "1", 4, "", "1", "tag1, tag2, tag3", "title1" },
                    { "2", "Description", "1", 4, "", "2", "tag1, tag2, tag3", "title2" },
                    { "3", "Description", "2", 4, "", "3", "tag1, tag2, tag3", "title3" },
                    { "4", "Description", "2", 6, "", "4", "tag2, tag3", "title4" }
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
        }
    }
}
