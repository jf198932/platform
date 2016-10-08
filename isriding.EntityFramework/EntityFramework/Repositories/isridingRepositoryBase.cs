using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace isriding.EntityFramework.Repositories
{
    public abstract class isridingRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<isridingDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected isridingRepositoryBase(IDbContextProvider<isridingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class isridingRepositoryBase<TEntity> : isridingRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected isridingRepositoryBase(IDbContextProvider<isridingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
