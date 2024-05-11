using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jina.Domain.Service.Infra.Migrations
{
    /// <inheritdoc />
    public partial class change_23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MultilingualContents_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropIndex(
                name: "IX_MultilingualContents_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropColumn(
                name: "MultilingualTopicId",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropColumn(
                name: "MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropColumn(
                name: "MultilingualTopicTenantId",
                schema: "language",
                table: "MultilingualContents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MultilingualTopicId",
                schema: "language",
                table: "MultilingualContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents",
                type: "nvarchar(5)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MultilingualTopicTenantId",
                schema: "language",
                table: "MultilingualContents",
                type: "nvarchar(5)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MultilingualContents_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents",
                columns: new[] { "MultilingualTopicTenantId", "MultilingualTopicId", "MultilingualTopicPrimaryCultureType" });

            migrationBuilder.AddForeignKey(
                name: "FK_MultilingualContents_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents",
                columns: new[] { "MultilingualTopicTenantId", "MultilingualTopicId", "MultilingualTopicPrimaryCultureType" },
                principalSchema: "language",
                principalTable: "MultilingualTopics",
                principalColumns: new[] { "TenantId", "Id", "PrimaryCultureType" });
        }
    }
}
