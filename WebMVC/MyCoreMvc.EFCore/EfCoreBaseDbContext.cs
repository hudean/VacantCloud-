using Microsoft.EntityFrameworkCore;
using VaCant.Common;
using VaCant.Common.EncryptionHelper;
using VaCant.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace VaCant.EFCore
{
    public class EfCoreBaseDbContext : DbContext
    {
        //private string ConnectionString { get; set; }
        public EfCoreBaseDbContext(DbContextOptions<EfCoreBaseDbContext> options) : base(options)
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
            base.OnModelCreating(modelBuilder);
            //不想写DbSet 用反射的获取所有的实体类
            //var assembly = Assembly.Load("");
            //foreach (Type type in assembly.ExportedTypes)
            //{
            //    if (type.IsClass && (type.Name != "xxx") && !type.IsAbstract)
            //    {
            //        var method = modelBuilder.GetType().GetMethods().Where(x => x.Name == "Entity").FirstOrDefault();
            //        if (method != null)
            //        {
            //            method = method.MakeGenericMethod(new Type[] { type });
            //            method.Invoke(modelBuilder, null);
            //        }
            //    }
            //}



            AddSeedData(modelBuilder);
        }

        /// <summary>
        /// 添加种子数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void AddSeedData(ModelBuilder modelBuilder)
        {
            string salt = GuidHelper.GetGuidString(4);
            //参考文章https://www.cnblogs.com/cgzl/p/9868501.html和官方文档
            //https://docs.microsoft.com/zh-cn/ef/core/modeling/data-seeding
            //生成的迁移类 命令：Add-Migration Xxx
            //迁移到数据库 命令：Update-Database -Verbose
            //种子数据为什么要指定主键的值？
            //因为在团队开发时，这样可以确保不同的开发人员、电脑、服务器上，在同一个迁移版本具有相同的种子数据。
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Address = "深圳市宝安区",
                CreationTime = DateTime.Now,
                CreatorUserId = null,
                Email = "1468553034@qq.com",
                IsDelete = false,
                LastLoginErrorDateTime = null,
                LoginErrorTimes = 0,
                Password ="123qwe", //MyEncryptionHelper.Md5Hash(salt + "123qwe"),
                PasswordSalt = salt,
                PhoneNum = "13538631840",
                UserName = "admin",
                IsActive = true
            });
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                RoleName = "Admin",
                Description = "Admin",
                DisplayName = "Admin",
                NormalizedName = "Admin",
                CreationTime = DateTime.Now,
                CreatorUserId = null,
                DeleterUserId = null,
                IsDelete = false,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierUserId = null
                
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                Id = 1,
                RoleId = 1,
                UserId = 1
            });
            modelBuilder.Entity<Permission>().HasData(new Permission
            {
                Id = 1,
                Description = "系统权限",
                PermissionName = "Pages.Tenants",
                Value = ""
            }, new Permission
            {
                Id = 2,
                Description = "系统权限",
                PermissionName = "Pages.Users",
                Value = ""
            }, new Permission
            {
                Id = 3,
                Description = "系统权限",
                PermissionName = "Pages.Roles",
                Value = ""
            });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission
            {
                Id = 1,
                RoleId = 1,
                PermissionId = 1
            }, new RolePermission
            {
                Id = 2,
                RoleId = 1,
                PermissionId = 2
            }, new RolePermission
            {
                Id = 3,
                RoleId = 1,
                PermissionId = 3
            });

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
