using Abp.EntityFramework;
using isriding.Track;

namespace isriding.EntityFramework.Repositories.Track
{
    public class TrackWriteRepository : isridingRepositoryBase<Entities.Track, int>, ITrackWriteRepository
    {
        public TrackWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}