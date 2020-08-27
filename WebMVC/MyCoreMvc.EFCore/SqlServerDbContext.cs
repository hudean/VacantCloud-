using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    //连接数据库-一般不用
        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}
    }
}
