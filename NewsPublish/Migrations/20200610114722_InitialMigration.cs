using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsPublish.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Picture = table.Column<string>(nullable: false),
                    Url = table.Column<string>(maxLength: 300, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Remark = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rights",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Count = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RightRoles",
                columns: table => new
                {
                    RightId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RightRoles", x => new { x.RoleId, x.RightId });
                    table.ForeignKey(
                        name: "FK_RightRoles_Rights_RightId",
                        column: x => x.RightId,
                        principalTable: "Rights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RightRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NickName = table.Column<string>(maxLength: 50, nullable: false),
                    Avatar = table.Column<string>(maxLength: 300, nullable: true),
                    Introduce = table.Column<string>(maxLength: 1000, nullable: true),
                    States = table.Column<bool>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    CoverPic = table.Column<string>(maxLength: 300, nullable: true),
                    Content = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    States = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatorAutheAudits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Remark = table.Column<string>(nullable: false),
                    ReviewRemark = table.Column<string>(nullable: true),
                    ReviewTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsPass = table.Column<bool>(nullable: false),
                    AuditStatus = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorAutheAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatorAutheAudits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Account = table.Column<string>(maxLength: 100, nullable: false),
                    AuthType = table.Column<int>(nullable: false),
                    Credential = table.Column<string>(maxLength: 200, nullable: false),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAuthes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleReviewAudits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReviewRemark = table.Column<string>(nullable: true),
                    ReviewTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsPass = table.Column<bool>(nullable: false),
                    AuditStatus = table.Column<bool>(nullable: false),
                    ArticleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleReviewAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleReviewAudits_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ArticleId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CommentId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ReceivedId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replies_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "CreateTime", "Picture", "Remark", "Url" },
                values: new object[] { new Guid("94163853-ea93-4281-b9bc-8b5924ef8516"), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(9040), "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png", "百度一下，你就知道", "https://www.baidu.com/" });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "CreateTime", "Picture", "Remark", "Url" },
                values: new object[] { new Guid("5e7b9dfd-7cbf-440e-92cf-2e9566fe4946"), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(8290), "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png", "百度一下，你就知道", "https://www.baidu.com/" });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "CreateTime", "Picture", "Remark", "Url" },
                values: new object[] { new Guid("a3c2b4a8-48f8-46a7-85db-90c3576bd6ce"), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(9020), "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png", "百度一下，你就知道", "https://www.baidu.com/" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Remark" },
                values: new object[] { new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "默认", "默认分类！" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Remark" },
                values: new object[] { new Guid("9ad93c00-09f4-4f88-8558-0ae3ce19c4c0"), "财经", "财经新闻" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Remark" },
                values: new object[] { new Guid("8c5f4aad-83e6-41c9-8e7f-96887257cafb"), "娱乐", "娱乐新闻" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Remark" },
                values: new object[] { new Guid("42a2bae9-09c2-47ae-94de-3912debff652"), "科技", "科技新闻" });

            migrationBuilder.InsertData(
                table: "Rights",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("fe4ebf2e-f1e9-48c8-9a52-d297e865c4a1"), "系统管理" });

            migrationBuilder.InsertData(
                table: "Rights",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), "内容查看" });

            migrationBuilder.InsertData(
                table: "Rights",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"), "内容审核" });

            migrationBuilder.InsertData(
                table: "Rights",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"), "内容创作" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "Name" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new DateTime(2020, 6, 10, 19, 47, 22, 564, DateTimeKind.Local).AddTicks(9320), new DateTime(2020, 6, 10, 19, 47, 22, 567, DateTimeKind.Local).AddTicks(5460), "管理员" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "Name" },
                values: new object[] { new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"), new DateTime(2020, 6, 10, 19, 47, 22, 567, DateTimeKind.Local).AddTicks(6330), new DateTime(2020, 6, 10, 19, 47, 22, 567, DateTimeKind.Local).AddTicks(6340), "内容审查" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "Name" },
                values: new object[] { new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), new DateTime(2020, 6, 10, 19, 47, 22, 567, DateTimeKind.Local).AddTicks(6360), new DateTime(2020, 6, 10, 19, 47, 22, 567, DateTimeKind.Local).AddTicks(6360), "自媒体" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "Name" },
                values: new object[] { new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"), new DateTime(2020, 6, 10, 19, 47, 22, 567, DateTimeKind.Local).AddTicks(6370), new DateTime(2020, 6, 10, 19, 47, 22, 567, DateTimeKind.Local).AddTicks(6370), "用户" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[] { new Guid("c456990c-05db-4965-9334-a295b7f0f993"), new DateTime(2020, 6, 10, 19, 47, 22, 570, DateTimeKind.Local).AddTicks(750), "华为" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[] { new Guid("675db0e3-d57d-469b-a07f-09a3ef8a20eb"), new DateTime(2020, 6, 10, 19, 47, 22, 570, DateTimeKind.Local).AddTicks(1150), "国际" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[] { new Guid("78750f26-d3e7-4ebc-b69a-1c88f44e67ba"), new DateTime(2020, 6, 10, 19, 47, 22, 570, DateTimeKind.Local).AddTicks(1160), "科技" });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("fe4ebf2e-f1e9-48c8-9a52-d297e865c4a1"), new Guid("03dd6779-866a-451e-8a31-731df51c90ff") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"), new Guid("2a93d43e-870c-4d34-8e9d-c960c6f6cfe7") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("2f964db3-a146-4b01-ac9a-6af2dd1d2e7b") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"), new Guid("994096a9-ba92-40d8-a703-1c4880320e83") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"), new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"), new Guid("4cdeeb8c-33af-4659-90ca-182fac7eb688") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("d98f7d65-e06e-44e8-a1a6-2ebd46577601") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"), new Guid("695f70bf-99d5-4fff-b76c-c133f8e8f9d8") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("452cee08-beb0-4bff-b684-13fe97d504ad") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("5a54e02a-e5ea-493f-a8ed-5600d06918c4") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Introduce", "NickName", "RoleId", "States" },
                values: new object[] { new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png", "大家好，我是张世纪!", "张世纪", new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Introduce", "NickName", "RoleId", "States" },
                values: new object[] { new Guid("6fb600c1-9011-4fd7-9234-881379716400"), "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png", "大家好，我是王五！", "王五", new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Introduce", "NickName", "RoleId", "States" },
                values: new object[] { new Guid("6fb600c1-9011-4fd7-9234-881379716440"), "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png", "大家好，我是苑紫清！", "苑紫清", new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Introduce", "NickName", "RoleId", "States" },
                values: new object[] { new Guid("bbdee09c-089b-4d30-bece-44df59237100"), "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png", "大家好，我是李四！", "李四", new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Introduce", "NickName", "RoleId", "States" },
                values: new object[] { new Guid("5efc910b-2f45-43df-afae-620d40542853"), "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png", "大家好，我是张三！", "张三", new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"), true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Introduce", "NickName", "RoleId", "States" },
                values: new object[] { new Guid("6091967b-0952-425a-9eda-840b24da5534"), "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png", "卑微的普通用户", "卑微的用户", new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"), true });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("0a0e5629-cf5d-438f-abd2-bf6918d232a9"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "小米全面屏电视Pro 32英寸新品发布，5月25日全渠道开售!", "https://img.ithome.com/newsuploadfiles/2020/5/20200525_120326_295.jpg", new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4200), new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4200), true, "小米全面屏电视 Pro 32 英寸新品发布，售价 899 元”", new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("ca88d765-8c17-4378-bd42-6146aec1a9ca"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "根据外媒macrumors消息，DigiTimes最近的一份报告显示苹果希望在未来几年内将环境光传感器集成到新型的AirPods中。", "https://img.ithome.com/newsuploadfiles/2020/5/20200525_202833_144.png", new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(3070), new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(3430), true, "DigiTimes：苹果未来 AirPods 将搭载“环境光传感器”", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), new Guid("42a2bae9-09c2-47ae-94de-3912debff652"), "一直以来，华为公司就像美国政府的“眼中钉”，不断遭美制裁。近两年，美国政府对华为的制裁愈演愈烈，甚至让其对外发声时只能苛求“活下来”。", "https://img.ithome.com/newsuploadfiles/2020/2/20200214_090339_584.jpg", new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4140), new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4150), true, "华为“去美国化”的成功几率有多大？”", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("84e6a64f-53c8-4b30-8735-eabc6d48a549"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "两会期间，董明珠在接受采访时表示：“六稳”“六保”首先就是稳企业、保居民就业。企业的生命力体现在遭遇不可抗力时的风险防控能力，这个时候还能呵护我的员工，让他们在这很安全，我们就要做这种风险防控，今年一下真的给兑现了。", "https://img.ithome.com/newsuploadfiles/2020/5/20200525_191650_79.png", new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4170), new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4180), false, "董明珠称坚决不裁员：员工少1000块钱能活，没工作很难活下去", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("11d67370-955b-4976-9285-834721ca57a5"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "新华社银川6月10日电 中共中央总书记、国家主席、中央军委主席习近平近日在宁夏考察时强调，要全面落实党中央决策部署，坚持稳中求进工作总基调，坚持新发展理念，落实全国“两会”工作部署，坚决打好三大攻坚战，扎实做好“六稳”工作，全面落实“六保”任务，努力克服新冠肺炎疫情带来的不利影响，优先稳就业保民生，决胜全面建成小康社会，决战脱贫攻坚，继续建设经济繁荣、民族团结、环境优美、人民富裕的美丽新宁夏。", "https://img.ithome.com/newsuploadfiles/2020/5/20200525102542_7599.jpg", new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4180), new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4180), false, "习近平在宁夏考察时强调 决胜全面建成小康社会决战脱贫攻坚 继续建设经济繁荣民族团结环境优美人民富裕的美丽新宁夏”", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("30c1f151-b97f-46b0-8d1f-e327051b67c8"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "今天，天猫618正式启动。据天猫官方数据显示，第1小时预售成交额同比增长515 %！", "https://img.ithome.com/newsuploadfiles/2020/5/20200525102542_7599.jpg", new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4190), new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(4190), true, "史上最大规模！天猫618今日正式启动”", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "CreatorAutheAudits",
                columns: new[] { "Id", "AuditStatus", "CreateTime", "IsPass", "Remark", "ReviewRemark", "ReviewTime", "UserId" },
                values: new object[] { new Guid("afda3d6d-508a-4552-bf69-026dc22d791d"), false, new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(5410), false, "身份信息如下：", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6091967b-0952-425a-9eda-840b24da5534") });

            migrationBuilder.InsertData(
                table: "CreatorAutheAudits",
                columns: new[] { "Id", "AuditStatus", "CreateTime", "IsPass", "Remark", "ReviewRemark", "ReviewTime", "UserId" },
                values: new object[] { new Guid("01883fc0-bc86-4b32-9de9-b2f7c4cb582a"), false, new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(6910), false, "2身份信息如下：", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6091967b-0952-425a-9eda-840b24da5534") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("9efa7f1c-a3da-4764-bad9-a88f01f389d6"), "17692463717", 1, "1234567890", new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(3320), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(2960), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("a11fe954-7f4d-4229-be77-0c276886ff9c"), "17692463717@qq.com", 2, "1234567890", new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4090), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4070), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("54e7051d-8daa-4b1e-bcf3-c7fe35ec0b45"), "13402330890", 1, "1234567890", new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4130), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4130), new Guid("6fb600c1-9011-4fd7-9234-881379716400") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("a55acec9-593d-4b5a-be27-1d5723411705"), "13300339033", 1, "1234567890", new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4110), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4110), new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("4957355f-d687-40a4-b2b7-5402df1c4cef"), "14302330899", 1, "1234567890", new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4120), new DateTime(2020, 6, 10, 19, 47, 22, 568, DateTimeKind.Local).AddTicks(4120), new Guid("5efc910b-2f45-43df-afae-620d40542853") });

            migrationBuilder.InsertData(
                table: "ArticleReviewAudits",
                columns: new[] { "Id", "ArticleId", "AuditStatus", "CreateTime", "IsPass", "ReviewRemark", "ReviewTime" },
                values: new object[] { new Guid("b48c02d8-4a57-4b7c-bb78-4d86c4f4f014"), new Guid("11d67370-955b-4976-9285-834721ca57a5"), true, new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(5690), true, "文章写的不错，通过了！", new DateTime(2020, 6, 10, 19, 47, 22, 569, DateTimeKind.Local).AddTicks(6040) });

            migrationBuilder.InsertData(
                table: "ArticleTags",
                columns: new[] { "ArticleId", "TagId" },
                values: new object[] { new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), new Guid("c456990c-05db-4965-9334-a295b7f0f993") });

            migrationBuilder.InsertData(
                table: "ArticleTags",
                columns: new[] { "ArticleId", "TagId" },
                values: new object[] { new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), new Guid("675db0e3-d57d-469b-a07f-09a3ef8a20eb") });

            migrationBuilder.InsertData(
                table: "ArticleTags",
                columns: new[] { "ArticleId", "TagId" },
                values: new object[] { new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), new Guid("78750f26-d3e7-4ebc-b69a-1c88f44e67ba") });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "Content", "CreateTime", "UserId" },
                values: new object[] { new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"), new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), "加油华为！", new DateTime(2020, 6, 10, 19, 47, 22, 570, DateTimeKind.Local).AddTicks(2330), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "Content", "CreateTime", "UserId" },
                values: new object[] { new Guid("ade40208-cf60-47e8-894a-379777a380a7"), new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), "干翻老美！", new DateTime(2020, 6, 10, 19, 47, 22, 570, DateTimeKind.Local).AddTicks(3420), new Guid("5efc910b-2f45-43df-afae-620d40542853") });

            migrationBuilder.InsertData(
                table: "Replies",
                columns: new[] { "Id", "CommentId", "Content", "CreateTime", "ReceivedId", "UserId" },
                values: new object[] { new Guid("422f5c71-19c8-4729-bbf3-c9a267ca8acc"), new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"), "说得好!", new DateTime(2020, 6, 10, 19, 47, 22, 570, DateTimeKind.Local).AddTicks(5650), new Guid("5efc910b-2f45-43df-afae-620d40542853"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "Replies",
                columns: new[] { "Id", "CommentId", "Content", "CreateTime", "ReceivedId", "UserId" },
                values: new object[] { new Guid("077c8ace-26d5-4699-9cbc-2e0ef7d5021c"), new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"), "说得好!", new DateTime(2020, 6, 10, 19, 47, 22, 570, DateTimeKind.Local).AddTicks(6070), new Guid("5efc910b-2f45-43df-afae-620d40542853"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleReviewAudits_ArticleId",
                table: "ArticleReviewAudits",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                table: "ArticleTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorAutheAudits_UserId",
                table: "CreatorAutheAudits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_CommentId",
                table: "Replies",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_UserId",
                table: "Replies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RightRoles_RightId",
                table: "RightRoles",
                column: "RightId");

            migrationBuilder.CreateIndex(
                name: "IX_Rights_Name",
                table: "Rights",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthes_Account",
                table: "UserAuthes",
                column: "Account",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthes_UserId",
                table: "UserAuthes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleReviewAudits");

            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "CreatorAutheAudits");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropTable(
                name: "RightRoles");

            migrationBuilder.DropTable(
                name: "Stars");

            migrationBuilder.DropTable(
                name: "UserAuthes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Rights");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
