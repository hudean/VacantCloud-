using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyCoreMvc.EFCore
{
    public class MySqlDbContext : DbContext
    {
        private string ConnectionString { get; set; }
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        { 
        
        }
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options, string connectionString) : base(options)
        {
            ConnectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString);


        // optionsBuilder.UseSqlServer(ConnectionString, p => p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout));
        // optionsBuilder.AddInterceptors(new DbCommandCustomInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region
            Assembly entityAssembly = Assembly.Load(new AssemblyName("YiSha.Entity"));
            IEnumerable<Type> typesToRegister = entityAssembly.GetTypes().Where(p => !string.IsNullOrEmpty(p.Namespace))
                                                                         .Where(p => !string.IsNullOrEmpty(p.GetCustomAttribute<TableAttribute>()?.Name));
            foreach (Type type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Model.AddEntityType(type);
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //PrimaryKeyConvention.SetPrimaryKey(modelBuilder, entity.Name);
                string currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName);

                //var properties = entity.GetProperties();
                //foreach (var property in properties)
                //{
                //    ColumnConvention.SetColumnName(modelBuilder, entity.Name, property.Name);
                //}
            }

            #endregion
            base.OnModelCreating(modelBuilder);
        }
      
    }
}
