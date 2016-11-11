using Abp.EntityFramework;
using isriding.Bike;

namespace isriding.EntityFramework.Repositories.Bike
{
    public class BikeWriteRepository : isridingRepositoryBase<Entities.Bike, int>, IBikeWriteRepository
    {
        public BikeWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}