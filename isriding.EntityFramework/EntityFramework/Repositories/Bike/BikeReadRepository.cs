using Abp.EntityFramework;
using isriding.Bike;

namespace isriding.EntityFramework.Repositories.Bike
{
    public class BikeReadRepository : ReadonlyisridingRepositoryBase<Entities.Bike, int>, IBikeReadRepository
    {
        public BikeReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}