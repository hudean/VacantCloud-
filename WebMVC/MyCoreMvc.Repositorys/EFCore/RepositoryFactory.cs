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
    public static class RepositoryFactory
    {

        public static  void Injection(IServiceCollection services, IConfiguration Configuration)
        {
            string dbType = Configuration.GetValue<string>("DbType");
            //string dbType2 = Configuration["DbType"];
            string connStr = Configuration.GetConnectionString("Default");
            switch (dbType)
            {
                case "SqlServer":
                    //DbHelper.DbType = DatabaseType.SqlServer;
                    //database = new SqlServerDatabase(dbConnectionString);
                    //依赖注入仓储  
                    services.AddDbContext<SqlServerDbContext>(options =>
                    {
                        // options.UseSqlServer(Configuration.GetConnectionString("Default"));//获取配置的连接字符串
                        options.UseSqlServer(connStr);

                    });
                    services.AddScoped(typeof(IRepository<,>), typeof(SqlServerRepository<,>));
                    services.AddScoped(typeof(IRepository<>), typeof(SqlServerRepository<>));
                    break;
                case "MySql":
                    //DbHelper.DbType = DatabaseType.MySql;
                    //database = new MySqlDatabase(dbConnectionString);
                    services.AddDbContext<MySqlDbContext>(options =>
                    {
                        //options.UseMySql(Configuration.GetConnectionString("Default"));//获取配置的连接字符串
                        options.UseMySql(connStr);

                    });
                    services.AddScoped(typeof(IRepository<,>), typeof(MysqlRepository<,>));
                    services.AddScoped(typeof(IRepository<>), typeof(MysqlRepository<>));
                    break;
                case "Oracle":
                    //DbHelper.DbType = DatabaseType.Oracle;
                    // 支持Oracle或是更多数据库请参考上面SqlServer或是MySql的写法
                    break;
                default:
                    throw new Exception("未找到数据库配置");
            }
            try
            {
                //检查是否连接到数据库
                DbConnection dbConnection = new MySqlConnection();
                //DbConnection db = 
                dbConnection.ConnectionString = connStr;
                var cmd = dbConnection.CreateCommand();
                dbConnection.Open();
                //cmd.CommandText = "select top 1 UserName from AbpUsers";
                //object obj = cmd.ExecuteScalar();
                //string str = obj.ToString();
                //IDbConnection dbConnection = SqlClientFactory.Instance.CreateConnection();
                //dbConnection.ConnectionString = connStr;
                //dbConnection.Open();
                //var cmd= dbConnection.CreateCommand();
                ////cmd.CommandText = "select top 1 UserName from AbpUsers";
                //cmd.CommandText = "select itemname from iteminfo where id=1";
                //object obj=cmd.ExecuteScalar();
                //string str= obj.ToString();
                //dbConnection.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("数据库配置连接字符串错误请检查："+ex.ToString());
            }

            #region 批量注入
            //加载程序集MyApplication
             //var serviceAsm = Assembly.Load(new AssemblyName("MyApplication"));
            //foreach (Type serviceType in serviceAsm.GetTypes().Where(t => typeof(IBaseService).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract))
            //{
            //    var interfaceTypes = serviceType.GetInterfaces();
            //    foreach (var interfaceType in interfaceTypes)
            //    {
            //        services.AddScoped(interfaceType, serviceType);
            //    }
            //}

            #endregion
        }
    }
}
