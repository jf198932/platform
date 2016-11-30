using Abp.EntityFramework;
using isriding.VersionUpdate;

namespace isriding.EntityFramework.Repositories.VersionUpdate
{
    public class VersionUpdateReadRepository : ReadonlyisridingRepositoryBase<Entities.VersionUpdate, int>, IVersionUpdateReadRepository
    {
        public VersionUpdateReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}