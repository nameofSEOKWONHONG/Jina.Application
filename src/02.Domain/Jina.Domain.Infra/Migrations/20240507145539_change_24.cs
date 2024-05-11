using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jina.Domain.Service.Infra.Migrations
{
    /// <inheritdoc />
    public partial class change_24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MultilingualContents",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropColumn(
                name: "Comment",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "language",
                table: "MultilingualContents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MultilingualContents",
                schema: "language",
                table: "MultilingualContents",
                columns: new[] { "TenantId", "Guid" });

            migrationBuilder.CreateTable(
                name: "MultilingualContentDetails",
                schema: "language",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CultureType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Input = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    MasterTenantId = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    MasterGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_MultilingualContentDetails", x => new { x.TenantId, x.Guid });
                    table.ForeignKey(
                        name: "FK_MultilingualContentDetails_MultilingualContents_MasterTenantId_MasterGuid",
                        columns: x => new { x.MasterTenantId, x.MasterGuid },
                        principalSchema: "language",
                        principalTable: "MultilingualContents",
                        principalColumns: new[] { "TenantId", "Guid" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultilingualContentDetails_MasterTenantId_MasterGuid",
                schema: "language",
                table: "MultilingualContentDetails",
                columns: new[] { "MasterTenantId", "MasterGuid" });

            migrationBuilder.CreateIndex(
                name: "IX_MultilingualContentDetails_TenantId",
                schema: "language",
                table: "MultilingualContentDetails",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultilingualContentDetails",
                schema: "language");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MultilingualContents",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "language",
                table: "MultilingualContents");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "language",
                table: "MultilingualContents",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                schema: "language",
                table: "MultilingualContents",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MultilingualContents",
                schema: "language",
                table: "MultilingualContents",
                columns: new[] { "TenantId", "Id", "CultureType" });
        }
    }
}
