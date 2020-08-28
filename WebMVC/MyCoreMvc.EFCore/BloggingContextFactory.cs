using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreMvc.EFCore
{
    /// <summary>
    /// 迁移帮助类
    /// 如果实现此接口的类在与派生的项目相同的项目中 DbContext 或在应用程序的启动项目中找到，则这些工具将绕过创建 DbContext 的其他方法，并改用设计时工厂
    /// 参考https://go.microsoft.com/fwlink/?linkid=851728
    /// </summary>
    public class BloggingContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
    {
        public SqlServerDbContext CreateDbContext(string[] args)
        {
            
            //迁移命令
            //DbContext所在项目设为启动项，然后在nuget程序控制台分别执行下面两行命令
            //Add-Migration init_20200828 -c SqlServerDbContext
            // Update-Database -c SqlServerDbContext
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost; Database=ServerDb; Trusted_Connection=True;uid=sa;pwd=123456;");

            return new SqlServerDbContext(optionsBuilder.Options);
        }
    }
}
