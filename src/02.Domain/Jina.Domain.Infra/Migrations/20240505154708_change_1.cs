using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jina.Domain.Service.Infra.Migrations
{
    /// <inheritdoc />
    public partial class change_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MultilingualContents_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.CreateTable(
                name: "MultilingualTopicConfigs",
                schema: "language",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CultureType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SortNo = table.Column<int>(type: "int", nullable: false),
                    MultilingualTopicTenantId = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    MultilingualTopicId = table.Column<int>(type: "int", nullable: false),
                    MultilingualTopicPrimaryCultureType = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false, comment: "생성자"),
                    CreatedName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "생성자명"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "생성일"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true, comment: "수정일"),
                    LastModifiedName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "수정자명"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "수정일"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultilingualTopicConfigs", x => new { x.TenantId, x.Id, x.CultureType });
                    table.ForeignKey(
                        name: "FK_MultilingualTopicConfigs_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                        columns: x => new { x.MultilingualTopicTenantId, x.MultilingualTopicId, x.MultilingualTopicPrimaryCultureType },
                        principalSchema: "language",
                        principalTable: "MultilingualTopics",
                        principalColumns: new[] { "TenantId", "Id", "PrimaryCultureType" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultilingualTopicConfigs_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualTopicConfigs",
                columns: new[] { "MultilingualTopicTenantId", "MultilingualTopicId", "MultilingualTopicPrimaryCultureType" });

            migrationBuilder.CreateIndex(
                name: "IX_MultilingualTopicConfigs_TenantId",
                schema: "language",
                table: "MultilingualTopicConfigs",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MultilingualContents_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents",
                columns: new[] { "MultilingualTopicTenantId", "MultilingualTopicId", "MultilingualTopicPrimaryCultureType" },
                principalSchema: "language",
                principalTable: "MultilingualTopics",
                principalColumns: new[] { "TenantId", "Id", "PrimaryCultureType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MultilingualContents_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropTable(
                name: "MultilingualTopicConfigs",
                schema: "language");

            migrationBuilder.AddForeignKey(
                name: "FK_MultilingualContents_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents",
                columns: new[] { "MultilingualTopicTenantId", "MultilingualTopicId", "MultilingualTopicPrimaryCultureType" },
                principalSchema: "language",
                principalTable: "MultilingualTopics",
                principalColumns: new[] { "TenantId", "Id", "PrimaryCultureType" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
