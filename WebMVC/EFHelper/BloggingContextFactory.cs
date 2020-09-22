using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaCant.EFCore
{
    /// <summary>
    /// 迁移帮助类
    /// 如果实现此接口的类在与派生的项目相同的项目中 DbContext 或在应用程序的启动项目中找到，则这些工具将绕过创建 DbContext 的其他方法，并改用设计时工厂
    /// 参考https://go.microsoft.com/fwlink/?linkid=851728
    /// </summary>
    public class BloggingContextFactory : IDesignTimeDbContextFactory<EfCoreBaseDbContext>
    {
        public EfCoreBaseDbContext CreateDbContext(string[] args)
        {
            //迁移命令
            //把当前项目设置为启动项目
            //程序包控制台的默认项目为VaCant.EFCore也就是EfCoreBaseDbContext 所在的项目
            //然后在nuget程序控制台分别执行下面两行命令
            //Add-Migration InitialCreate -c EfCoreBaseDbContext
            // Update-Database -c EfCoreBaseDbContext
            var optionsBuilder = new DbContextOptionsBuilder<EfCoreBaseDbContext>();
            optionsBuilder.UseSqlServer("Server=.; Database=ServerDb; Trusted_Connection=True;uid=sa;pwd=123456;",b=>b.MigrationsAssembly("VaCant.EFCore"));

            return new EfCoreBaseDbContext(optionsBuilder.Options);
        }
    }
}
