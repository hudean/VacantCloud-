using Microsoft.EntityFrameworkCore;
using VaCant.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaCant.Repositorys.EFCore
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
