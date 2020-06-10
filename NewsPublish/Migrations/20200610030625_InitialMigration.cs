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
                    ReturnRemark = table.Column<string>(nullable: true),
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
                values: new object[] { new Guid("3a43c843-4992-423e-9f10-67c2107b56e5"), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(6070), "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png", "百度一下，你就知道", "https://www.baidu.com/" });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "CreateTime", "Picture", "Remark", "Url" },
                values: new object[] { new Guid("73c28988-1a82-457c-9931-50e36ecfd7b3"), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(5350), "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png", "百度一下，你就知道", "https://www.baidu.com/" });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "CreateTime", "Picture", "Remark", "Url" },
                values: new object[] { new Guid("59360764-adf3-4023-9feb-aabbe89a855a"), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(6040), "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png", "百度一下，你就知道", "https://www.baidu.com/" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Remark" },
                values: new object[] { new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "默认", "默认分类！" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Remark" },
                values: new object[] { new Guid("33dd3718-a9fc-4778-b23b-201d897e0b4b"), "财经", "财经新闻" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Remark" },
                values: new object[] { new Guid("b97a33f5-8819-4a77-b42e-3f2a3b0c707e"), "娱乐", "娱乐新闻" });

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
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new DateTime(2020, 6, 10, 11, 6, 24, 918, DateTimeKind.Local).AddTicks(9460), new DateTime(2020, 6, 10, 11, 6, 24, 921, DateTimeKind.Local).AddTicks(2910), "管理员" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "Name" },
                values: new object[] { new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"), new DateTime(2020, 6, 10, 11, 6, 24, 921, DateTimeKind.Local).AddTicks(3730), new DateTime(2020, 6, 10, 11, 6, 24, 921, DateTimeKind.Local).AddTicks(3740), "内容审查" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "Name" },
                values: new object[] { new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), new DateTime(2020, 6, 10, 11, 6, 24, 921, DateTimeKind.Local).AddTicks(3760), new DateTime(2020, 6, 10, 11, 6, 24, 921, DateTimeKind.Local).AddTicks(3760), "自媒体" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "Name" },
                values: new object[] { new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"), new DateTime(2020, 6, 10, 11, 6, 24, 921, DateTimeKind.Local).AddTicks(3770), new DateTime(2020, 6, 10, 11, 6, 24, 921, DateTimeKind.Local).AddTicks(3770), "用户" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[] { new Guid("c456990c-05db-4965-9334-a295b7f0f993"), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(3350), "华为" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[] { new Guid("675db0e3-d57d-469b-a07f-09a3ef8a20eb"), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(3730), "国际" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[] { new Guid("78750f26-d3e7-4ebc-b69a-1c88f44e67ba"), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(3740), "科技" });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("fe4ebf2e-f1e9-48c8-9a52-d297e865c4a1"), new Guid("87794936-d75b-43cf-850f-7912d6d5a512") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"), new Guid("28d8f092-07a7-4d8c-9ff9-52185c038e19") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("45d3b314-7ab9-44ef-bdff-f9994bd8488e") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"), new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"), new Guid("e501ffcb-a195-44c7-bc5a-20b1bd4a93c0") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"), new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"), new Guid("7eafadd7-fdc7-44bc-8991-dcaa1db43421") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("51aec548-6dee-4b8e-befb-8f056ddf515e") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"), new Guid("2bdb61bc-8e41-47d3-825e-d143de225bd0") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("12180273-7dd1-4164-96d1-418a3e9ddb6a") });

            migrationBuilder.InsertData(
                table: "RightRoles",
                columns: new[] { "RoleId", "RightId", "Id" },
                values: new object[] { new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"), new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"), new Guid("53a94b71-1cf7-4dee-a29f-4d857f987889") });

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
                values: new object[] { new Guid("0a0e5629-cf5d-438f-abd2-bf6918d232a9"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "小米全面屏电视Pro 32英寸新品发布，5月25日全渠道开售!", "https://img.ithome.com/newsuploadfiles/2020/5/20200525_120326_295.jpg", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(1010), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(1010), true, "小米全面屏电视 Pro 32 英寸新品发布，售价 899 元”", new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("ca88d765-8c17-4378-bd42-6146aec1a9ca"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "根据外媒macrumors消息，DigiTimes最近的一份报告显示苹果希望在未来几年内将环境光传感器集成到新型的AirPods中。", "https://img.ithome.com/newsuploadfiles/2020/5/20200525_202833_144.png", new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(9930), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(270), true, "DigiTimes：苹果未来 AirPods 将搭载“环境光传感器”", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), new Guid("42a2bae9-09c2-47ae-94de-3912debff652"), "一直以来，华为公司就像美国政府的“眼中钉”，不断遭美制裁。近两年，美国政府对华为的制裁愈演愈烈，甚至让其对外发声时只能苛求“活下来”。", "https://img.ithome.com/newsuploadfiles/2020/2/20200214_090339_584.jpg", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(960), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(970), true, "华为“去美国化”的成功几率有多大？”", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("84e6a64f-53c8-4b30-8735-eabc6d48a549"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "两会期间，董明珠在接受采访时表示：“六稳”“六保”首先就是稳企业、保居民就业。企业的生命力体现在遭遇不可抗力时的风险防控能力，这个时候还能呵护我的员工，让他们在这很安全，我们就要做这种风险防控，今年一下真的给兑现了。", "https://img.ithome.com/newsuploadfiles/2020/5/20200525_191650_79.png", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(990), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(990), false, "董明珠称坚决不裁员：员工少1000块钱能活，没工作很难活下去", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CoverPic", "CreateTime", "ModifyTime", "States", "Title", "UserId" },
                values: new object[] { new Guid("11d67370-955b-4976-9285-834721ca57a5"), new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"), "今天，天猫618正式启动。据天猫官方数据显示，第1小时预售成交额同比增长515 %！", "https://img.ithome.com/newsuploadfiles/2020/5/20200525102542_7599.jpg", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(1000), new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(1000), true, "史上最大规模！天猫618今日正式启动”", new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "CreatorAutheAudits",
                columns: new[] { "Id", "AuditStatus", "CreateTime", "IsPass", "Remark", "ReturnRemark", "UserId" },
                values: new object[] { new Guid("afda3d6d-508a-4552-bf69-026dc22d791d"), false, new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(2550), false, "身份信息如下：", null, new Guid("6091967b-0952-425a-9eda-840b24da5534") });

            migrationBuilder.InsertData(
                table: "CreatorAutheAudits",
                columns: new[] { "Id", "AuditStatus", "CreateTime", "IsPass", "Remark", "ReturnRemark", "UserId" },
                values: new object[] { new Guid("01883fc0-bc86-4b32-9de9-b2f7c4cb582a"), false, new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(4010), false, "2身份信息如下：", null, new Guid("6091967b-0952-425a-9eda-840b24da5534") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("be4207e1-ad15-42b0-87b2-a8c8c361e159"), "17692463717", 1, "1234567890", new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(470), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(30), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("51b9b7ef-9add-429d-8cfb-a8e5cd1ad521"), "17692463717@qq.com", 2, "1234567890", new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1240), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1230), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("e0d14d6b-998e-4ca6-aca2-aa00c40c9b37"), "13402330890", 1, "1234567890", new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1280), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1280), new Guid("6fb600c1-9011-4fd7-9234-881379716400") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("3f93ff8d-7f87-40b3-b37f-e21e60041921"), "13300339033", 1, "1234567890", new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1270), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1270), new Guid("6fb600c1-9011-4fd7-9234-881379716440") });

            migrationBuilder.InsertData(
                table: "UserAuthes",
                columns: new[] { "Id", "Account", "AuthType", "Credential", "ModifyTime", "RegisterTime", "UserId" },
                values: new object[] { new Guid("f119ec50-94e8-4569-a384-8fbf92d5d5f0"), "14302330899", 1, "1234567890", new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1280), new DateTime(2020, 6, 10, 11, 6, 24, 922, DateTimeKind.Local).AddTicks(1270), new Guid("5efc910b-2f45-43df-afae-620d40542853") });

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
                values: new object[] { new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"), new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), "加油华为！", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(4950), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "Content", "CreateTime", "UserId" },
                values: new object[] { new Guid("0253e336-5ba5-4067-9b0c-d0337ede4a5f"), new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"), "干翻老美！", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(6000), new Guid("5efc910b-2f45-43df-afae-620d40542853") });

            migrationBuilder.InsertData(
                table: "Replies",
                columns: new[] { "Id", "CommentId", "Content", "CreateTime", "ReceivedId", "UserId" },
                values: new object[] { new Guid("1ef4fe02-f9ae-4dd5-9ca5-bba4324f27f6"), new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"), "说得好!", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(8270), new Guid("5efc910b-2f45-43df-afae-620d40542853"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

            migrationBuilder.InsertData(
                table: "Replies",
                columns: new[] { "Id", "CommentId", "Content", "CreateTime", "ReceivedId", "UserId" },
                values: new object[] { new Guid("e5f237a1-6cff-4ffb-9921-4bc27b3aba1e"), new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"), "说得好!", new DateTime(2020, 6, 10, 11, 6, 24, 923, DateTimeKind.Local).AddTicks(8670), new Guid("5efc910b-2f45-43df-afae-620d40542853"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c") });

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
