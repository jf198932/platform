using Abp.EntityFramework;
using isriding.Bikesite;

namespace isriding.EntityFramework.Repositories.Bikesite
{
    public class BikesiteReadRepository : ReadonlyisridingRepositoryBase<Entities.Bikesite,int>, IBikesiteReadRepository
    {
        public BikesiteReadRepository(IDbContextProvider<ReadonlyisridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}