using Abp.EntityFramework;
using isriding.Sitemonitor;

namespace isriding.EntityFramework.Repositories.Sitemonitor
{
    public class SitemonitorReadRepository : ReadonlyisridingRepositoryBase<Entities.Sitemonitor, int>, ISitemonitorReadRepository
    {
        public SitemonitorReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}