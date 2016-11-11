using Abp.EntityFramework;
using isriding.Authen.Role;

namespace isriding.EntityFramework.Repositories.Authen.Role
{
    public class RoleReadRepository : ReadonlyisridingRepositoryBase<Entities.Authen.Role, int>, IRoleReadRepository
    {
        public RoleReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}