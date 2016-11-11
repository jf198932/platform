using Abp.Domain.Repositories;

namespace isriding.Track
{
    public interface ITrackWriteRepository : IRepository<Entities.Track, int>
    {
         
    }
}