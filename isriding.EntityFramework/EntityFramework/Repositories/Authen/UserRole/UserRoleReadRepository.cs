using Abp.EntityFramework;
using isriding.Authen.UserRole;

namespace isriding.EntityFramework.Repositories.Authen.UserRole
{
    public class UserRoleReadRepository : ReadonlyisridingRepositoryBase<Entities.Authen.UserRole, int>, IUserRoleReadRepository
    {
        public UserRoleReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}