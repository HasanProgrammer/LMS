using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class CreateTablesVersion_1_0_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
