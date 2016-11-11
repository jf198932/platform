using Abp.EntityFramework;
using isriding.Authen.UserRole;

namespace isriding.EntityFramework.Repositories.Authen.UserRole
{
    public class UserRoleWriteRepository : isridingRepositoryBase<Entities.Authen.UserRole, int>, IUserRoleWriteRepository
    {
        public UserRoleWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}