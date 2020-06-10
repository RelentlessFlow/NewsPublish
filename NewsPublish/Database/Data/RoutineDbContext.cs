using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Database.Entities.RoleEntities;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Database.Entities.WebEntities;

namespace NewsPublish.Database.Data
{
    /// <summary>
    /// EF Core 数据库配置文件
    /// </summary>
    public class RoutineDbContext : DbContext
    {
        // 注入基础连接配置
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options)
        {
        }

        // 数据库映射
        public DbSet<Role> Roles { get; set; }
        public DbSet<Right> Rights { get; set; }
        public DbSet<RightRole> RightRoles { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserAuthe> UserAuthes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        // 点赞
        public DbSet<Star> Stars { get; set; }

        // 多对多
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }

        public DbSet<Banner> Banners { get; set; }
        
        // 创作者认证报表
        public DbSet<CreatorAutheAudit> CreatorAutheAudits { get; set; }
        // 文章授权报表
        public DbSet<ArticleReviewAudit> ArticleReviewAudits { get; set; }

        // 配置数据库约束
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 有关用户信息的表约束
            modelBuilder.Entity<User>()
                .Property(x => x.NickName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(x => x.Avatar).HasMaxLength(300);
            modelBuilder.Entity<User>()
                .Property(x => x.Introduce).HasMaxLength(1000);
            modelBuilder.Entity<User>()
                .Property(x => x.States).IsRequired();
            modelBuilder.Entity<UserAuthe>()
                .Property(x => x.Account).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserAuthe>()
                .HasIndex(c => c.Account)
                .IsUnique();
            modelBuilder.Entity<UserAuthe>()
                .Property(x => x.Credential).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Role>()
                .Property(x => x.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Right>()
                .Property(x => x.Name).IsRequired().HasMaxLength(50);

            // 角色名称唯一约束
            modelBuilder.Entity<Role>()
                .HasIndex(b => b.Name).IsUnique();
            // 权限功能名称唯一约束
            modelBuilder.Entity<Right>()
                .HasIndex(b => b.Name).IsUnique();

            // 一个用户对应多个验证信息 一对多关系 级联删除
            modelBuilder.Entity<UserAuthe>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserAuthes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // 一个Role对应多个用户，一对多关系，级联删除
            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            // 一个Role对应许很多Right，多对多
            modelBuilder.Entity<RightRole>().HasKey(x => new {x.RoleId, x.RightId});
            modelBuilder.Entity<RightRole>()
                .HasOne(x => x.Right)
                .WithMany(x => x.RightRoles)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.RightId);
            modelBuilder.Entity<RightRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.RightRoles)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.RoleId);


            // 有关文章的表约束
            modelBuilder.Entity<Article>()
                .Property(x => x.Title).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Article>()
                .Property(x => x.CoverPic).HasMaxLength(300);
            modelBuilder.Entity<Article>()
                .Property(x => x.Content).IsRequired();
            modelBuilder.Entity<Article>()
                .Property(x => x.CreateTime).IsRequired();
            modelBuilder.Entity<Article>()
                .Property(x => x.States).IsRequired();
            modelBuilder.Entity<Category>()
                .Property(x => x.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Category>()
                .Property(x => x.Remark).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<Category>()
                .HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<Comment>()
                .Property(x => x.Content).IsRequired();
            modelBuilder.Entity<Reply>()
                .Property(x => x.CommentId).IsRequired();
            modelBuilder.Entity<Reply>()
                .Property(x => x.UserId).IsRequired();
            modelBuilder.Entity<Reply>()
                .Property(x => x.Content).IsRequired();
            modelBuilder.Entity<Reply>()
                .Property(x => x.ReceivedId).IsRequired();
            modelBuilder.Entity<Reply>()
                .Property(x => x.CreateTime).IsRequired();

            modelBuilder.Entity<Tag>()
                .Property(x => x.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Tag>()
                .HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Star>()
                .Property(x => x.Type).IsRequired();
            modelBuilder.Entity<Star>()
                .Property(x => x.Count).IsRequired();
            modelBuilder.Entity<Star>()
                .Property(x => x.StartId).IsRequired();
            

            // 一个分类中包含很多文章
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(a => a.Articles)
                .HasForeignKey(a => a.CategoryId);
            // 一个用户会发布很多文章
            modelBuilder.Entity<Article>()
                .HasOne(a => a.User)
                .WithMany(a => a.Articles)
                .HasForeignKey(a => a.UserId);
            // 一个文章会有很多评论
            modelBuilder.Entity<Comment>()
                .HasOne(a => a.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(a => a.ArticleId);
            // 一个评论下可能有很多回复
            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Replies);
            //  文章标签：多对多
            modelBuilder.Entity<ArticleTag>().HasKey(x => new {x.ArticleId, x.TagId});
            modelBuilder.Entity<ArticleTag>()
                .HasOne(x => x.Article)
                .WithMany(x => x.ArticleTags)
                .HasForeignKey(x => x.ArticleId);
            modelBuilder.Entity<ArticleTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.ArticleTags)
                .HasForeignKey(x => x.TagId);

        
            // Web站点有关的实体
            modelBuilder.Entity<Banner>()
                .Property(x => x.Picture).IsRequired();
            modelBuilder.Entity<Banner>()
                .Property(x => x.Url).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<Banner>()
                .Property(x => x.Remark).IsRequired().HasMaxLength(1000);
            
            // 创作者中心报表表约束
            modelBuilder.Entity<CreatorAutheAudit>()
                .Property(x => x.Remark).IsRequired();
            modelBuilder.Entity<CreatorAutheAudit>()
                .Property(x => x.IsPass).IsRequired();
            modelBuilder.Entity<CreatorAutheAudit>()
                .Property(x => x.AuditStatus).IsRequired();
            
            // 文章审核报表表约束
            modelBuilder.Entity<ArticleReviewAudit>()
                .Property(x => x.CreateTime).IsRequired();
            modelBuilder.Entity<ArticleReviewAudit>()
                .Property(x => x.IsPass).IsRequired();
            modelBuilder.Entity<ArticleReviewAudit>()
                .Property(x => x.AuditStatus).IsRequired();
            
            modelBuilder.Entity<ArticleReviewAudit>()
                .HasOne(s => s.Article)
                .WithMany(x => x.Audits)
                .HasForeignKey(x => x.ArticleId);
            
            // 种子数据
            modelBuilder.Entity<Right>().HasData(
                new Right
                {
                    Id = Guid.Parse("fe4ebf2e-f1e9-48c8-9a52-d297e865c4a1"),
                    Name = "系统管理"
                },
                new Right
                {
                    Id = Guid.Parse("8897880e-3ffe-4842-9c22-ff3a212448bc"),
                    Name = "内容创作"
                },
                new Right
                {
                    Id = Guid.Parse("086de130-5dd4-4542-a459-ce819a9b3a08"),
                    Name = "内容查看"
                },
                new Right
                {
                    Id = Guid.Parse("875fe2b2-73e1-4c04-873c-daa459220cb7"),
                    Name = "内容审核"
                });
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.Parse("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    Name = "管理员"
                },
                new Role
                {
                    Id = Guid.Parse("6b42039e-2da4-4d1e-876f-a0ef001537c3"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    Name = "内容审查"
                },
                new Role
                {
                    Id = Guid.Parse("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    Name = "自媒体"
                },
                new Role
                {
                    Id = Guid.Parse("c04383df-5c8e-45c6-9841-9c775ab5af2e"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    Name = "用户"
                }
            );
            modelBuilder.Entity<RightRole>().HasData(
                // 分配给管理员权限
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                    RightId = Guid.Parse("fe4ebf2e-f1e9-48c8-9a52-d297e865c4a1")
                },
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                    RightId = Guid.Parse("8897880e-3ffe-4842-9c22-ff3a212448bc")
                },
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                    RightId = Guid.Parse("086de130-5dd4-4542-a459-ce819a9b3a08")
                },
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                    RightId = Guid.Parse("875fe2b2-73e1-4c04-873c-daa459220cb7")
                },
                
                // 分配给审查员权限
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("6b42039e-2da4-4d1e-876f-a0ef001537c3"),
                    RightId = Guid.Parse("875fe2b2-73e1-4c04-873c-daa459220cb7"),
                },
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("6b42039e-2da4-4d1e-876f-a0ef001537c3"),
                    RightId = Guid.Parse("086de130-5dd4-4542-a459-ce819a9b3a08"),
                },


                // 分配给自媒体权限
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                    RightId = Guid.Parse("8897880e-3ffe-4842-9c22-ff3a212448bc"),
                },
                new RightRole
                    {
                        Id = Guid.NewGuid(),
                        RoleId = Guid.Parse("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                        RightId = Guid.Parse("086de130-5dd4-4542-a459-ce819a9b3a08"),
                    },
                
                // 分配给一般用户权限
                new RightRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = Guid.Parse("c04383df-5c8e-45c6-9841-9c775ab5af2e"),
                    RightId = Guid.Parse("086de130-5dd4-4542-a459-ce819a9b3a08"),
                }
                
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c"),
                    NickName = "张世纪",
                    Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                    Introduce = "大家好，我是张世纪!",
                    RoleId = Guid.Parse("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                    States = true
                },
                new User
                {
                    Id = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440"),
                    NickName = "苑紫清",
                    Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                    Introduce = "大家好，我是苑紫清！",
                    RoleId = Guid.Parse("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                    States = true
                },
                new User
                {
                    Id = Guid.Parse("5efc910b-2f45-43df-afae-620d40542853"),
                    NickName = "张三",
                    Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                    Introduce = "大家好，我是张三！",
                    RoleId = Guid.Parse("c04383df-5c8e-45c6-9841-9c775ab5af2e"),
                    States = true
                },
                new User
                {
                    Id = Guid.Parse("bbdee09c-089b-4d30-bece-44df59237100"),
                    NickName = "李四",
                    Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                    Introduce = "大家好，我是李四！",
                    RoleId = Guid.Parse("b44b39d1-f61d-4e58-94a7-db1603a82c34"),
                    States = true
                },
                new User
                {
                    Id = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716400"),
                    NickName = "王五",
                    Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                    Introduce = "大家好，我是王五！",
                    RoleId = Guid.Parse("b6355a7e-4511-45ba-adfb-cc4d026e1f6f"),
                    States = true
                },
                new User
                {
                    Id = Guid.Parse("6091967b-0952-425a-9eda-840b24da5534"),
                    NickName = "卑微的用户",
                    Avatar = "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                    Introduce = "卑微的普通用户",
                    RoleId = Guid.Parse("c04383df-5c8e-45c6-9841-9c775ab5af2e"),
                    States = true
                }
            );

            modelBuilder.Entity<UserAuthe>().HasData(
                new UserAuthe
                {
                    Id = Guid.NewGuid(),
                    Account = "17692463717",
                    AuthType = UserAuthType.手机号,
                    Credential = "1234567890",
                    RegisterTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    UserId = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c")
                },
                new UserAuthe
                {
                    Id = Guid.NewGuid(),
                    Account = "17692463717@qq.com",
                    AuthType = UserAuthType.邮箱,
                    Credential = "1234567890",
                    RegisterTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    UserId = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c")
                },
                new UserAuthe
                {
                    Id = Guid.NewGuid(),
                    Account = "13300339033",
                    AuthType = UserAuthType.手机号,
                    Credential = "1234567890",
                    RegisterTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    UserId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440")
                },
                new UserAuthe
                {
                    Id = Guid.NewGuid(),
                    Account = "14302330899",
                    AuthType = UserAuthType.手机号,
                    Credential = "1234567890",
                    RegisterTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    UserId = Guid.Parse("5efc910b-2f45-43df-afae-620d40542853")
                },
                new UserAuthe
                {
                    Id = Guid.NewGuid(),
                    Account = "13402330890",
                    AuthType = UserAuthType.手机号,
                    Credential = "1234567890",
                    RegisterTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    UserId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716400")
                }
            );

            modelBuilder.Entity<CreatorAutheAudit>().HasData(
                new CreatorAutheAudit
                {
                    Id = Guid.Parse("afda3d6d-508a-4552-bf69-026dc22d791d"),
                    Remark = "身份信息如下：",
                    CreateTime = DateTime.Now,
                    UserId = Guid.Parse("6091967b-0952-425a-9eda-840b24da5534"),
                    AuditStatus = false,
                    IsPass = false
                });
            
            modelBuilder.Entity<CreatorAutheAudit>().HasData(
                new CreatorAutheAudit
                {
                    Id = Guid.Parse("01883fc0-bc86-4b32-9de9-b2f7c4cb582a"),
                    Remark = "2身份信息如下：",
                    CreateTime = DateTime.Now,
                    UserId = Guid.Parse("6091967b-0952-425a-9eda-840b24da5534")
                });
            modelBuilder.Entity<Banner>().HasData(
                new Banner
                {
                    Id = Guid.NewGuid(),
                    Picture = "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png",
                    Url = "https://www.baidu.com/",
                    CreateTime = DateTime.Now,
                    Remark = "百度一下，你就知道"
                },
                new Banner
                {
                    Id = Guid.NewGuid(),
                    Picture = "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png",
                    Url = "https://www.baidu.com/",
                    CreateTime = DateTime.Now,
                    Remark = "百度一下，你就知道"
                },
                new Banner
                {
                    Id = Guid.NewGuid(),
                    Picture = "https://www.baidu.com/img/pc_cc75653cd975aea6d4ba1f59b3697455.png",
                    Url = "https://www.baidu.com/",
                    CreateTime = DateTime.Now,
                    Remark = "百度一下，你就知道"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                    Name = "默认",
                    Remark = "默认分类！"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "娱乐",
                    Remark = "娱乐新闻"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "财经",
                    Remark = "财经新闻"
                },
                new Category
                {
                    Id = Guid.Parse("42a2bae9-09c2-47ae-94de-3912debff652"),
                    Name = "科技",
                    Remark = "科技新闻"
                }
            );
            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Id = Guid.Parse("ca88d765-8c17-4378-bd42-6146aec1a9ca"),
                    Title = "DigiTimes：苹果未来 AirPods 将搭载“环境光传感器”",
                    CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525_202833_144.png",
                    Content = "根据外媒macrumors消息，DigiTimes最近的一份报告显示苹果希望在未来几年内将环境光传感器集成到新型的AirPods中。",
                    UserId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440"),
                    CategoryId = Guid.Parse("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    States = true
                },
                new Article
                {
                    Id = Guid.Parse("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                    Title = "华为“去美国化”的成功几率有多大？”",
                    CoverPic = "https://img.ithome.com/newsuploadfiles/2020/2/20200214_090339_584.jpg",
                    Content = "一直以来，华为公司就像美国政府的“眼中钉”，不断遭美制裁。近两年，美国政府对华为的制裁愈演愈烈，甚至让其对外发声时只能苛求“活下来”。",
                    UserId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440"),
                    CategoryId = Guid.Parse("42a2bae9-09c2-47ae-94de-3912debff652"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    States = true
                },
                new Article
                {
                    Id = Guid.Parse("84e6a64f-53c8-4b30-8735-eabc6d48a549"),
                    Title = "董明珠称坚决不裁员：员工少1000块钱能活，没工作很难活下去",
                    CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525_191650_79.png",
                    Content =
                        "两会期间，董明珠在接受采访时表示：“六稳”“六保”首先就是稳企业、保居民就业。企业的生命力体现在遭遇不可抗力时的风险防控能力，这个时候还能呵护我的员工，让他们在这很安全，我们就要做这种风险防控，今年一下真的给兑现了。",
                    UserId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440"),
                    CategoryId = Guid.Parse("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    States = false
                },
                new Article
                {
                    Id = Guid.Parse("11d67370-955b-4976-9285-834721ca57a5"),
                    Title = "习近平在宁夏考察时强调 决胜全面建成小康社会决战脱贫攻坚 继续建设经济繁荣民族团结环境优美人民富裕的美丽新宁夏”",
                    CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525102542_7599.jpg",
                    Content = "新华社银川6月10日电 中共中央总书记、国家主席、中央军委主席习近平近日在宁夏考察时强调，要全面落实党中央决策部署，坚持稳中求进工作总基调，坚持新发展理念，落实全国“两会”工作部署，坚决打好三大攻坚战，扎实做好“六稳”工作，全面落实“六保”任务，努力克服新冠肺炎疫情带来的不利影响，优先稳就业保民生，决胜全面建成小康社会，决战脱贫攻坚，继续建设经济繁荣、民族团结、环境优美、人民富裕的美丽新宁夏。",
                    UserId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440"),
                    CategoryId = Guid.Parse("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    States = true
                },
                new Article
                {
                    Id = Guid.Parse("30c1f151-b97f-46b0-8d1f-e327051b67c8"),
                    Title = "史上最大规模！天猫618今日正式启动”",
                    CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525102542_7599.jpg",
                    Content = "今天，天猫618正式启动。据天猫官方数据显示，第1小时预售成交额同比增长515 %！",
                    UserId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440"),
                    CategoryId = Guid.Parse("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    States = true
                },
                new Article
                {
                    Id = Guid.Parse("0a0e5629-cf5d-438f-abd2-bf6918d232a9"),
                    Title = "小米全面屏电视 Pro 32 英寸新品发布，售价 899 元”",
                    CoverPic = "https://img.ithome.com/newsuploadfiles/2020/5/20200525_120326_295.jpg",
                    Content = "小米全面屏电视Pro 32英寸新品发布，5月25日全渠道开售!",
                    UserId = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c"),
                    CategoryId = Guid.Parse("1c4ec57b-35b4-4b06-971c-02e4fa316a92"),
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    States = true
                });
            modelBuilder.Entity<ArticleReviewAudit>().HasData(
                new List<ArticleReviewAudit>
                {
                    new ArticleReviewAudit
                    {
                        ArticleId = Guid.Parse("11d67370-955b-4976-9285-834721ca57a5"),
                        AuditStatus = true,
                        CreateTime = DateTime.Now,
                        ReviewTime = DateTime.Now,
                        Id = Guid.Parse("b48c02d8-4a57-4b7c-bb78-4d86c4f4f014"),
                        IsPass = true,
                        ReviewRemark = "文章写的不错，通过了！"
                    }
                }
            );
            modelBuilder.Entity<ArticleTag>().HasData(
                new ArticleTag
                {
                    ArticleId = Guid.Parse("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                    TagId = Guid.Parse("c456990c-05db-4965-9334-a295b7f0f993")
                },
                new ArticleTag
                {
                    ArticleId = Guid.Parse("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                    TagId = Guid.Parse("675db0e3-d57d-469b-a07f-09a3ef8a20eb")
                },
                new ArticleTag
                {
                    ArticleId = Guid.Parse("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                    TagId = Guid.Parse("78750f26-d3e7-4ebc-b69a-1c88f44e67ba")
                }
            );
            modelBuilder.Entity<Tag>().HasData(
                new Tag
                {
                    Id = Guid.Parse("c456990c-05db-4965-9334-a295b7f0f993"),
                    Name = "华为",
                    CreateTime = DateTime.Now
                },
                new Tag
                {
                    Id = Guid.Parse("675db0e3-d57d-469b-a07f-09a3ef8a20eb"),
                    Name = "国际",
                    CreateTime = DateTime.Now
                },
                new Tag
                {
                    Id = Guid.Parse("78750f26-d3e7-4ebc-b69a-1c88f44e67ba"),
                    Name = "科技",
                    CreateTime = DateTime.Now
                });
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = Guid.Parse("e75c9ede-8394-43f6-821d-e06f36024f9e"),
                    Content = "加油华为！",
                    CreateTime = DateTime.Now,
                    ArticleId = Guid.Parse("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                    UserId = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c")
                },
                new Comment
                {
                    Id = Guid.NewGuid(),
                    Content = "干翻老美！",
                    CreateTime = DateTime.Now,
                    ArticleId = Guid.Parse("5930a2b7-0f93-49e1-a5e9-c764238fb5c1"),
                    UserId = Guid.Parse("5efc910b-2f45-43df-afae-620d40542853")
                });
            modelBuilder.Entity<Reply>().HasData(
                new Reply
                {
                    Id = Guid.NewGuid(),
                    CommentId = Guid.Parse("e75c9ede-8394-43f6-821d-e06f36024f9e"),
                    UserId = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c"),
                    Content = "说得好!",
                    ReceivedId = Guid.Parse("5efc910b-2f45-43df-afae-620d40542853"),
                    CreateTime = DateTime.Now
                },
                new Reply
                {
                    Id = Guid.NewGuid(),
                    CommentId = Guid.Parse("e75c9ede-8394-43f6-821d-e06f36024f9e"),
                    UserId = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c"),
                    Content = "说得好!",
                    ReceivedId = Guid.Parse("5efc910b-2f45-43df-afae-620d40542853"),
                    CreateTime = DateTime.Now
                }
            );
        }
    }
}