using Microsoft.EntityFrameworkCore;
using MyCoreMvc.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreMvc.Repositorys.EFCore
{
    public class EfCoreRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, long>, IRepository<TEntity>
       where TEntity : class, IEntity<long>
       where TDbContext : DbContext
    {
        public EfCoreRepositoryBase(TDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
