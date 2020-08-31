using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCoreMvc.EFCore;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyCoreMvc.Repositorys
{
    /// <summary>
    /// 初始化工厂
    /// </summary>
    public static class InitializationFactory
    {

        public static  void Injection(IServiceCollection services, IConfiguration Configuration)
        {
            string dbType = Configuration.GetValue<string>("DbType");
            string DIMethod= Configuration.GetValue<string>("DIMethod");
            //string dbType2 = Configuration["DbType"];
            string connStr = Configuration.GetConnectionString("Default");
            switch (dbType)
            {
                //注入DbContext
                case "SqlServer":
                    services.AddDbContext<SqlServerDbContext>(options =>
                    {
                        // options.UseSqlServer(Configuration.GetConnectionString("Default"));//获取配置的连接字符串
                        options.UseSqlServer(connStr);
                    });
                    break;
                case "MySql":
                    services.AddDbContext<MySqlDbContext>(options =>
                    {
                        options.UseMySql(connStr);

                    });
                    break;
                case "Oracle":
                    // 支持Oracle或是更多数据库请参考上面SqlServer或是MySql的写法
                    break;
                default:
                    throw new Exception("未找到数据库配置");
            }

            //依赖注入仓储  
            //参考 文档https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
            switch (DIMethod)
            {
                case "AddScoped":
                    // AddScoped 作用域 方法使用范围内生存期（单个请求的生存期）注册服务。作用域生存期服务 (AddScoped) 以每个客户端请求（连接）一次的方式创建。
                    services.AddScoped(typeof(IRepository<,>), typeof(SqlServerRepository<,>));
                    services.AddScoped(typeof(IRepository<>), typeof(SqlServerRepository<>));
                    break;

                case "AddTransient":
                    //暂时/瞬态 暂时生存期服务 (AddTransient) 是每次从服务容器进行请求时创建的。 这种生存期适合轻量级、 无状态的服务。
                    services.AddTransient(typeof(IRepository<,>), typeof(SqlServerRepository<,>));
                    services.AddTransient(typeof(IRepository<>), typeof(SqlServerRepository<>));
                    break;
                case "AddSingleton":
                    //单例 在首次请求它们时进行创建；或者在向容器直接提供实现实例时由开发人员进行创建。 很少用到此方法
                    services.AddSingleton(typeof(IRepository<,>), typeof(SqlServerRepository<,>));
                    services.AddSingleton(typeof(IRepository<>), typeof(SqlServerRepository<>));
                    break;
                default:
                    services.AddScoped(typeof(IRepository<,>), typeof(SqlServerRepository<,>));
                    services.AddScoped(typeof(IRepository<>), typeof(SqlServerRepository<>));
                    break;
            }
            try
            {
                //检查是否连接到数据库
                DbConnection dbConnection = new MySqlConnection();
                dbConnection.ConnectionString = connStr;
                var cmd = dbConnection.CreateCommand();
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("数据库配置连接字符串错误请检查："+ex.ToString());
            }

            #region 批量注入Services
            //加载程序集MyApplication
            var serviceAsm = Assembly.Load(new AssemblyName("MyApplication"));
            foreach (Type serviceType in serviceAsm.GetTypes().Where(t => typeof(IBaseService).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract))
            {
                var interfaceTypes = serviceType.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }

            #endregion
        }
    }
}
