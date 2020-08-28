using Microsoft.EntityFrameworkCore;
using MyCoreMvc.EFCore;
using MyCoreMvc.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace MyCoreMvc.Repositorys
{
    public class SqlServerRepository<TEntity> : SqlServerRepository<TEntity, long>, IRepository<TEntity> where TEntity : class, IEntity<long>
    {
        public SqlServerRepository(SqlServerDbContext dbContext) : base(dbContext)
        {

        }
    }

    public class SqlServerRepository<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public readonly SqlServerDbContext _dbContext;

        public SqlServerRepository(SqlServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //public virtual DbTransaction Transaction
        //{
        //    get
        //    {   //安装ef6才行
        //        //return ((DbContextTransaction)_dbContext.Database.CurrentTransaction).UnderlyingTransaction;
        //        //return (DbTransaction)TransactionProvider?.GetActiveTransaction(new ActiveTransactionProviderArgs
        //        //{
        //        //    {"ContextType", typeof(SqlServerDbContext) },
        //        //    {"MultiTenancySide", MultiTenancySide }
        //        //});
        //    }
        //}

        public virtual DbConnection Connection
        {
            get
            {
                var connection = _dbContext.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }
        }





        public override void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = _dbContext.Set<TEntity>().SingleOrDefault(t => t.Id.Equals(id));
            Delete(entity);

        }

        public override IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }

        public override TEntity Insert(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public override TEntity Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            _dbContext.SaveChanges();
            return entity;
        }
    }
}
