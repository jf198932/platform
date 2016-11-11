using Abp.Domain.Repositories;

namespace isriding.Track
{
    public interface ITrackReadRepository : IRepository<Entities.Track, int>
    {
         
    }
}