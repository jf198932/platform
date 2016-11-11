using Abp.EntityFramework;
using isriding.Authen.ModulePermission;

namespace isriding.EntityFramework.Repositories.Authen.ModulePermission
{
    public class ModulePermissionWriteRepository : isridingRepositoryBase<Entities.Authen.ModulePermission, int>, IModulePermissionWriteRepository
    {
        public ModulePermissionWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}