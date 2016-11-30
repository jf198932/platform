using Abp.EntityFramework;
using isriding.Authen.Permission;

namespace isriding.EntityFramework.Repositories.Authen.Permission
{
    public class PermissionWriteRepository : isridingRepositoryBase<Entities.Authen.Permission, int>, IPermissionWriteRepository
    {
        public PermissionWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}