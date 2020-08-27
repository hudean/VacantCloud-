using MyCoreMvc.EFCore;
using MyCoreMvc.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreMvc.Repositorys
{
    public class MysqlRepository<TEntity> : MysqlRepository<TEntity, long>, IRepository<TEntity> where TEntity : class, IEntity<long>
    {
        public MysqlRepository(MySqlDbContext dbContext) : base(dbContext)
        {

        }
    }


    public class MysqlRepository<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public readonly MySqlDbContext _dbContext;

        public MysqlRepository(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
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
