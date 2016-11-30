using Abp.EntityFramework;
using isriding.Authen.ModulePermission;

namespace isriding.EntityFramework.Repositories.Authen.ModulePermission
{
    public class ModulePermissionReadRepository : ReadonlyisridingRepositoryBase<Entities.Authen.ModulePermission, int>, IModulePermissionReadRepository
    {
        public ModulePermissionReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}