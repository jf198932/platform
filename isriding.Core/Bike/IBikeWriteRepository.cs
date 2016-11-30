using Abp.Domain.Repositories;

namespace isriding.Bike
{
    public interface IBikeWriteRepository : IRepository<Entities.Bike, int>
    {
         
    }
}