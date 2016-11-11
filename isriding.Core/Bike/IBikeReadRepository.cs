using Abp.Domain.Repositories;

namespace isriding.Bike
{
    public interface IBikeReadRepository : IRepository<Entities.Bike, int>
    {
         
    }
}