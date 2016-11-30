using Abp.EntityFramework;
using isriding.Authen.RoleModulePermission;

namespace isriding.EntityFramework.Repositories.Authen.RoleModulePermission
{
    public class RoleModulePermissionWriteRepository : isridingRepositoryBase<Entities.Authen.RoleModulePermission, int>, IRoleModulePermissionWriteRepository
    {
        public RoleModulePermissionWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}