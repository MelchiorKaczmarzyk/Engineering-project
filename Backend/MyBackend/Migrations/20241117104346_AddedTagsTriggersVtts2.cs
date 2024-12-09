using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedTagsTriggersVtts2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Sessions_SessionId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_Sessions_SessionId",
                table: "Triggers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vtts_Sessions_SessionId",
                table: "Vtts");

            migrationBuilder.DropIndex(
                name: "IX_Vtts_SessionId",
                table: "Vtts");

            migrationBuilder.DropIndex(
                name: "IX_Triggers_SessionId",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Tags_SessionId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Vtts");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Tags");

            migrationBuilder.AddColumn<string>(
                name: "Triggers",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vtts",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Triggers",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Vtts",
                table: "Sessions");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Vtts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Triggers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Tags",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vtts_SessionId",
                table: "Vtts",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_SessionId",
                table: "Triggers",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_SessionId",
                table: "Tags",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Sessions_SessionId",
                table: "Tags",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_Sessions_SessionId",
                table: "Triggers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vtts_Sessions_SessionId",
                table: "Vtts",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");
        }
    }
}
