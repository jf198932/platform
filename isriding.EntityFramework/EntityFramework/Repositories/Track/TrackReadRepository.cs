using Abp.EntityFramework;
using isriding.Track;

namespace isriding.EntityFramework.Repositories.Track
{
    public class TrackReadRepository : ReadonlyisridingRepositoryBase<Entities.Track, int>, ITrackReadRepository
    {
        public TrackReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}