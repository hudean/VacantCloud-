using Microsoft.EntityFrameworkCore;
using MyCoreMvc.Entitys;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCoreMvc.Repositorys.EFCore
{





    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }
        IDisposable Use(CancellationToken cancellationToken);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext">包含的DbContext <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">该存储库的实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">实体的主键</typeparam>
    public class EfCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
       // public readonly DbContext Context;
        public EfCoreRepositoryBase(TDbContext dbContext)
        {
            Context = dbContext;
        }
        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public TDbContext Context;


        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        /// <summary>
        /// Gets DbQuery for given entity.
        /// </summary>
        public virtual DbSet<TEntity> DbQueryTable => Context.Set<TEntity>();



        private static readonly ConcurrentDictionary<Type, bool> EntityIsDbQuery =
           new ConcurrentDictionary<Type, bool>();


        protected virtual IQueryable<TEntity> GetQueryable()
        {
            if (EntityIsDbQuery.GetOrAdd(typeof(TEntity), key => Context.GetType().GetProperties().Any(property =>
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbQuery<>)) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                        typeof(IEntity<>)) &&
                    property.PropertyType.GetGenericArguments().Any(x => x == typeof(TEntity)))))
            {
                return DbQueryTable.AsQueryable();
            }

            return Table.AsQueryable();
        }

        public ICancellationTokenProvider CancellationTokenProvider { get; set; }



        public virtual DbConnection Connection
        {
            get
            {
                var connection = Context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }
        }



        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync(CancellationTokenProvider.Token);
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync(CancellationTokenProvider.Token);
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate, CancellationTokenProvider.Token);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id), CancellationTokenProvider.Token);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate, CancellationTokenProvider.Token);
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                await Context.SaveChangesAsync(CancellationTokenProvider.Token);
            }

            return entity.Id;
        }

        public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                await Context.SaveChangesAsync(CancellationTokenProvider.Token);
            }

            return entity.Id;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = Update(entity);
            return Task.FromResult(entity);
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            //Could not found the entity, do nothing.
        }

        public override async Task<int> CountAsync()
        {
            return await GetAll().CountAsync(CancellationTokenProvider.Token);
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync(CancellationTokenProvider.Token);
        }

        public override async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync(CancellationTokenProvider.Token);
        }

        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync(CancellationTokenProvider.Token);
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }


        public Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Collection(collectionExpression).LoadAsync(cancellationToken);
        }

        public void EnsureCollectionLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            Context.Entry(entity).Collection(collectionExpression).Load();
        }

        public Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }

        public void EnsurePropertyLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            Context.Entry(entity).Reference(propertyExpression).Load();
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        private static bool MayHaveTemporaryKey(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(byte))
            {
                return true;
            }

            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(entity.Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(entity.Id) <= 0;
            }

            return false;
        }


        /**************************************************/
        //public override void Delete(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void Delete(TPrimaryKey id)
        //{
        //    throw new NotImplementedException();
        //}

        //public override IQueryable<TEntity> GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public override TEntity Insert(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public override TEntity Update(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
