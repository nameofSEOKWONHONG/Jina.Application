using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jina.Domain.Service.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "common");

            migrationBuilder.EnsureSchema(
                name: "application");

            migrationBuilder.EnsureSchema(
                name: "language");

            migrationBuilder.EnsureSchema(
                name: "example");

            migrationBuilder.CreateTable(
                name: "CodeGroups",
                schema: "common",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    GroupCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_CodeGroups", x => new { x.TenantId, x.GroupCode, x.Key });
                });

            migrationBuilder.CreateTable(
                name: "MenuRoles",
                schema: "application",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    MenuRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
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
                    table.PrimaryKey("PK_MenuRoles", x => new { x.TenantId, x.MenuRoleId });
                });

            migrationBuilder.CreateTable(
                name: "MultilingualTopics",
                schema: "language",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryCultureType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "메인 입력 언어 타입"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "번역 주제"),
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
                    table.PrimaryKey("PK_MultilingualTopics", x => new { x.TenantId, x.Id, x.PrimaryCultureType });
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "dbo",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트ID"),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true, comment: "테넌트명"),
                    RedirectUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "리다이렉트 url"),
                    TimeZone = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true, comment: "시간대")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                schema: "example",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "도시명"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "날짜"),
                    TemperatureC = table.Column<int>(type: "int", nullable: true, comment: "섭씨온도"),
                    Summary = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "요약"),
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
                    table.PrimaryKey("PK_WeatherForecasts", x => new { x.TenantId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "MenuGroups",
                schema: "application",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    MenuGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    MenuRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MenuGroups", x => new { x.TenantId, x.MenuGroupId });
                    table.ForeignKey(
                        name: "FK_MenuGroups_MenuRoles_TenantId_MenuRoleId",
                        columns: x => new { x.TenantId, x.MenuRoleId },
                        principalSchema: "application",
                        principalTable: "MenuRoles",
                        principalColumns: new[] { "TenantId", "MenuRoleId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MultilingualContents",
                schema: "language",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CultureType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Input = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_MultilingualContents", x => new { x.TenantId, x.Id, x.CultureType });
                    table.ForeignKey(
                        name: "FK_MultilingualContents_MultilingualTopics_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                        columns: x => new { x.MultilingualTopicTenantId, x.MultilingualTopicId, x.MultilingualTopicPrimaryCultureType },
                        principalSchema: "language",
                        principalTable: "MultilingualTopics",
                        principalColumns: new[] { "TenantId", "Id", "PrimaryCultureType" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false, comment: "생성자"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "생성일"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true, comment: "수정자"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "수정일"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProfilePictureDataUrl = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, comment: "활성화 여부"),
                    CreatedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false, comment: "생성자"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "생성일"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true, comment: "수정자"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "수정일"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "application",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "테넌트"),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    SortNo = table.Column<int>(type: "int", nullable: false),
                    MenuGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_Menus", x => new { x.TenantId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_Menus_MenuGroups_TenantId_MenuGroupId",
                        columns: x => new { x.TenantId, x.MenuGroupId },
                        principalSchema: "application",
                        principalTable: "MenuGroups",
                        principalColumns: new[] { "TenantId", "MenuGroupId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_TenantId_ParentMenuId",
                        columns: x => new { x.TenantId, x.ParentMenuId },
                        principalSchema: "application",
                        principalTable: "Menus",
                        principalColumns: new[] { "TenantId", "MenuId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "설명"),
                    Group = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true, comment: "그룹명"),
                    CreatedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false, comment: "생성자"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "생성일"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true, comment: "수정자"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "수정일"),
                    RoleId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, comment: "활성화 여부"),
                    CreatedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false, comment: "생성자"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "생성일"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true, comment: "수정자"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "수정일")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId1",
                table: "AspNetRoleClaims",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_TenantId",
                table: "AspNetRoleClaims",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_TenantId",
                table: "AspNetUserRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_TenantId",
                schema: "dbo",
                table: "AuditTrails",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGroups_TenantId",
                schema: "common",
                table: "CodeGroups",
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
                name: "IX_MenuRoles_TenantId",
                schema: "application",
                table: "MenuRoles",
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
                name: "IX_MultilingualContents_MultilingualTopicTenantId_MultilingualTopicId_MultilingualTopicPrimaryCultureType",
                schema: "language",
                table: "MultilingualContents",
                columns: new[] { "MultilingualTopicTenantId", "MultilingualTopicId", "MultilingualTopicPrimaryCultureType" });

            migrationBuilder.CreateIndex(
                name: "IX_MultilingualContents_TenantId",
                schema: "language",
                table: "MultilingualContents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MultilingualTopics_TenantId",
                schema: "language",
                table: "MultilingualTopics",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecasts_TenantId",
                schema: "example",
                table: "WeatherForecasts",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditTrails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CodeGroups",
                schema: "common");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "application");

            migrationBuilder.DropTable(
                name: "MultilingualContents",
                schema: "language");

            migrationBuilder.DropTable(
                name: "WeatherForecasts",
                schema: "example");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MenuGroups",
                schema: "application");

            migrationBuilder.DropTable(
                name: "MultilingualTopics",
                schema: "language");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MenuRoles",
                schema: "application");
        }
    }
}
