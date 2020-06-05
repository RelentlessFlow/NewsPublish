using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.OpenApi.Models;
using NewsPublish.Authorization.ConfigurationModel;
using NewsPublish.Authorization.Filter;
using NewsPublish.Database.Data;
using NewsPublish.Infrastructure.Services;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.Implementation;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Implementation;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices;
using NewsPublish.Infrastructure.Services.CommonServices.Implementation;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;
using Newtonsoft.Json.Serialization;

namespace NewsPublish
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; //允许跨域

        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            // 配置Swagger
            services.AddSwaggerGen(m =>
                {
                    m.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "NewsPublish",
                        Version = "v1"
                    });
                });
            
            
            // 配置跨域
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,

                    builder => builder.AllowAnyOrigin()
                        .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
                        .AllowAnyMethod().AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination")

                    // .AllowCredentials()

                );
            });
            
            services.AddControllers(setup =>
                {
                    setup.ReturnHttpNotAcceptable = true;
                }).AddNewtonsoftJson(setup =>
                {
                    setup.SerializerSettings.ContractResolver = 
                        new CamelCasePropertyNamesContractResolver();
                }).AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(setup =>
                {
                    setup.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "http://www.baidu.com",
                            Title = "有错误！！！",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "请看详细信息",
                            Instance = context.HttpContext.Request.Path
                        };

                        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });
            
            // AutoMapper 注入默认配置
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
            
            // 处理依赖注入
            
            // 基础服务依赖注入
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWebRepository, WebRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            
            // 管理员服务依赖注入
            services.AddScoped<IUserRepositoryExtendAdmin, UserRepositoryExtendAdmin>();


            // 权限校验
            // 权限校验
            services.AddSingleton<ITokenList, TokenList>();
            
            
            // 系统组件
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            // 过滤器
            services.AddScoped<AutheFilter>();
            services.AddScoped<AdminFilter>();
            services.AddScoped<AssessorFilter>();
            services.AddScoped<CreatorFilter>();
            services.AddScoped<UserFilter>();

            // 配置注入
            services.Configure<SystemFunctionOptionName>(_configuration.GetSection("SystemFunctionOptionName"));


            services.AddDbContextPool<RoutineDbContext>(
                option=>
                {
                    option.UseSqlite("Data Source=news3.db");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            
            app.UseStaticFiles();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            // 注册Swagger中间件
            app.UseSwagger();
            
            app.UseSwaggerUI(m =>
            {
                m.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsPublish");
            });

            
            // 配置跨域
            app.UseCors(MyAllowSpecificOrigins);
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}