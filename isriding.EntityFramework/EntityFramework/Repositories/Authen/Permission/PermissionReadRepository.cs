using Abp.EntityFramework;
using isriding.Authen.Permission;

namespace isriding.EntityFramework.Repositories.Authen.Permission
{
    public class PermissionReadRepository : ReadonlyisridingRepositoryBase<Entities.Authen.Permission, int>, IPermissionReadRepository
    {
        public PermissionReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}