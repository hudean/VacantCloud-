using Microsoft.EntityFrameworkCore;
using MyCoreMvc.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCoreMvc.EFCore
{
    public class SqlServerDbContext : DbContext
    {
        //private string ConnectionString { get; set; }
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
        { 
        
        }

        //public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options, string connectionString) : base(options)
        //{
        //    ConnectionString = connectionString;
        //}

        /// <summary>
        /// 重写 OnModelCreating 利用反射获取命名空间下的所有的public类，此处可以注入一个标记来识别是否是实体类。将所有实体放入EF中
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //不想写DbSet 用反射的获取所有的实体类
            var assembly = Assembly.Load("");
            foreach (Type type in assembly.ExportedTypes)
            {
                if (type.IsClass && (type.Name != "xxx")&& !type.IsAbstract)
                {
                    var method = modelBuilder.GetType().GetMethods().Where(x => x.Name == "Entity").FirstOrDefault();
                    if (method != null)
                    {
                        method = method.MakeGenericMethod(new Type[] { type });
                        method.Invoke(modelBuilder, null);
                    }
                }
            }
            base.OnModelCreating(modelBuilder);


        }
        /// <summary>
        /// 重写 OnConfiguring 方法，指定数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    //连接数据库-一般不用
        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}

        /// <summary>
        /// public void Detach() 方法是在SaveChanges后更改EF追踪实体标记的，用来取消追踪的 => Added、Detached、UnChanged… 将所有Save后的对象的标记改成Detached。
        /// </summary>
        public void Detach()
        {
            ChangeTracker.Entries().ToList().ForEach(aEntry =>
            {
                var temp = aEntry;
                if (aEntry.State != EntityState.Detached)
                    aEntry.State = EntityState.Detached;
            });
        }

        /// <summary>
        /// 重写 SaveChanges() 方法，此处EF在保存前都会进行变更追踪，所以优化是在保存前先关闭，在保存后开启。ChangeTracker.AutoDetectChangesEnabled = false/true;
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            int count = base.SaveChanges();
            Detach();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return count;
        }

        /// <summary>
        /// 重写 SaveChanges() 方法，此处EF在保存前都会进行变更追踪，所以优化是在保存前先关闭，在保存后开启。ChangeTracker.AutoDetectChangesEnabled = false/true;
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            int count = await base.SaveChangesAsync(cancellationToken);
            Detach();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return count;
        }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }

        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public virtual DbSet<Menu> Menu { get; set; }

        public virtual DbSet<Tenant> Tenant { get; set; }

        //不想写DbSet 用反射的获取所有的实体类
        


    }
}
