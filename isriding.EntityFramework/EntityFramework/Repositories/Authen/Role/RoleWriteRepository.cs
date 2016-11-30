using Abp.EntityFramework;
using isriding.Authen.Role;

namespace isriding.EntityFramework.Repositories.Authen.Role
{
    public class RoleWriteRepository : isridingRepositoryBase<Entities.Authen.Role, int>, IRoleWriteRepository
    {
        public RoleWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}