using Abp.EntityFramework;
using isriding.VersionUpdate;

namespace isriding.EntityFramework.Repositories.VersionUpdate
{
    public class VersionUpdateWriteRepository : isridingRepositoryBase<Entities.VersionUpdate, int>, IVersionUpdateWriteRepository
    {
        public VersionUpdateWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}