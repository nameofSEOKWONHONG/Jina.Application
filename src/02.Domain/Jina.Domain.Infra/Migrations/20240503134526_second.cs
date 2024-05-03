using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jina.Domain.Service.Infra.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroups_MenuRoles_MenuRoleTenantId_MenuRoleRoleId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_MenuGroups_MenuGroupTenantId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Menus_ParentMenuTenantId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherForecast",
                schema: "example",
                table: "WeatherForecast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menus",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_MenuGroupTenantId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_ParentMenuTenantId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuRoles",
                schema: "application",
                table: "MenuRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuGroups",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropIndex(
                name: "IX_MenuGroups_MenuRoleTenantId_MenuRoleRoleId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Codes",
                schema: "common",
                table: "Codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeGroups",
                schema: "common",
                table: "CodeGroups");

            migrationBuilder.DropColumn(
                name: "MenuGroupTenantId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ParentMenuTenantId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "MenuRoleRoleId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropColumn(
                name: "MenuRoleTenantId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.AlterColumn<string>(
                name: "TimeZone",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true,
                comment: "시간대",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldNullable: true,
                oldComment: "시스템 시간");

            migrationBuilder.AlterColumn<string>(
                name: "RedirectUrl",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "리다이렉트 url",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "이동 주소 URL");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                comment: "테넌트명",
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true,
                oldComment: "테넌트 명");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                comment: "테넌트ID",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldComment: "테넌트 ID");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "application",
                table: "Menus",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "application",
                table: "Menus",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                schema: "application",
                table: "Menus",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MenuGroupId",
                schema: "application",
                table: "Menus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MenuRoleId",
                schema: "application",
                table: "MenuGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "common",
                table: "Codes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                comment: "테넌트 ID",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldComment: "테넌트 ID");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "common",
                table: "CodeGroups",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                comment: "테넌트 ID",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldComment: "테넌트 ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherForecast",
                schema: "example",
                table: "WeatherForecast",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menus",
                schema: "application",
                table: "Menus",
                columns: new[] { "TenantId", "MenuId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuRoles",
                schema: "application",
                table: "MenuRoles",
                columns: new[] { "TenantId", "MenuRoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuGroups",
                schema: "application",
                table: "MenuGroups",
                columns: new[] { "TenantId", "MenuGroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Codes",
                schema: "common",
                table: "Codes",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeGroups",
                schema: "common",
                table: "CodeGroups",
                columns: new[] { "Code", "GroupCode", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecast_TenantId",
                schema: "example",
                table: "WeatherForecast",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_TenantId",
                schema: "application",
                table: "Menus",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_TenantId_MenuGroupId",
                schema: "application",
                table: "Menus",
                columns: new[] { "TenantId", "MenuGroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_TenantId_ParentMenuId",
                schema: "application",
                table: "Menus",
                columns: new[] { "TenantId", "ParentMenuId" });

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoles_TenantId",
                schema: "application",
                table: "MenuRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuGroups_TenantId",
                schema: "application",
                table: "MenuGroups",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuGroups_TenantId_MenuRoleId",
                schema: "application",
                table: "MenuGroups",
                columns: new[] { "TenantId", "MenuRoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Codes_TenantId",
                schema: "common",
                table: "Codes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGroups_TenantId",
                schema: "common",
                table: "CodeGroups",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroups_MenuRoles_TenantId_MenuRoleId",
                schema: "application",
                table: "MenuGroups",
                columns: new[] { "TenantId", "MenuRoleId" },
                principalSchema: "application",
                principalTable: "MenuRoles",
                principalColumns: new[] { "TenantId", "MenuRoleId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_MenuGroups_TenantId_MenuGroupId",
                schema: "application",
                table: "Menus",
                columns: new[] { "TenantId", "MenuGroupId" },
                principalSchema: "application",
                principalTable: "MenuGroups",
                principalColumns: new[] { "TenantId", "MenuGroupId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Menus_TenantId_ParentMenuId",
                schema: "application",
                table: "Menus",
                columns: new[] { "TenantId", "ParentMenuId" },
                principalSchema: "application",
                principalTable: "Menus",
                principalColumns: new[] { "TenantId", "MenuId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroups_MenuRoles_TenantId_MenuRoleId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_MenuGroups_TenantId_MenuGroupId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Menus_TenantId_ParentMenuId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherForecast",
                schema: "example",
                table: "WeatherForecast");

            migrationBuilder.DropIndex(
                name: "IX_WeatherForecast_TenantId",
                schema: "example",
                table: "WeatherForecast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menus",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_TenantId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_TenantId_MenuGroupId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_TenantId_ParentMenuId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuRoles",
                schema: "application",
                table: "MenuRoles");

            migrationBuilder.DropIndex(
                name: "IX_MenuRoles_TenantId",
                schema: "application",
                table: "MenuRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuGroups",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropIndex(
                name: "IX_MenuGroups_TenantId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropIndex(
                name: "IX_MenuGroups_TenantId_MenuRoleId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Codes",
                schema: "common",
                table: "Codes");

            migrationBuilder.DropIndex(
                name: "IX_Codes_TenantId",
                schema: "common",
                table: "Codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeGroups",
                schema: "common",
                table: "CodeGroups");

            migrationBuilder.DropIndex(
                name: "IX_CodeGroups_TenantId",
                schema: "common",
                table: "CodeGroups");

            migrationBuilder.DropColumn(
                name: "MenuGroupId",
                schema: "application",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "MenuRoleId",
                schema: "application",
                table: "MenuGroups");

            migrationBuilder.AlterColumn<string>(
                name: "TimeZone",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true,
                comment: "시스템 시간",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldNullable: true,
                oldComment: "시간대");

            migrationBuilder.AlterColumn<string>(
                name: "RedirectUrl",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "이동 주소 URL",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "리다이렉트 url");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                comment: "테넌트 명",
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true,
                oldComment: "테넌트명");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Tenants",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                comment: "테넌트 ID",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldComment: "테넌트ID");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "application",
                table: "Menus",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "application",
                table: "Menus",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                schema: "application",
                table: "Menus",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AddColumn<string>(
                name: "MenuGroupTenantId",
                schema: "application",
                table: "Menus",
                type: "nvarchar(5)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentMenuTenantId",
                schema: "application",
                table: "Menus",
                type: "nvarchar(5)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuRoleRoleId",
                schema: "application",
                table: "MenuGroups",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuRoleTenantId",
                schema: "application",
                table: "MenuGroups",
                type: "nvarchar(5)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "common",
                table: "Codes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                comment: "테넌트 ID",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true,
                oldComment: "테넌트 ID");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                schema: "common",
                table: "CodeGroups",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                comment: "테넌트 ID",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true,
                oldComment: "테넌트 ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherForecast",
                schema: "example",
                table: "WeatherForecast",
                column: "TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menus",
                schema: "application",
                table: "Menus",
                column: "TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuRoles",
                schema: "application",
                table: "MenuRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuGroups",
                schema: "application",
                table: "MenuGroups",
                column: "TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Codes",
                schema: "common",
                table: "Codes",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeGroups",
                schema: "common",
                table: "CodeGroups",
                columns: new[] { "TenantId", "Code", "GroupCode", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenuGroupTenantId",
                schema: "application",
                table: "Menus",
                column: "MenuGroupTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentMenuTenantId",
                schema: "application",
                table: "Menus",
                column: "ParentMenuTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuGroups_MenuRoleTenantId_MenuRoleRoleId",
                schema: "application",
                table: "MenuGroups",
                columns: new[] { "MenuRoleTenantId", "MenuRoleRoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroups_MenuRoles_MenuRoleTenantId_MenuRoleRoleId",
                schema: "application",
                table: "MenuGroups",
                columns: new[] { "MenuRoleTenantId", "MenuRoleRoleId" },
                principalSchema: "application",
                principalTable: "MenuRoles",
                principalColumns: new[] { "TenantId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_MenuGroups_MenuGroupTenantId",
                schema: "application",
                table: "Menus",
                column: "MenuGroupTenantId",
                principalSchema: "application",
                principalTable: "MenuGroups",
                principalColumn: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Menus_ParentMenuTenantId",
                schema: "application",
                table: "Menus",
                column: "ParentMenuTenantId",
                principalSchema: "application",
                principalTable: "Menus",
                principalColumn: "TenantId");
        }
    }
}
