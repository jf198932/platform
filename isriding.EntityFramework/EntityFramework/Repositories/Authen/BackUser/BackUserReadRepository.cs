using Abp.EntityFramework;
using isriding.Authen.BackUser;

namespace isriding.EntityFramework.Repositories.Authen.BackUser
{
    public class BackUserReadRepository: ReadonlyisridingRepositoryBase<Entities.Authen.BackUser, int>, IBackUserReadRepository
    {
        public BackUserReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}