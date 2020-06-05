﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsPublish.Database.Data;

namespace NewsPublish.Migrations
{
    [DbContext(typeof(RoutineDbContext))]
    partial class RoutineDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CoverPic")
                        .HasColumnType("TEXT")
                        .HasMaxLength(300);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("States")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ca88d765-8c17-4378-bd42-6146aec1a9ca"),
                            CategoryId = new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                            Content = "根据外媒macrumors消息，DigiTimes最近的一份报告显示苹果希望在未来几年内将环境光传感器集成到新型的AirPods中。",
                            CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525_202833_144.png",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(9800),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(150),
                            States = true,
                            Title = "DigiTimes：苹果未来 AirPods 将搭载“环境光传感器”",
                            UserId = new Guid("6fb600c1-9011-4fd7-9234-881379716440")
                        },
                        new
                        {
                            Id = new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                            CategoryId = new Guid("42a2bae9-09c2-47ae-94de-3912debff652"),
                            Content = "一直以来，华为公司就像美国政府的“眼中钉”，不断遭美制裁。近两年，美国政府对华为的制裁愈演愈烈，甚至让其对外发声时只能苛求“活下来”。",
                            CoverPic = "https://img.ithome.com/newsuploadfiles/2020/2/20200214_090339_584.jpg",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(860),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(880),
                            States = true,
                            Title = "华为“去美国化”的成功几率有多大？”",
                            UserId = new Guid("6fb600c1-9011-4fd7-9234-881379716440")
                        },
                        new
                        {
                            Id = new Guid("84e6a64f-53c8-4b30-8735-eabc6d48a549"),
                            CategoryId = new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                            Content = "两会期间，董明珠在接受采访时表示：“六稳”“六保”首先就是稳企业、保居民就业。企业的生命力体现在遭遇不可抗力时的风险防控能力，这个时候还能呵护我的员工，让他们在这很安全，我们就要做这种风险防控，今年一下真的给兑现了。",
                            CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525_191650_79.png",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(900),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(900),
                            States = false,
                            Title = "董明珠称坚决不裁员：员工少1000块钱能活，没工作很难活下去",
                            UserId = new Guid("6fb600c1-9011-4fd7-9234-881379716440")
                        },
                        new
                        {
                            Id = new Guid("11d67370-955b-4976-9285-834721ca57a5"),
                            CategoryId = new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                            Content = "今天，天猫618正式启动。据天猫官方数据显示，第1小时预售成交额同比增长515 %！",
                            CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525102542_7599.jpg",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(910),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(910),
                            States = true,
                            Title = "史上最大规模！天猫618今日正式启动”",
                            UserId = new Guid("6fb600c1-9011-4fd7-9234-881379716440")
                        },
                        new
                        {
                            Id = new Guid("0a0e5629-cf5d-438f-abd2-bf6918d232a9"),
                            CategoryId = new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                            Content = "小米全面屏电视Pro 32英寸新品发布，5月25日全渠道开售!",
                            CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525_120326_295.jpg",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(920),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(920),
                            States = true,
                            Title = "小米全面屏电视 Pro 32 英寸新品发布，售价 899 元”",
                            UserId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c")
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.ArticleTag", b =>
                {
                    b.Property<Guid>("ArticleId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TagId")
                        .HasColumnType("TEXT");

                    b.HasKey("ArticleId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ArticleTags");

                    b.HasData(
                        new
                        {
                            ArticleId = new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                            TagId = new Guid("c456990c-05db-4965-9334-a295b7f0f993")
                        },
                        new
                        {
                            ArticleId = new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                            TagId = new Guid("675db0e3-d57d-469b-a07f-09a3ef8a20eb")
                        },
                        new
                        {
                            ArticleId = new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                            TagId = new Guid("78750f26-d3e7-4ebc-b69a-1c88f44e67ba")
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                            Name = "默认",
                            Remark = "默认分类！"
                        },
                        new
                        {
                            Id = new Guid("3a3bf56d-fa96-4135-9ce3-f6b8d51f7165"),
                            Name = "娱乐",
                            Remark = "娱乐新闻"
                        },
                        new
                        {
                            Id = new Guid("557bd08b-a182-49da-b044-577a817e7f70"),
                            Name = "财经",
                            Remark = "财经新闻"
                        },
                        new
                        {
                            Id = new Guid("42a2bae9-09c2-47ae-94de-3912debff652"),
                            Name = "科技",
                            Remark = "科技新闻"
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"),
                            ArticleId = new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                            Content = "加油华为！",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(5030),
                            UserId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c")
                        },
                        new
                        {
                            Id = new Guid("9477d1d5-d6ff-45e5-859a-0a7985318fe3"),
                            ArticleId = new Guid("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                            Content = "干翻老美！",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(6570),
                            UserId = new Guid("5efc910b-2f45-43df-afae-620d40542853")
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Reply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ReceivedId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("Replies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c57558d0-68b4-46d1-82cb-97ffd806641b"),
                            CommentId = new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"),
                            Content = "说得好!",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(8830),
                            ReceivedId = new Guid("5efc910b-2f45-43df-afae-620d40542853"),
                            UserId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c")
                        },
                        new
                        {
                            Id = new Guid("29d89c3a-8e53-4976-bfe2-5e3db48a5a82"),
                            CommentId = new Guid("e75c9ede-8394-43f6-821d-e06f36024f9e"),
                            Content = "说得好!",
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(9250),
                            ReceivedId = new Guid("5efc910b-2f45-43df-afae-620d40542853"),
                            UserId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c")
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Star", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("StartId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Stars");
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c456990c-05db-4965-9334-a295b7f0f993"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(3440),
                            Name = "华为"
                        },
                        new
                        {
                            Id = new Guid("675db0e3-d57d-469b-a07f-09a3ef8a20eb"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(3820),
                            Name = "国际"
                        },
                        new
                        {
                            Id = new Guid("78750f26-d3e7-4ebc-b69a-1c88f44e67ba"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 582, DateTimeKind.Local).AddTicks(3850),
                            Name = "科技"
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.RoleEntities.Right", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Rights");

                    b.HasData(
                        new
                        {
                            Id = new Guid("fe4ebf2e-f1e9-48c8-9a52-d297e865c4a1"),
                            Name = "系统管理"
                        },
                        new
                        {
                            Id = new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"),
                            Name = "内容创作"
                        },
                        new
                        {
                            Id = new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"),
                            Name = "内容查看"
                        },
                        new
                        {
                            Id = new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"),
                            Name = "内容审核"
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.RoleEntities.RightRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RightId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.HasKey("RoleId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("RightRoles");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                            RightId = new Guid("fe4ebf2e-f1e9-48c8-9a52-d297e865c4a1"),
                            Id = new Guid("2e2fc05d-b30b-4433-818e-f0fec2b7908e")
                        },
                        new
                        {
                            RoleId = new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                            RightId = new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"),
                            Id = new Guid("12976eb3-e9b3-49ca-88e9-b8fb6ad41caf")
                        },
                        new
                        {
                            RoleId = new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                            RightId = new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"),
                            Id = new Guid("4ac2888f-5d27-45b4-b200-cdb5cae8af48")
                        },
                        new
                        {
                            RoleId = new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                            RightId = new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"),
                            Id = new Guid("cf4c5126-3ff6-442c-82ec-a89407b00e0d")
                        },
                        new
                        {
                            RoleId = new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"),
                            RightId = new Guid("875fe2b2-73e1-4c04-873c-daa459220cb7"),
                            Id = new Guid("07b77346-f038-429f-b6f3-082f1900ce5f")
                        },
                        new
                        {
                            RoleId = new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"),
                            RightId = new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"),
                            Id = new Guid("8a4540f5-92f4-40c7-b2e5-c2f8af59eb0d")
                        },
                        new
                        {
                            RoleId = new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                            RightId = new Guid("8897880e-3ffe-4842-9c22-ff3a212448bc"),
                            Id = new Guid("04df7fc6-db0b-4d37-9209-2a69418c8c1a")
                        },
                        new
                        {
                            RoleId = new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                            RightId = new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"),
                            Id = new Guid("5f23aeea-6df0-4c47-8f41-b497cd5e7339")
                        },
                        new
                        {
                            RoleId = new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"),
                            RightId = new Guid("086de130-5dd4-4542-a459-ce819a9b3a08"),
                            Id = new Guid("2248243f-b915-4952-999f-31643cd7f719")
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.RoleEntities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 577, DateTimeKind.Local).AddTicks(7390),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 580, DateTimeKind.Local).AddTicks(4660),
                            Name = "管理员"
                        },
                        new
                        {
                            Id = new Guid("6b42039e-2da4-4d1e-876f-a0ef001537c3"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 580, DateTimeKind.Local).AddTicks(5620),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 580, DateTimeKind.Local).AddTicks(5630),
                            Name = "内容审查"
                        },
                        new
                        {
                            Id = new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 580, DateTimeKind.Local).AddTicks(5650),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 580, DateTimeKind.Local).AddTicks(5650),
                            Name = "自媒体"
                        },
                        new
                        {
                            Id = new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 580, DateTimeKind.Local).AddTicks(5650),
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 580, DateTimeKind.Local).AddTicks(5660),
                            Name = "用户"
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.UserEntities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Avatar")
                        .HasColumnType("TEXT")
                        .HasMaxLength(300);

                    b.Property<string>("Introduce")
                        .HasColumnType("TEXT")
                        .HasMaxLength(1000);

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("States")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df5923716c"),
                            Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                            Introduce = "大家好，我是张世纪!",
                            NickName = "张世纪",
                            RoleId = new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                            States = true
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716440"),
                            Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                            Introduce = "大家好，我是苑紫清！",
                            NickName = "苑紫清",
                            RoleId = new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                            States = true
                        },
                        new
                        {
                            Id = new Guid("5efc910b-2f45-43df-afae-620d40542853"),
                            Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                            Introduce = "大家好，我是张三！",
                            NickName = "张三",
                            RoleId = new Guid("c04383df-5c8e-45c6-9841-9c775ab5af2e"),
                            States = true
                        },
                        new
                        {
                            Id = new Guid("bbdee09c-089b-4d30-bece-44df59237100"),
                            Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                            Introduce = "大家好，我是李四！",
                            NickName = "李四",
                            RoleId = new Guid("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                            States = true
                        },
                        new
                        {
                            Id = new Guid("6fb600c1-9011-4fd7-9234-881379716400"),
                            Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                            Introduce = "大家好，我是王五！",
                            NickName = "王五",
                            RoleId = new Guid("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                            States = true
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.UserEntities.UserAuthe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<int>("AuthType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Credential")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Account")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserAuthes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("efce6067-0a2a-49b6-b432-1a81a7cae107"),
                            Account = "17692463717",
                            AuthType = 1,
                            Credential = "1234567890",
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(2450),
                            RegisterTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(2090),
                            UserId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c")
                        },
                        new
                        {
                            Id = new Guid("1b1cf9ec-0547-45b6-904f-1957e4684900"),
                            Account = "17692463717@qq.com",
                            AuthType = 2,
                            Credential = "1234567890",
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3220),
                            RegisterTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3210),
                            UserId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c")
                        },
                        new
                        {
                            Id = new Guid("a145e756-090e-46c7-ba6d-c61b980dfc36"),
                            Account = "13300339033",
                            AuthType = 1,
                            Credential = "1234567890",
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3250),
                            RegisterTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3250),
                            UserId = new Guid("6fb600c1-9011-4fd7-9234-881379716440")
                        },
                        new
                        {
                            Id = new Guid("b9190ee3-4ba8-47d2-b847-587293b70f19"),
                            Account = "14302330899",
                            AuthType = 1,
                            Credential = "1234567890",
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3260),
                            RegisterTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3260),
                            UserId = new Guid("5efc910b-2f45-43df-afae-620d40542853")
                        },
                        new
                        {
                            Id = new Guid("f452ced4-8337-45ed-9d80-b40dc7563c79"),
                            Account = "13402330890",
                            AuthType = 1,
                            Credential = "1234567890",
                            ModifyTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3270),
                            RegisterTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(3270),
                            UserId = new Guid("6fb600c1-9011-4fd7-9234-881379716400")
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.WebEntities.Banner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(1000);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.ToTable("Banners");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9300c6fb-10ad-41b6-adf3-bc415c18ef90"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(4860),
                            Picture = "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png",
                            Remark = "百度一下，你就知道",
                            Url = "https://www.baidu.com/"
                        },
                        new
                        {
                            Id = new Guid("3073cdae-a898-4275-9487-209527f74d33"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(5580),
                            Picture = "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png",
                            Remark = "百度一下，你就知道",
                            Url = "https://www.baidu.com/"
                        },
                        new
                        {
                            Id = new Guid("a224282b-65d5-4439-bb17-0de26454e8fc"),
                            CreateTime = new DateTime(2020, 6, 5, 9, 26, 7, 581, DateTimeKind.Local).AddTicks(5610),
                            Picture = "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png",
                            Remark = "百度一下，你就知道",
                            Url = "https://www.baidu.com/"
                        });
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Article", b =>
                {
                    b.HasOne("NewsPublish.Database.Entities.ArticleEntities.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsPublish.Database.Entities.UserEntities.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.ArticleTag", b =>
                {
                    b.HasOne("NewsPublish.Database.Entities.ArticleEntities.Article", "Article")
                        .WithMany("ArticleTags")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsPublish.Database.Entities.ArticleEntities.Tag", "Tag")
                        .WithMany("ArticleTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Comment", b =>
                {
                    b.HasOne("NewsPublish.Database.Entities.ArticleEntities.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsPublish.Database.Entities.UserEntities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.ArticleEntities.Reply", b =>
                {
                    b.HasOne("NewsPublish.Database.Entities.ArticleEntities.Comment", "Comment")
                        .WithMany("Replies")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsPublish.Database.Entities.UserEntities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.RoleEntities.RightRole", b =>
                {
                    b.HasOne("NewsPublish.Database.Entities.RoleEntities.Right", "Right")
                        .WithMany("RightRoles")
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsPublish.Database.Entities.RoleEntities.Role", "Role")
                        .WithMany("RightRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.UserEntities.User", b =>
                {
                    b.HasOne("NewsPublish.Database.Entities.RoleEntities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsPublish.Database.Entities.UserEntities.UserAuthe", b =>
                {
                    b.HasOne("NewsPublish.Database.Entities.UserEntities.User", "User")
                        .WithMany("UserAuthes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}