using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using VaCant.EFCore;
using VaCant.Initialization;
using VaCant.WebMvc.Filter;

namespace VaCant.WebMvc
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
        //ConfigureServices添加服务和功能，  Configure配置添加好的服务的使用方式
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AddRazorRuntimeCompilation 可以使页面像以前一样可以修改页面实时刷新，而不需要重新生成
            services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
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
            //services.AddDistributedMemoryCache();
            services.AddSession();

            #region 第一种权限过滤
            //全局注册Filter
            services.AddMvc(options =>
            {
                // options.Filters.Add(typeof(Filter.VaCantAuthorizationFilter));
                // options.Filters.Add<Filter.CheckLoginAuthorizeFilter>();
                options.Filters.Add<Filter.MyExceptionFilter>();
               // options.Filters.Add<Filter.MyActionFilter>();
            });
            //只在控制器或action上[ServiceFilter(typeof(VaCantAuthorizationFilter))]才有用
            services.AddScoped<VaCantAuthorizationFilter>();
            //services.AddAuthentication(option =>
            //{
            //    option.DefaultScheme = "Cookie";
            //    option.DefaultChallengeScheme = "Cookie";
            //    option.DefaultAuthenticateScheme = "Cookie";
            //    option.DefaultForbidScheme = "Cookie";
            //    option.DefaultSignInScheme = "Cookie";
            //    option.DefaultSignOutScheme = "Cookie";
            //}).AddCookie("Cookie", option =>
            //{
            //    option.LoginPath = "/Account/Login";
            //    option.AccessDeniedPath = "/Account/Forbidden";
            //    //.......
            //});
            #endregion

            #region Quartz定时任务

            //注册ISchedulerFactory的实例
            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //StartQuartzAsync();
            #endregion

            #region 第二种权限过滤
            ////权限要求参数
            //var permissionRequirement = new PermissionRequirement(
            //    "/Home/visitDeny",// 拒绝授权的跳转地址
            //    ClaimTypes.Name,//基于用户名的授权
            //    expiration: TimeSpan.FromSeconds(60 * 5)//接口的过期时间
            //    );

            ////【授权】
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Permission", policy => policy.Requirements.Add(permissionRequirement));
            //});
            //// 注入权限处理器
            //services.AddTransient<IAuthorizationHandler, PermissionHandler>();
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
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            //开启身份认证
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                     //pattern: "{controller=Home}/{action=Index}/{id?}");
                     pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }

        /// <summary>
        /// Quartz异步执行定时任务
        /// </summary>
        /// <returns></returns>
        private async Task StartQuartzAsync()
        {
            // 参考文章 https://www.cnblogs.com/MicroHeart/p/9402731.html
            //https://www.cnblogs.com/dangzhensheng/p/10496278.html
            StdSchedulerFactory _schedulerFactory = new StdSchedulerFactory();
            //1.通过工场类获得调度器
            IScheduler _scheduler = await _schedulerFactory.GetScheduler();
            //2.开启调度器
            await _scheduler.Start();
            //3.创建触发器(也叫时间策略)
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(3).RepeatForever())//每10秒执行一次
                            .Build();
            //4.创建作业实例
            //Jobs即我们需要执行的作业
            var jobDetail = JobBuilder.Create<DemoJob>()
                            .WithIdentity("Myjob", "group")//我们给这个作业取了个“Myjob”的名字，并取了个组名为“group”
                            .Build();
            //5.将触发器和作业任务绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
