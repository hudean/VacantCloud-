using VaCant.EFCore;
using VaCant.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaCant.Repositorys.EFCore
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    //public abstract class VacantRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<MySqlDbContext, TEntity, TPrimaryKey>
    //    where TEntity : class, IEntity<TPrimaryKey>
    //{
    //    protected VacantRepositoryBase(MySqlDbContext dbContextProvider)
    //        : base(dbContextProvider)
    //    {
    //    }

    //    // Add your common methods for all repositories
    //}

    ///// <summary>
    ///// Base class for custom repositories of the application.
    ///// This is a shortcut of <see cref="VacantRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    ///// </summary>
    ///// <typeparam name="TEntity">Entity type</typeparam>
    //public abstract class VacantRepositoryBase<TEntity> : VacantRepositoryBase<TEntity, long>, IRepository<TEntity>
    //    where TEntity : class, IEntity<long>
    //{
    //    protected VacantRepositoryBase(MySqlDbContext dbContextProvider)
    //        : base(dbContextProvider)
    //    {
    //    }

    //    // Do not add any method here, add to the class above (since this inherits it)!!!
    //}
}
