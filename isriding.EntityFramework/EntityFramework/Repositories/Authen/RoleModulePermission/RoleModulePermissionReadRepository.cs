using Abp.EntityFramework;
using isriding.Authen.RoleModulePermission;

namespace isriding.EntityFramework.Repositories.Authen.RoleModulePermission
{
    public class RoleModulePermissionReadRepository : ReadonlyisridingRepositoryBase<Entities.Authen.RoleModulePermission, int>, IRoleModulePermissionReadRepository
    {
        public RoleModulePermissionReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}