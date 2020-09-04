using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCoreMvc.Initialization;
using MyCoreMvc.Repositorys;
using WebMVC.Filter;

namespace WebMVC
{
    public class Startup
    {

        //log4net日志
        //public static ILoggerRepository repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //log4net
           // repository = LogManager.CreateRepository("NETCoreRepository");
            //指定配置文件
           // XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            #region 已注释
            //注册服务连接数据库
            //services.AddDbContext<SqlServerDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("Default"));//获取配置的连接字符串
            //});
            //重点仓储和服务注入方式要一样
            //依赖注入仓储  
            //services.AddScoped(typeof(IRepository<,>), typeof(MyRepository<,>));
            //services.AddScoped(typeof(IRepository<>), typeof(MyRepository<>));

            //services.AddScoped<IUserService,UserService>();
            //services.AddScoped<IRoleService, RoleService>();
            #endregion

            //调用工厂模式进行依赖注入
            InitializationFactory.Injection(services, Configuration);

            //基于内存存储Session
            services.AddDistributedMemoryCache();
            services.AddSession();

            #region 第一种权限过滤
            services.AddMvc(options =>
            {
                options.Filters.Add<Filter.CheckLoginAuthorizeFilter>();
                options.Filters.Add<Filter.MyExceptionFilter>();
            });
            #endregion




            #region 第二种权限过滤
            //权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/Home/visitDeny",// 拒绝授权的跳转地址
                ClaimTypes.Name,//基于用户名的授权
                expiration: TimeSpan.FromSeconds(60 * 5)//接口的过期时间
                );

            //【授权】
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy => policy.Requirements.Add(permissionRequirement));
            });
            // 注入权限处理器
            services.AddTransient<IAuthorizationHandler, PermissionHandler>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
