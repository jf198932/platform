using Abp.EntityFramework;
using isriding.Bikesite;

namespace isriding.EntityFramework.Repositories.Bikesite
{
    public class BikesiteWriteRepository : isridingRepositoryBase<Entities.Bikesite,int>, IBikesiteWriteRepository
    {
        public BikesiteWriteRepository(IDbContextProvider<isridingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}