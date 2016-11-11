using Abp.EntityFramework;
using isriding.Sitemonitor;

namespace isriding.EntityFramework.Repositories.Sitemonitor
{
    public class SitemonitorWriteRepository : isridingRepositoryBase<Entities.Sitemonitor, int>, ISitemonitorWriteRepository
    {
        public SitemonitorWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}