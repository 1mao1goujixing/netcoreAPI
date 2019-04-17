using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using MynetCore.Repository.sugar;
using MynetCore.Tool.Redis;
using MynetCoreAPI.AOP;
using MynetCoreAPI.AuthHelper;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MynetCoreAPI
{
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Configuration属性
        /// </summary>
        public IConfiguration Configuration { get; }

       
     
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));//这是AutoMapper的2.0新特性
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v0.1.0",
                    Title = "MynetCoreAPI",

                    Description = "框架说明文档",

                    TermsOfService = "None",

                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "TD.Core",
                        Email = "heihei",
                        Url = "www.baidu.com"
                    }
                
                });
                //配置 MynetCoreAPI.xml
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "MynetCoreAPI.xml");//这个就是刚刚配置的xml文件名
                var xmlModelPath = Path.Combine(basePath, "MynetCore.Model.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                c.IncludeXmlComments(xmlModelPath);
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });

            });
            //缓存
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });
            //认证
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("AdminType").Build());
            });

            #endregion
            #region CORS 用于跨域 
            services.AddCors(c =>
            {
                c.AddPolicy("AllRequests", policy =>
                {
                    policy.AllowAnyOrigin()//允许任何源
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy.WithOrigins("http://localhost:8088")
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .WithHeaders("authorization");
                    policy.WithOrigins("http://localhost:8080")
                   .WithMethods("GET", "POST", "PUT", "DELETE")
                   .WithHeaders("authorization");
                });
            });
            #endregion
            services.AddScoped<ICaching, MemoryCaching>();//记得把缓存注入！！！
            services.AddScoped<IRedisCacheManager, RedisCacheManager>();//redis缓存注入！！！
            #region AutoFac
            //实例化容器
            var builder = new ContainerBuilder();
            //注册要通过反射创建的组件
            //单个类的注入
            // builder.RegisterType<UserInfoServices>().As<UserInfoIServices>();
            builder.RegisterType<LogAOP>();
            builder.RegisterType<CacheAOP>();
            //使用程序集注入
            var assemblysServices = Assembly.Load("MynetCore.Services");
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces()
                      .InstancePerLifetimeScope()
                      .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      .InterceptedBy(typeof(LogAOP), typeof(CacheAOP));//可以直接替换拦截器
          //指定已扫描程序集中的类型注册为提供所有其实现的接口。
            var assemblysRepository = Assembly.Load("MynetCore.Repository");
            builder.RegisterAssemblyTypes(assemblysRepository)
                      .AsImplementedInterfaces();
                     


            //将services 填充Autofac 容器生成器
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainter = builder.Build();
            #endregion
            
            //获取数据库连接
            BaseDBConfig.ConnectionString = Configuration.GetSection("AppSettings:SqlServerConnection").Value;
            //将services填充到Autofac容器生成器中
            return new AutofacServiceProvider(ApplicationContainter);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                //app.UseHsts();

            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });


            #endregion
            app.UseMiddleware<TokenAuth>();
            app.UseMvc();
            app.UseStaticFiles();//用于访问wwwroot下的文件 
        }
    }
}
